using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

public class TradeRoute
{
    private OneWayTradeRoute exportRoute;
    private OneWayTradeRoute importRoute;
    
    public TradeRoute(OneWayTradeRoute exportRoute, OneWayTradeRoute importRoute)
    {
        this.exportRoute = exportRoute;
        this.importRoute = importRoute;
    }
    
    public OneWayTradeRoute GetExportRoute()
    {
        return exportRoute;
    } 
    
    public OneWayTradeRoute GetImportRoute()
    {
        return importRoute;
    } 

    public override bool Equals(object that)
    {
        if (!(that is TradeRoute))
            return false;
        
        return exportRoute.Equals( ((TradeRoute)that).exportRoute ) && importRoute.Equals( ((TradeRoute)that).importRoute );
    }

    public override string ToString()
    {
        return exportRoute + " and back with " + importRoute.GetTradeQuantity() + " " + importRoute.GetTradeItem();
    }

    public CoinAmount GetProfitFrom()
    {
        CoinAmount profit = exportRoute.GetProfitFrom();
        profit.Add(importRoute.GetProfitFrom());
        return profit;
    }

    public Town GetFromTown()
    {
        return exportRoute.GetFromTown();
    }
    
    public Town GetToTown()
    {
        return exportRoute.GetToTown();
    }

    public Stack<Vector2Int> GetExportPath()
    {
        return exportRoute.GetPath();
    }

    public Stack<Vector2Int> GetImportPath()
    {
        return importRoute.GetPath();
    }

    public ItemType GetExportItem()
    {
        return exportRoute.GetTradeItem();
    }

    public ItemType GetImportItem()
    {
        return importRoute.GetTradeItem();
    }

    public int GetImportQuantity()
    {
        return importRoute.GetTradeQuantity();
    }

    public int GetExportQuantity()
    {
        return exportRoute.GetTradeQuantity();
    }
}