using System;

namespace EconSim.logic;

public class Town
{
    private String name;
    private Inventory inventory;
    
    public Town(String name)
    {
        this.name = name;
        inventory = new Inventory();
    }
    
    public Inventory getInventory() { return inventory; }
}