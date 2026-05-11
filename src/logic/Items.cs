using System.Collections.Generic;

namespace EconSim.logic;

public class Items
{
    public static readonly List<ItemType> ALL_FOOD_ITEMS = new List<ItemType> { ItemType.GRAIN, ItemType.FISH }; 
    public static readonly List<ItemType> ALL_DESIRED_ITEMS = new List<ItemType> { ItemType.GRAIN, ItemType.FISH };
    
    public static Dictionary<ItemType, int> BASE_PRICE = new Dictionary<ItemType, int> {
        { ItemType.GRAIN, 1 },
        { ItemType.WOOD, 10 },
        { ItemType.FURNITURE, 20 },
        { ItemType.FISH, 2 },
        { ItemType.BEER, 3 },
        { ItemType.SILVER_ORE, 20 },
        { ItemType.JEWELRY, 100 },
    };
}

public enum ItemType
{
    NONE,
    GRAIN,
    FISH,
    BEER,
    
    WOOD,
    FURNITURE,
    
    SILVER_ORE,
    JEWELRY,
}