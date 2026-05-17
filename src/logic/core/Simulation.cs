using System;
using System.Collections.Generic;
using EconSim.data;
using EconSim.logic.buildings;

namespace EconSim.logic;


public class Simulation
{
    private GameMap gameMap;
    private List<Agent> agents;
    private TurnAndTimeManager turnAndTimeManager = new TurnAndTimeManager();

    public Simulation(bool debug = false)
    {
        gameMap = new GameMap(27, 13);
        
        Town sili = new Town("San Silicio", 40, new Vector2Int(3, 3), turnAndTimeManager);
        Town burg = new Town("Burgherville", 20, new Vector2Int(13, 7), turnAndTimeManager);
        Town soko = new Town("Sokotra", 60, new Vector2Int(20, 9), turnAndTimeManager);
        
        gameMap.AddTown(sili);
        gameMap.AddTown(burg);
        gameMap.AddTown(soko);
        
        sili.GetInventory().AddItems(ItemType.Grain, 10000);
        sili.GetInventory().AddItems(ItemType.Wood, 100);
        sili.GetInventory().AddItems(ItemType.Fish, 10000);
        Building siliBrewery = new Brewery(sili);
        siliBrewery.EmployWorkers(3);
        sili.AddBuilding(siliBrewery);
        
        burg.GetInventory().AddItems(ItemType.Grain, 10000);
        burg.GetInventory().AddItems(ItemType.Wood, 1000);
        burg.GetInventory().AddItems(ItemType.Fish, 10000);

        Building burgLumberYard = new CarpentryYard(burg); 
        burg.AddBuilding(burgLumberYard);
        burgLumberYard.EmployWorkers(12);

        Building burgJeweler = new Jeweler(burg); 
        burg.AddBuilding(burgJeweler);
        burgJeweler.EmployWorkers(4);
        
        soko.GetInventory().AddItems(ItemType.Grain, 10000);
        soko.GetInventory().AddItems(ItemType.Wood, 100);
        soko.GetInventory().AddItems(ItemType.Fish, 10000);
        Building sokoBrewery = new Brewery(sili);
        sokoBrewery.EmployWorkers(10);
        soko.AddBuilding(sokoBrewery);
        
        Agent agent = new Agent(sili, gameMap);
        agents = new List<Agent> {agent};
    }

    public void DoTurn() {
        Console.WriteLine("TURN " + turnAndTimeManager.GetTurnCount() + " YEAR " + turnAndTimeManager.GetYear());
        turnAndTimeManager.NextTurn();
        
        foreach (Agent agent in agents)
        {
            agent.DoTurn();
        }
        
        foreach (Town town in gameMap.GetTowns()) {
            Console.WriteLine(town);
            town.DoProductionTurn();
            town.DoConsumptionTurn();
        }
    }

    public List<Merchant> GetMerchants()
    {
        List<Merchant> merchants = new List<Merchant>();
        foreach (Agent agent in agents) {
            foreach (Merchant agentMerchants in agent.GetMerchants()) {
                merchants.Add(agentMerchants);
            }
        }
        
        return merchants;
    }
    
    public GameMap GetGameMap() { return gameMap; }
    
}