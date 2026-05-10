using System;
using System.Collections.Generic;
using EconSim.logic.buildings;

namespace EconSim.logic;

public class Town
{
    private String name;
    private Market market;
    private List<Building> buildings;
    private Stack<Laborer> unemployedPopulation;
    private List<Laborer> allPopulation;
    private TurnAndTimeManager turnAndTimeManager;
    
    public Town(String name, int population, TurnAndTimeManager turnAndTimeManager) {
        unemployedPopulation = new Stack<Laborer>(population);
        allPopulation = new List<Laborer>(population);
        for (int i = 0; i < population; i++) {
            Laborer laborer = new Laborer();
            unemployedPopulation.Push(laborer);
            allPopulation.Add(laborer);
        }
        this.turnAndTimeManager = turnAndTimeManager;
        this.name = name;
        this.buildings = new List<Building>();

        SubsistenceFarm subsistenceFarm = new SubsistenceFarm(this, turnAndTimeManager);
        subsistenceFarm.setEmployees(unemployedPopulation);
        buildings.Add(subsistenceFarm);
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
    
    public void doConsumptionTurn() {
        foreach (Laborer person in allPopulation) {
            person.consumeAtMarket(market);
        }
    }

    public override String ToString() {
        return name + " - Market: " + market;
    }

    public void addBuilding(Building building) {
        buildings.Add(building);
    }

    public int getUnemployedPopulationCount() {
        return unemployedPopulation.Count;
    }
    
    public Stack<Laborer> getUnemployedPopulation() {
        return unemployedPopulation;
    }

    public int getPopulationCount() {
        return allPopulation.Count;
    }

    public List<Laborer> getAllPopulation() {
        return allPopulation;
    }
}