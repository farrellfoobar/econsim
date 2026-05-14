using System;
using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

public class Market
{
    
    private Inventory inventory = new Inventory();
    private TurnAndTimeManager turnManager;
    private Dictionary<ItemType, MarketHistory> itemSupplyDemandHistory = new Dictionary<ItemType, MarketHistory>();

    public Market(TurnAndTimeManager turnManager) {
        this.turnManager = turnManager;
        foreach (ItemType itemType in Enum.GetValues(typeof(ItemType))) {
            itemSupplyDemandHistory[itemType] = new MarketHistory();
        }
    }

    public CoinAmount SellItems(ItemType itemType, int quantity) {
        CoinAmount cost = new CoinAmount(0);
        for (int i = 0; i < quantity; i++) {
            cost.Add(sellItem(itemType));
        }
        
        return cost;
    }

    public Optional<CoinAmount> TryBuyItems(ItemType itemType, int quantity) {
        if (quantity > inventory.GetItemCount(itemType)) {
            return Optional<CoinAmount>.Empty();
        }

        CoinAmount cost = new CoinAmount();
        for (int i = 0; i < quantity; i++) {
            cost.Add(buyItem(itemType));
        }
        
        return new Optional<CoinAmount>(cost);
    }

    private CoinAmount sellItem(ItemType itemType) {
        inventory.AddItems(itemType, 1);
        itemSupplyDemandHistory[itemType].AddSupply(turnManager.GetTurnCount());
        
        return GetPrice(itemType);
    }

    private CoinAmount buyItem(ItemType itemType) {
        inventory.RemoveItems(itemType, 1);
        itemSupplyDemandHistory[itemType].AddDemand(turnManager.GetTurnCount());
        
        return GetPrice(itemType);
    }
    
    public Inventory GetInventory() {return inventory;}

    public void DoTurn(int turnCount) {
        foreach (ItemType itemType in itemSupplyDemandHistory.Keys) {
            itemSupplyDemandHistory[itemType].CullSupplyDemandHistory(turnCount);
        }
    }

    public virtual CoinAmount GetPrice(ItemType itemType) {
        CoinAmount price = CoinAmount.GetMultiplyBy(
            SimulationConstants.BasePrice[itemType], 
            getSupplyDemandFactor(itemType)
        );
        
        return price;
    }

    public bool IsInStock(ItemType itemType) {
        return inventory.GetItemCount(itemType) > 0;
    }

    private double getSupplyDemandFactor(ItemType itemType) {
        double totalDemand = itemSupplyDemandHistory[itemType].GetTotalDemand();
        double totalSupply = itemSupplyDemandHistory[itemType].GetTotalSupply();
        double supplyDemandFactor = 1;
        if (totalSupply != 0 && totalDemand != 0) {
            supplyDemandFactor = totalSupply / totalDemand;
        }

        return supplyDemandFactor;
    }
    
    public override string ToString() {
        String str = "<";
        foreach (ItemType itemType in Enum.GetValues(typeof (ItemType))) {
            if (inventory.ContainsItem(itemType)) {
                str += itemType + ":" + inventory.GetItemCount(itemType) + ":" + GetPrice(itemType);

                if (SimpleLogger.IsDebug)
                    str += ":S/D=" + itemSupplyDemandHistory[itemType].GetTotalSupply() + "/" +
                           itemSupplyDemandHistory[itemType].GetTotalDemand();
                    
                str += ", ";
            }
        }
        str += ">";
        return str;
    }
}