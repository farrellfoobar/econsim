using System;
using System.Collections.Generic;
using EconSim.logic.buildings;

namespace EconSim.logic;

public class Town
{
    private String name;
    private Inventory inventory;
    private List<Building> buildings;
    private int population;
    private int unemployedPopulation;
    
    public Town(String name, int population) {
        this.population = population;
        this.unemployedPopulation = population;
        this.name = name;
        this.buildings = new List<Building>();
        buildings.Add(new SubsistanceFarm(this));
        buildings[0].addWorker(unemployedPopulation);
        inventory = new Inventory();
    }
    
    public Inventory getInventory() { return inventory; }

    public void doProductionTurn() {
        foreach (Building building in buildings) {
            building.doProductionTurn();
        }
    }

    public override String ToString() {
        return name + " - Market: " + inventory;
    }
}