using System;
using System.Collections.Generic;
using System.Linq;
using EconSim.data;
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
        String ret = name;

        double unemployment = (double) unemployedPopulation.Count / getPopulationCount();
        ret += " - " + unemployment.ToString("P0") + " unemployment";
        
        ret += " - Market: " + market;
        
        return ret ;
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

    public String getWealthDistribution() {
        List<Laborer> dist = allPopulation.OrderBy(laborer => laborer.getWealth().asDouble()).OrderDescending().ToList();

        int third = dist.Count/3;

        double highestThirdWealth = 0;
        for (int i = 0; i < third; i++) {
            highestThirdWealth+=dist[i].getWealth().asDouble();
        }
        
        double middleThirdWealth = 0;
        for (int i = third; i < third*2; i++) {
            middleThirdWealth+=dist[i].getWealth().asDouble();
        }
        
        double lowThirdWealth = 0;
        for (int i = third*2; i < dist.Count-1; i++) {
            lowThirdWealth+=dist[i].getWealth().asDouble();
        }
        
        double totalWealth = highestThirdWealth + middleThirdWealth + lowThirdWealth;
        
        double highestShare = highestThirdWealth/totalWealth;
        double middleShare = middleThirdWealth/totalWealth;
        double lowShare = lowThirdWealth/totalWealth;

        return "WD:<"
               + lowShare.ToString("P0") + ","
               + middleShare.ToString("P0") + ","
               + highestShare.ToString("P0") + ","
               + ">";
    }
}