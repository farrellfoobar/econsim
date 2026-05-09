using System;
using System.Collections.Generic;
using EconSim.logic.buildings;

namespace EconSim.logic;

public class Town
{
    private String name;
    private Market market;
    private List<Building> buildings;
    private int population;
    private int unemployedPopulation;
    private TurnAndTimeManager turnAndTimeManager;
    
    public Town(String name, int population, TurnAndTimeManager turnAndTimeManager) {
        this.population = population;
        this.unemployedPopulation = population;
        this.turnAndTimeManager = turnAndTimeManager;
        this.name = name;
        this.buildings = new List<Building>();
        buildings.Add(new SubsistanceFarm(this, turnAndTimeManager));
        buildings[0].addWorker(unemployedPopulation);
        market = new Market(turnAndTimeManager);
    }
    
    /*
     * Basically just for setup now, should be using the market to exchange between agents
     */
    public Inventory getInventory() { return market.getInventory(); }
    
    public Market getMarket() { return market; }

    public void doProductionTurn() {
        market.doTurn(turnAndTimeManager.getTurnCount());
        foreach (Building building in buildings) {
            building.doProductionTurn();
        }
    }

    public override String ToString() {
        return name + " - Market: " + market;
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