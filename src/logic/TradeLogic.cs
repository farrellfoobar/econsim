using System;
using System.Collections.Generic;
using System.Linq;
using EconSim.data;

namespace EconSim.logic;

public class TradeLogic
{
    //todo: take distance into account in getMostProfitableTradeRoute, possibly also desired 'home' of the merchant
    GameMap gameMap;
    private List<Town> towns;
    
    public TradeLogic(GameMap gameMap)
    {
        this.gameMap = gameMap;
        towns = gameMap.getTowns();
    }

    public Vector2Int getMostProfitableTradeRouteFrom(Town startTown)
    {
        /*
         * getAveragePrices(), getLocalPrices(), buy best averagePrice/localPrice
         */
        Dictionary<ItemType, double> itemAveragePrices = new Dictionary<ItemType, double>();
        Dictionary<ItemType, double> itemLocalPrices = new Dictionary<ItemType, double>();
        ItemType mostProfitable = ItemType.NONE;
        
        foreach (ItemType item in Enum.GetValues(typeof(ItemType))) {
            itemAveragePrices[item] = 0;
            foreach (Town town in towns) {
                itemAveragePrices[item] += town.getInventory().getItemCount(item); //This is count, not price, gotta figure out how tf to do that

                if (town.Equals(startTown)) {
                    itemLocalPrices[item] = town.getInventory().getItemCount(item);
                }
            }
            itemAveragePrices[item] /= (double) towns.Count; 
        }
        
        Dictionary<ItemType, double> returnOnInvestment = new Dictionary<ItemType, double>();
        foreach (KeyValuePair<ItemType, double> kvp in itemLocalPrices) {
            double averageItemPrice = itemAveragePrices[kvp.Key];
            double localItemPrice = kvp.Value;
            returnOnInvestment[kvp.Key] = averageItemPrice/localItemPrice;
        }

        ItemType bestROI = returnOnInvestment.OrderByDescending(kvp => kvp.Value).First().Key;
        
        /* This is where I realized I had a problem. Initially I didnt want merchants to 'magically' know all prices
         * everywhere, hence why I created the above algorithm to only know the average 'going' price of a good.
         * This is where I realized `getMostProfitableTradeRoute` is not `getMostProfitableTradeItem` and if I want to
         * avoid that 'magic' I need to either implement trade rumours and just have them choose at random at first, or
         * decide where to go some other way. 
         */
        return null;
    }
}