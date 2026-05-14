using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

public class Items
{
    public static readonly List<ItemType> AllFoodItems = new List<ItemType> { ItemType.Grain, ItemType.Fish }; 
}

public enum ItemType
{
    None,
    Grain,
    Fish,
    Beer,
    
    Wood,
    Furniture,
    
    SilverOre,
    Jewelry,
}