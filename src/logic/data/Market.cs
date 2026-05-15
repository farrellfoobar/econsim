using System;
using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

public enum PurchaseResult
{
    Success,
    FailedNotInStock,
    FailedCantAfford,
}

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
        
        price = price.IsLessThan(CoinAmount.MinValue) ? CoinAmount.MinValue : price;
        
        return price;
    }
    
    public void SellItems(CoinAmount wealth, ItemType itemType, int quantity)
    {
        wealth.Add(CoinAmount.GetMultiplyBy(GetPrice(itemType), quantity));
        addItems(itemType, quantity);
    }

    public PurchaseResult TryBuyItems(CoinAmount wealth, ItemType itemType, int quantity) {
        PurchaseResult result = PurchaseResult.Success;
        CoinAmount cost = CoinAmount.GetMultiplyBy(GetPrice(itemType), quantity);
        
        if (quantity > inventory.GetItemCount(itemType)) {
            result = PurchaseResult.FailedNotInStock;
        }
        else if (!wealth.IsGreaterThan(cost)) {
            result = PurchaseResult.FailedCantAfford;
        }
        else {
            removeItems(itemType, quantity);
            wealth.Subtract(cost);
        }
        
        return result;
    }

    private void addItems(ItemType itemType, int quantity) {
        inventory.AddItems(itemType, quantity);
        itemSupplyDemandHistory[itemType].AddSupply(turnManager.GetTurnCount(), quantity);
    }

    private void removeItems(ItemType itemType, int quantity) {
        inventory.RemoveItems(itemType, quantity);
        itemSupplyDemandHistory[itemType].AddDemand(turnManager.GetTurnCount(), quantity);
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
    
    public bool IsInStock(ItemType itemType, int quantity) {
        return inventory.GetItemCount(itemType) >= quantity;
    }
    
    public Inventory GetInventory() {return inventory;}
    
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