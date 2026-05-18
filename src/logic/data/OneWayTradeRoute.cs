using System;
using System.Collections.Generic;
using System.Linq;
using EconSim.data;

namespace EconSim.logic;

public class OneWayTradeRoute
{
    private ItemType tradeItem;
    private int numExport;
    private Town toTown;
    private Town fromTown;
    private Stack<Vector2Int> path;
    
    public ItemType GetTradeItem(){ return tradeItem; }
    public int GetTradeQuantity(){ return numExport; }
    public Stack<Vector2Int> GetPath(){ return path; }
    public Town GetToTown(){ return toTown; }
    public Town GetFromTown(){ return fromTown; }

    public OneWayTradeRoute(Town fromTown, Town toTown, ItemType tradeItem, Stack<Vector2Int> path, int pathCost, CoinAmount maxCost)
    {
        this.toTown = toTown;
        this.fromTown = fromTown;
        this.tradeItem = tradeItem;
        this.path = path;
        this.numExport = getMaxQuantity(maxCost, pathCost);
    }
    
    public CoinAmount GetProfitFrom()
    {
        CoinAmount unitProfit = new CoinAmount(toTown.GetMarket().GetPrice(tradeItem));
        unitProfit.Subtract(fromTown.GetMarket().GetPrice(tradeItem));

        return CoinAmount.GetMultiplyBy(unitProfit, numExport);
    }

    public override bool Equals(object obj)
    {
        if (!(obj is OneWayTradeRoute))
            return false;
        
        OneWayTradeRoute that = (OneWayTradeRoute)obj;

        return tradeItem.Equals(that.tradeItem) &&
               numExport.Equals(that.numExport) &&
               toTown.Equals(that.toTown) &&
               fromTown.Equals(that.fromTown) &&
               path.SequenceEqual(that.path);
    }
    
    public override string ToString()
    {
        return numExport + " " + tradeItem + " from " + fromTown.getName() + " to " + toTown.getName() + " via " + String.Join(",", path);
    }

    private int getMaxQuantity(CoinAmount maxCost, int pathCost)
    {
        //TODO: this logic treats every object as weighing the same, which isnt necessarily true 
        int cargoCapacity = SimulationConstants.BASE_WAGON_CAPACITY -
                            pathCost * SimulationConstants.WAGON_GRAIN_CONSUMPTION_PER_TILE;
        
        int quantityCanAfford = maxCost.AsInt() / fromTown.GetMarket().GetPrice(tradeItem).AsInt();
        
        return Math.Min(Math.Min(cargoCapacity, quantityCanAfford), fromTown.GetMarket().GetStock(tradeItem));
    }
}