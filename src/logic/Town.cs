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
    
    public Town(String name, int population, TurnAndTimeManager turnAndTimeManager) {
        this.population = population;
        this.unemployedPopulation = population;
        this.name = name;
        this.buildings = new List<Building>();
        buildings.Add(new SubsistanceFarm(this, turnAndTimeManager));
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

    public void addBuilding(Building building) {
        buildings.Add(building);
    }

    public int getUnemployedPopulation() {
        return unemployedPopulation;
    }

    public int getPopulation() {
        return population;
    }

    public void setUnemployedPopulation(int unemployedPopulation) {
        this.unemployedPopulation = unemployedPopulation;
    }
}