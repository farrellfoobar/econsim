using System;
using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

//TODO: PPPY, CPUP -> double maybe depending on implementation?
public class BuildingInitialValues(ItemType produces, ItemType consumes, double pppy, double cpup, CoinAmount wppy)
{
    public ItemType ITEM_PRODUCED { get; init; } = produces;
    public ItemType ITEM_CONSUMED { get; init; } = consumes;
    
    public double PRODUCTION_PER_PERSONYEAR { get; init; } = pppy;   
    public double CONSUMPTION_PER_UNIT_PRODUCED { get; init; } = cpup;

    public CoinAmount WAGE_PER_PERSONYEAR { get; init; } = wppy;
}

public class SimulationConstants
{
    public static BuildingInitialValues BreweryValues = new BuildingInitialValues(
        ItemType.BEER, ItemType.GRAIN, 
        1404, // 9*12 packs (10 gallons) a day, cleaning every other day = 9*3*52=1404 
        2.5, // 2.378 lb grain/Liter * 2.13Liter/6pack ~= 5, halved since beer was way weeker back then (/ballance)
        CoinAmount.Silver(8)
        );
    
    public static BuildingInitialValues CarpentryYardValues = new BuildingInitialValues(
        ItemType.FURNITURE, ItemType.WOOD, 
        18, // one every three weeks
        2, //see BASE_PRICE[ItemType.Furniture] declaration
        CoinAmount.Silver(15)
    );
    
    public static BuildingInitialValues JewelryValues = new BuildingInitialValues(
        ItemType.JEWELRY, ItemType.SILVER_ORE, 
        12, //one a month
        1, // one ring ~= 4 grams ~= CoinAmount.Silver(1) := about 3.5grams silver
        CoinAmount.Silver(18)
    );
    
    private const int FOOD_CONSUMPTION_PER_PERSON_YEAR = 20;
    public const int FOOD_CONSUMPTION_PER_TURN = FOOD_CONSUMPTION_PER_PERSON_YEAR / TurnAndTimeManager.TURNS_IN_A_YEAR;

    public static BuildingInitialValues SubsistanceFarmValues = new BuildingInitialValues(
        ItemType.GRAIN, ItemType.NONE , 20, 0, new CoinAmount(0)
    );
    
    public static Dictionary<ItemType, CoinAmount> BASE_PRICE = new Dictionary<ItemType, CoinAmount> {
        
        //1 copper ~= $1
        { ItemType.GRAIN, CoinAmount.Copper(3) }, //$3 = enough for a lb of flour
        { ItemType.BEER, CoinAmount.Copper(12) }, //$12 = a six pack
        
        { ItemType.FISH, CoinAmount.Copper(10) }, //$10 = a 1lb fillet
        
        //todo: rename wood -> lumber
        { ItemType.WOOD, CoinAmount.Copper(50) }, //$50 = 50 board feet 
        { ItemType.FURNITURE, CoinAmount.Silver(5) }, //$500 = a big ass dining table, ~= 100 board feet of wood
        
        { ItemType.SILVER_ORE, CoinAmount.Silver(1) }, // $100 = a big chunk of ore, i.e. enough for a coin or rings
        { ItemType.JEWELRY, CoinAmount.Silver(6) }, // $600 = a fancy silver band ring 
        
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
    
    public static Dictionary<ItemType, double> DEMAND_ELASTICITY = new Dictionary<ItemType, double> {
        { ItemType.GRAIN, 0 },
        { ItemType.BEER, 0 },
        
        { ItemType.FISH, 0 },
        
        { ItemType.WOOD, 0 },
        { ItemType.FURNITURE, 0 },
        
        { ItemType.SILVER_ORE, 0 },
        { ItemType.JEWELRY, 0 },
    };
    
    public static Dictionary<ItemType, double> DEMAND_SLOPE = new Dictionary<ItemType, double> {
        { ItemType.GRAIN, getDemandSlope(ItemType.GRAIN) },
        { ItemType.BEER, getDemandSlope(ItemType.BEER) },
        
        { ItemType.FISH, getDemandSlope(ItemType.FISH) },
        
        { ItemType.WOOD, getDemandSlope(ItemType.WOOD) },
        { ItemType.FURNITURE, getDemandSlope(ItemType.FURNITURE) },
        
        { ItemType.SILVER_ORE, getDemandSlope(ItemType.SILVER_ORE) },
        { ItemType.JEWELRY, getDemandSlope(ItemType.JEWELRY) },
    };

    private static double getDemandSlope(ItemType itemType) {
        //We define: QuantityofDemand(price) = m/price^elasticity 
        //Thus: m = QuantityofDemand * price^elasticity
        //We define QD(basePrice) = baseDemand
        // m = baseDemand * basePrice^elasticity
        
        double baseDemand = BASE_DEMAND[itemType];
        double basePrice = BASE_PRICE[itemType].asDouble();
        double elasticity = DEMAND_ELASTICITY[itemType];
        
        double m = baseDemand * Math.Pow(basePrice, elasticity);

        return m;
    }
}