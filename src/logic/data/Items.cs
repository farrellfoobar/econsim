using System.Collections.Generic;

namespace EconSim.logic;

public class Items
{
    public static readonly List<ItemType> AllFoodItems = new List<ItemType> { ItemType.Grain, ItemType.Fish }; 
    public static readonly List<ItemType> ALL_ITEMS = new List<ItemType>
    {
        ItemType.Grain, ItemType.Fish, ItemType.Beer, ItemType.Wood, ItemType.Furniture, ItemType.SilverOre, ItemType.Jewelry
    };
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