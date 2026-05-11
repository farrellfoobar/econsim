using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

//TODO: PPPY, CPUP -> double maybe depending on implementation?
public class BuildingInitialValues(ItemType produces, ItemType consumes, int pppy, int cpup, CoinAmount wppy)
{
    public ItemType ITEM_PRODUCED { get; init; } = produces;
    public ItemType ITEM_CONSUMED { get; init; } = consumes;
    
    public int PRODUCTION_PER_PERSONYEAR { get; init; } = pppy;   
    public int CONSUMPTION_PER_UNIT_PRODUCED { get; init; } = cpup;

    public CoinAmount WAGE_PER_PERSONYEAR { get; init; } = wppy;
}

public class SimulationConstants
{
    public static BuildingInitialValues BreweryValues = new BuildingInitialValues(
        ItemType.BEER, ItemType.GRAIN, 1000, 1, new CoinAmount(60)
        );
    
    public static BuildingInitialValues CarpentryYardValues = new BuildingInitialValues(
        ItemType.WOOD, ItemType.FURNITURE, 60, 2, new CoinAmount(60)
    );
    
    public static BuildingInitialValues JewelryValues = new BuildingInitialValues(
        ItemType.SILVER_ORE, ItemType.JEWELRY, 24, 2, new CoinAmount(120)
    );
    
    public static BuildingInitialValues SubsistanceFarmValues = new BuildingInitialValues(
        ItemType.NONE, ItemType.GRAIN, 20, 0, new CoinAmount(0)
    );
    
    public static Dictionary<ItemType, CoinAmount> BASE_PRICE = new Dictionary<ItemType, CoinAmount> {
        { ItemType.FISH, CoinAmount.Silver(4) },
        
        { ItemType.GRAIN, CoinAmount.Silver(1) },
        { ItemType.BEER, CoinAmount.Silver(3) },
        
        { ItemType.WOOD, CoinAmount.Silver(5) },
        { ItemType.FURNITURE, CoinAmount.Silver(20) },
        
        { ItemType.SILVER_ORE, CoinAmount.Silver(1000) },
        { ItemType.JEWELRY, CoinAmount.Silver(3000) },
    };
    
    public static Dictionary<ItemType, double> BASE_DEMAND = new Dictionary<ItemType, double> {
        { ItemType.GRAIN, 15 },
        { ItemType.BEER, 5 },
        
        { ItemType.FISH, 5 },
        
        { ItemType.WOOD, 0 },
        { ItemType.FURNITURE, 5 },
        
        { ItemType.SILVER_ORE, 0 },
        { ItemType.JEWELRY, 5 },
    };
    
    
}