using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

public class Items
{
    public static readonly List<ItemType> ALL_FOOD_ITEMS = new List<ItemType> { ItemType.GRAIN, ItemType.FISH }; 
    public static readonly List<ItemType> ALL_DESIRED_ITEMS = new List<ItemType> { ItemType.GRAIN, ItemType.FISH };
    
    public static Dictionary<ItemType, CoinAmount> BASE_PRICE = new Dictionary<ItemType, CoinAmount> {
        { ItemType.FISH, CoinAmount.Silver(4) },
        
        { ItemType.GRAIN, CoinAmount.Silver(1) },
        { ItemType.BEER, CoinAmount.Silver(3) },
        
        { ItemType.WOOD, CoinAmount.Silver(5) },
        { ItemType.FURNITURE, CoinAmount.Silver(20) },
        
        { ItemType.SILVER_ORE, CoinAmount.Silver(1000) },
        { ItemType.JEWELRY, CoinAmount.Silver(3000) },
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