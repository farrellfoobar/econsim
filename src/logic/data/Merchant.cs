using System;
using EconSim.data;

namespace EconSim.logic;


public class Merchant
{
    private Goal goal = Goal.None;
    private MerchantType type = MerchantType.None;
    private Vector2Int position;
    private TradeRoute tradeRoute;
    private Inventory inventory;
    private Agent parent;
    private GameMap map;

    public Merchant(Agent parent, TradeRoute tradeRoute, GameMap map, MerchantType type)
    {
        this.position = parent.GetPosition();
        this.tradeRoute = tradeRoute;
        this.inventory = new Inventory();
        this.parent = parent;
        this.map = map;
        this.type = type;
        goal = Goal.BuyExport;
    }
    
    public void DoTurn()
    {
        switch (goal) {
            case Goal.BuyExport:
                    buyForRoute(tradeRoute.GetExportRoute());
                    goal = Goal.GoToDestination;
                break;
            case Goal.GoToDestination:
                position = tradeRoute.GetExportPath().Pop();
                if (position.Equals(tradeRoute.GetToTown().GetPosition()))
                    goal = Goal.SellExportAndBuyImportFromDestination;
                break;
            case Goal.SellExportAndBuyImportFromDestination:
                tradeRoute.GetToTown().GetMarket().SellItems(parent.GetWealth(), tradeRoute.GetExportItem(), tradeRoute.GetExportQuantity());
                buyForRoute(tradeRoute.GetImportRoute());
                goal = Goal.GoBackToOrigin;
                break;
            case Goal.GoBackToOrigin:
                position = tradeRoute.GetImportPath().Pop();
                if (position.Equals(tradeRoute.GetFromTown().GetPosition()))
                    goal = Goal.SellImport;
                break;
            case Goal.SellImport:
                tradeRoute.GetFromTown().GetMarket().SellItems(parent.GetWealth(), tradeRoute.GetImportItem(), tradeRoute.GetImportQuantity());
                goal = Goal.None;
                break;
            default:
                parent.RemoveMerchant(this);
                break;
        }
        
    }
    
    public Vector2Int GetPosition()
    {
        return position;
    }

    public MerchantType GetType()
    {
        return type;
    }
    
    private void buyForRoute(OneWayTradeRoute route)
    {
        PurchaseResult purchaseResult = map.GetTownAt(position).GetMarket().TryBuyItems(parent.GetWealth(), route.GetTradeItem(), route.GetTradeQuantity());
        if(purchaseResult != PurchaseResult.Success)
            throw new Exception("Could not purchase items for trade route " + tradeRoute + " because " + purchaseResult + map.GetTownAt(position));
        
        inventory.AddItems(route.GetTradeItem(), route.GetTradeQuantity());
    }
}

public enum MerchantType {
    None, 
    Wagon,
    Boat,
}

public enum Goal{
    None,
    BuyExport,
    GoToDestination,
    SellExportAndBuyImportFromDestination,
    GoBackToOrigin,
    SellImport,
}