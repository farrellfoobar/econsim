using System;
using System.Collections.Generic;
using System.Linq;
using EconSim.data;
using EconSim.logic.buildings;
using EconSim.tst;

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
        subsistenceFarm.SetEmployees(unemployedPopulation);
        buildings.Add(subsistenceFarm);
        market = new Market(turnAndTimeManager);
    }
    
    /*
     * Basically just for setup now, should be using the market to exchange between agents
     */
    public Inventory GetInventory() { return market.GetInventory(); }
    
    public Market GetMarket() { return market; }

    public void DoProductionTurn() {
        market.DoTurn(turnAndTimeManager.GetTurnCount());
        foreach (Building building in buildings) {
            building.DoProductionTurn();
        }
    }
    
    public void DoConsumptionTurn() {
        foreach (Laborer person in allPopulation) {
            person.ConsumeAtMarket(market);
        }
    }

    public override String ToString() {
        String ret = name;

        double unemployment = (double) unemployedPopulation.Count / GetPopulationCount();
        ret += " - " + unemployment.ToString("P0") + " unemployment";
        
        ret += " - Market: " + market;

        ret += " - Wealth: " + GetWealthDistribution();
        
        return ret ;
    }

    public void AddBuilding(Building building) {
        buildings.Add(building);
    }

    public int GetUnemployedPopulationCount() {
        return unemployedPopulation.Count;
    }
    
    public Stack<Laborer> GetUnemployedPopulation() {
        return unemployedPopulation;
    }

    public int GetPopulationCount() {
        return allPopulation.Count;
    }

    public List<Laborer> GetAllPopulation() {
        return allPopulation;
    }

    public String GetWealthDistribution() {
        List<Laborer> dist = allPopulation.OrderBy(laborer => laborer.GetWealth().AsInt()).ToList();

        int third = dist.Count/3;

        double highestThirdWealth = 0;
        for (int i = 0; i < third; i++) {
            highestThirdWealth+=dist[i].GetWealth().AsInt();
        }
        
        double middleThirdWealth = 0;
        for (int i = third; i < third*2; i++) {
            middleThirdWealth+=dist[i].GetWealth().AsInt();
        }
        
        double lowThirdWealth = 0;
        for (int i = third*2; i < dist.Count-1; i++) {
            lowThirdWealth+=dist[i].GetWealth().AsInt();
        }
        
        double totalWealth = highestThirdWealth + middleThirdWealth + lowThirdWealth;
        
        double highestShare = highestThirdWealth/totalWealth;
        double middleShare = middleThirdWealth/totalWealth;
        double lowShare = lowThirdWealth/totalWealth;

        return "<"
               + lowShare.ToString("P0") + "@" + dist[third].GetWealth() +","
               + middleShare.ToString("P0") + "@" + dist[third*2].GetWealth() +","
               + highestShare.ToString("P0") + "@" + dist[(third*3)-1].GetWealth() +","
               + ">";
    }

    /*
     * For testing
     */
    public void SetMarket(FixedPriceMarket fixedPriceMarket) {
        this.market = fixedPriceMarket;
    }
}