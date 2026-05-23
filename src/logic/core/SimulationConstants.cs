using System;
using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

public class BuildingInitialValues(ItemType produces, ItemType consumes, double pppy, double cpup, CoinAmount wppy)
{
    public ItemType ItemProduced { get; init; } = produces;
    public ItemType ItemConsumed { get; init; } = consumes;
    
    public double ProductionPerPersonyear { get; init; } = pppy;   
    public double ConsumptionPerUnitProduced { get; init; } = cpup;

    public CoinAmount WagePerPersonyear { get; init; } = wppy;
}

public class SimulationConstants
{
    public static BuildingInitialValues BreweryValues = new BuildingInitialValues(
        ItemType.Beer, ItemType.Grain, 
        1404, // 9*12 packs (10 gallons) a day, cleaning every other day = 9*3*52=1404 
        2.5, // 2.378 lb grain/Liter * 2.13Liter/6pack ~= 5, halved since beer was way weeker back then (/ballance)
        CoinAmount.Silver(12)
        );
    
    public static BuildingInitialValues CarpentryYardValues = new BuildingInitialValues(
        ItemType.Furniture, ItemType.Wood, 
        18, // one every three weeks
        2, //see BASE_PRICE[ItemType.Furniture] declaration
        CoinAmount.Silver(15)
    );
    
    public static BuildingInitialValues JewelryValues = new BuildingInitialValues(
        ItemType.Jewelry, ItemType.SilverOre, 
        12, //one a month
        1, // one ring ~= 4 grams ~= CoinAmount.Silver(1) := about 3.5grams silver
        CoinAmount.Silver(18)
    );
    
    public static BuildingInitialValues SubsistanceFarmValues = new BuildingInitialValues(
        ItemType.Grain, ItemType.None , 100, 0, new CoinAmount(0)
    );
    
    public static CoinAmount BuildingStaringWealth = CoinAmount.Gold(1);
    
    public static Dictionary<ItemType, CoinAmount> BasePrice = new Dictionary<ItemType, CoinAmount> {
        
        //1 copper ~= $1
        { ItemType.Grain, CoinAmount.Copper(3) }, //$3 = enough for a lb of flour
        { ItemType.Beer, CoinAmount.Copper(12) }, //$12 = a six pack
        
        { ItemType.Fish, CoinAmount.Copper(10) }, //$10 = a 1lb fillet
        
        //todo: rename wood -> lumber
        { ItemType.Wood, CoinAmount.Copper(50) }, //$50 = 50 board feet 
        { ItemType.Furniture, CoinAmount.Silver(5) }, //$500 = a big ass dining table, ~= 100 board feet of wood
        
        { ItemType.SilverOre, CoinAmount.Silver(1) }, // $100 = a big chunk of ore, i.e. enough for a coin or rings
        { ItemType.Jewelry, CoinAmount.Silver(6) }, // $600 = a fancy silver band ring 
        
    };
    
// ############ EDIT THESE WITH CAUTION: they interact in a lot of complicated ways, use the spreadsheet to try them out first ############
    public const int MinFoodConsumptionPerPersonYear = 300;
    public const int FoodConsumptionPerTurn = MinFoodConsumptionPerPersonYear / TurnAndTimeManager.TurnsInAYear;
    public static Dictionary<ItemType, double> BaseDemand = new Dictionary<ItemType, double> {
        { ItemType.Grain, 200 },
        { ItemType.Beer, 730 },
        
        { ItemType.Fish, 100 },
        
        { ItemType.Wood, 0 },
        { ItemType.Furniture, 1 },
        
        { ItemType.SilverOre, 0 },
        { ItemType.Jewelry, 0.1 },
    };
    
    public static Dictionary<ItemType, double> DemandElasticity = new Dictionary<ItemType, double> {
        { ItemType.Grain, 0.03 },
        { ItemType.Beer, 0.5 },
        
        { ItemType.Fish, 0.3 },
        
        { ItemType.Wood, 0 },
        { ItemType.Furniture, 2 },
        
        { ItemType.SilverOre, 0 },
        { ItemType.Jewelry, 3 },
    };

    public static readonly CoinAmount AgentStartingWealth = CoinAmount.Gold(10);
    public static readonly int WAGON_GRAIN_CONSUMPTION_PER_TILE = 10;
    public static readonly int BASE_WAGON_CAPACITY = 6000;
    
    public static CoinAmount PovertyLineWealth = CoinAmount.GetMultiplyBy(
        BasePrice[ItemType.Grain],
        MinFoodConsumptionPerPersonYear
        );
    
    // ############ END EDIT WITH CAUTION ############
    
    private static double getDemandSlope(ItemType itemType) {
        //We define: QuantityofDemand(price) = m/price^elasticity 
        //Thus: m = QuantityofDemand * price^elasticity
        //We define QD(basePrice) = baseDemand
        // m = baseDemand * basePrice^elasticity
        
        double baseDemand = BaseDemand[itemType];
        double basePrice = BasePrice[itemType].AsInt();
        double elasticity = DemandElasticity[itemType];
        
        double m = baseDemand * Math.Pow(basePrice, elasticity);

        return m;
    }
    
    public static Dictionary<ItemType, double> DemandSlope = new Dictionary<ItemType, double> {
        { ItemType.Grain, getDemandSlope(ItemType.Grain) },
        { ItemType.Beer, getDemandSlope(ItemType.Beer) },
        
        { ItemType.Fish, getDemandSlope(ItemType.Fish) },
        
        { ItemType.Wood, getDemandSlope(ItemType.Wood) },
        { ItemType.Furniture, getDemandSlope(ItemType.Furniture) },
        
        { ItemType.SilverOre, getDemandSlope(ItemType.SilverOre) },
        { ItemType.Jewelry, getDemandSlope(ItemType.Jewelry) },
    };
}