using System;
using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

public class Market
{
    
    private Inventory inventory = new Inventory();
    private TurnAndTimeManager turnManager;
    private Dictionary<ItemType, MarketHistory> itemSupplyDemandHistory = new Dictionary<ItemType, MarketHistory>();
        
    private int lastTurnSeen = 0;

    public Market(TurnAndTimeManager turnManager) {
        this.turnManager = turnManager;
        foreach (ItemType itemType in Enum.GetValues(typeof(ItemType))) {
            itemSupplyDemandHistory[itemType] = new MarketHistory();
        }
    }

    public CoinAmount sellItems(ItemType itemType, int quantity) {
        CoinAmount cost = new CoinAmount(0);
        for (int i = 0; i < quantity; i++) {
            cost.add(sellItem(itemType));
        }
        
        return cost;
    }

    public Optional<CoinAmount> tryBuyItems(ItemType itemType, int quantity) {
        if (quantity > inventory.getItemCount(itemType)) {
            return Optional<CoinAmount>.EMPTY();
        }

        CoinAmount cost = new CoinAmount();
        for (int i = 0; i < quantity; i++) {
            cost.add(buyItem(itemType));
        }
        
        return new Optional<CoinAmount>(cost);
    }

    private CoinAmount sellItem(ItemType itemType) {
        inventory.addItems(itemType, 1);
        itemSupplyDemandHistory[itemType].addSupply(turnManager.getTurnCount());
        
        return getPrice(itemType);
    }

    private CoinAmount buyItem(ItemType itemType) {
        inventory.removeItems(itemType, 1);
        itemSupplyDemandHistory[itemType].addDemand(turnManager.getTurnCount());
        
        return getPrice(itemType);
    }
    
    public Inventory getInventory() {return inventory;}

    public void doTurn(int turnCount) {
        foreach (ItemType itemType in itemSupplyDemandHistory.Keys) {
            itemSupplyDemandHistory[itemType].cullSupplyDemandHistory(turnCount);
        }
    }

    public CoinAmount getPrice(ItemType itemType) {
        double totalDemand = itemSupplyDemandHistory[itemType].getTotalDemand();
        double totalSupply = itemSupplyDemandHistory[itemType].getTotalSupply();
        double supplyDemandFactor = 1;
        if (totalSupply != 0 && totalDemand != 0) {
            supplyDemandFactor = totalSupply / totalDemand;
        }
        CoinAmount price = CoinAmount.getMultiplyBy(SimulationConstants.BASE_PRICE[itemType], supplyDemandFactor);
        
        if(price.asDouble() < 1E-3)
            Console.WriteLine("!!! 0= " +  totalSupply + " / " + totalDemand);
        
        return price;
    }
    
    public override string ToString() {
        String str = "<";
        foreach (ItemType itemType in Enum.GetValues(typeof (ItemType))) {
            if (inventory.ContainsItem(itemType))
                str += itemType + ":" + inventory.getItemCount(itemType) + ":" + getPrice(itemType) + ", ";
        }
        str += ">";
        return str;
    }
}