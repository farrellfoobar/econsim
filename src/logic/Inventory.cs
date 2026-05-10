using System;
using System.Collections.Generic;

namespace EconSim.logic;

public class Inventory
{
    private Dictionary<ItemType, int> inventory = new Dictionary<ItemType, int>();
    
    public void addItems(ItemType itemType, int amount) {
        if( inventory.ContainsKey(itemType) ) {
            inventory[itemType] += amount;
        } else {
            inventory[itemType] = amount;
        }
    }
    
    public void removeItems(ItemType itemType, int amount) {
        if (!inventory.ContainsKey(itemType) ||  inventory[itemType] < amount) {
            throw new ArgumentException("Tried to remove " + amount + " item, but only have " + inventory[itemType] + " items.");
        }
        
        inventory[itemType] -= amount;
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

    public bool ContainsItem(ItemType itemType) {
        return inventory.ContainsKey(itemType);
    }
}