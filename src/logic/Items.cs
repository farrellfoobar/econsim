using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

public class Items
{
    public static readonly List<ItemType> ALL_FOOD_ITEMS = new List<ItemType> { ItemType.GRAIN, ItemType.FISH }; 
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