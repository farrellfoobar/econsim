using System;
using System.Collections.Generic;

namespace EconSim.logic;

public class Inventory
{
    private Dictionary<ItemType, int> inventory = new Dictionary<ItemType, int>();
    
    public void addItems(ItemType item, int amount) {
        if( inventory.ContainsKey(item) ) {
            inventory[item] += amount;
        } else {
            inventory[item] = amount;
        }
    }

    public double getItemCount(ItemType item) {
        int ret = 0;
        if( inventory.ContainsKey(item) ) {
            ret = inventory[item];
        }

        return ret;
    }

    public override string ToString() {
        String str = "<";
        foreach (ItemType itemType in Enum.GetValues(typeof (ItemType))) {
            if (inventory.ContainsKey(itemType))
                str += itemType + ":" + inventory[itemType] + ", ";
        }
        str += ">";
        return str;
    }
}

public enum ItemType
{
    NONE,
    GRAIN,
    WOOD,
    LUMBER,
    FISH,
}