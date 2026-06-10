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
    private List<Laborer> unemployedPopulation;
    private List<Laborer> allPopulation;
    private TurnAndTimeManager turnAndTimeManager;
    private Vector2Int position;
    private SubsistenceFarm subsistenceFarm;
    private Random rand;
    
    public Town(String name, int population, Vector2Int position, TurnAndTimeManager turnAndTimeManager) {
        unemployedPopulation = new List<Laborer>(population);
        allPopulation = new List<Laborer>(population);
        for (int i = 0; i < population; i++) {
            Laborer laborer = new Laborer();
            unemployedPopulation.Add(laborer);
            allPopulation.Add(laborer);
        }
        this.turnAndTimeManager = turnAndTimeManager;
        this.name = name;
        this.position = position;
        this.buildings = new List<Building>();

        subsistenceFarm = new SubsistenceFarm(this, turnAndTimeManager);
        subsistenceFarm.SetEmployees(unemployedPopulation);
        buildings.Add(subsistenceFarm);
        market = new Market(turnAndTimeManager);
        rand = new Random();
    }
    
    /*
     * Basically just for setup now, should be using the market to exchange between agents
     */
    public Inventory GetInventory() { return market.GetInventory(); }
    public Market GetMarket() { return market; }
    public Vector2Int GetPosition() { return position; }

    public void DoProductionTurn() {
        market.DoTurn(turnAndTimeManager.GetTurnCount());
        subsistenceFarm.DoProductionTurn();
        foreach (Building building in buildings) {
            building.DoProductionTurn();
        }
    }
    
    public void DoLaborersTurn() {
        foreach (Laborer person in allPopulation) {
            person.DoTurn(this);
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
    
    public void BuildBuilding(CoinAmount wealth, Building building) {
        if (wealth.IsLessThan(building.GET_BUILD_COST())) {
            SimpleLogger.Log("Tried to build building that cant be afforded");
        }
        else {
            buildings.Add(building);
            wealth.Subtract(building.GET_BUILD_COST());
        }
    }

    public int GetUnemployedPopulationCount() {
        return unemployedPopulation.Count;
    }
    
    public List<Laborer> GetUnemployedPopulation() {
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

        return allPopulation.Count < 3 ? "Pop too low for distribution" : "<"
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

    public List<Building> GetBuildings()
    {
        return buildings;
    }
    
    public string getName()
    {
        return name;
    }

    public void SetUnemployed(Laborer laborer)
    {
        unemployedPopulation.Remove(laborer);
        subsistenceFarm.EmployWorker(laborer);
    }

    public void EmployLaborer(Laborer laborer)
    {
        List<Building> placesToWork = buildings;
        placesToWork.Remove(subsistenceFarm);

        placesToWork = placesToWork.Where(x => x.GetEmployeeCount() < x.GET_MAX_EMPLOYEES() && x.GetType() != typeof(SubsistenceFarm)).ToList();

        if (placesToWork.Count == 0) {
            return;
        }
        
        int buildingIndex = rand.Next(0, placesToWork.Count);
        
        placesToWork[buildingIndex].EmployWorker(laborer);
    }
}