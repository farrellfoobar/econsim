using System;
using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

public class Market
{
    private readonly Dictionary<ItemType, int> BASE_PRICE = new Dictionary<ItemType, int> {
        { ItemType.GRAIN, 1 },
        { ItemType.WOOD, 10 },
        { ItemType.FURNITURE, 20 },
        { ItemType.FISH, 2 },
        { ItemType.BEER, 3 },
        { ItemType.SILVER_ORE, 20 },
        { ItemType.JEWELRY, 100 },
    };
    
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

    public int sellItems(ItemType itemType, int quantity) {
        int cost = 0;
        for (int i = 0; i < quantity; i++) {
            cost += sellItem(itemType);
        }
        
        return cost;
    }

    public Optional<int> tryBuyItems(ItemType itemType, int quantity) {
        if (quantity > inventory.getItemCount(itemType)) {
            return Optional<int>.EMPTY();
        }

        int cost = 0;
        for (int i = 0; i < quantity; i++) {
            cost += buyItem(itemType);
        }
        
        return new Optional<int>(cost);
    }

    private int sellItem(ItemType itemType) {
        inventory.addItems(itemType, 1);
        itemSupplyDemandHistory[itemType].addSupply(turnManager.getTurnCount());
        
        return getPrice(itemType);
    }

    private int buyItem(ItemType itemType) {
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

    public int getPrice(ItemType itemType) {
        int totalDemand = itemSupplyDemandHistory[itemType].getTotalDemand();
        int totalSupply = itemSupplyDemandHistory[itemType].getTotalSupply();
        int supplyDemandFactor = 1;
        if (totalSupply != 0 && totalDemand != 0) {
            supplyDemandFactor = totalSupply / totalDemand;
        }
        int price = BASE_PRICE[itemType] * supplyDemandFactor;
        
        return price;
    }
    
    public override string ToString() {
        String str = "<";
        foreach (ItemType itemType in Enum.GetValues(typeof (ItemType))) {
            if (inventory.ContainsItem(itemType))
                str += itemType + ":" + inventory.getItemCount(itemType) + ":$" + getPrice(itemType) + ", ";
        }
        str += ">";
        return str;
    }
}