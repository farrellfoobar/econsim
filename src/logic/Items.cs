using System.Collections.Generic;

namespace EconSim.logic;

public class Items
{
    public static readonly List<ItemType> ALL_FOOD_ITEMS = new List<ItemType> { ItemType.GRAIN, ItemType.FISH }; 
    public static readonly List<ItemType> ALL_DESIRED_ITEMS = new List<ItemType> { ItemType.GRAIN, ItemType.FISH };
}

public enum ItemType
{
    NONE,
    GRAIN,
    WOOD,
    FURNITURE,
    FISH,
    BEER,
    SILVER_ORE,
    JEWELRY,
}