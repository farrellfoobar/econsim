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

    public Simulation(GameMap gamemapFromGodotEditor, bool debug = false)
    {
        gameMap = gamemapFromGodotEditor;
        Town sili = new Town("San Silicio", 40, turnAndTimeManager);
        Town burg = new Town("Burgherville", 20, turnAndTimeManager);
        Town soko = new Town("Sokotra", 60, turnAndTimeManager);
        
        gameMap.AssignTowns(new List<Town> { sili, burg, soko });
        
        sili.GetInventory().AddItems(ItemType.Grain, 10000);
        sili.GetInventory().AddItems(ItemType.Wood, 100);
        //sili.GetInventory().AddItems(ItemType.Fish, 10000);
        
        burg.GetInventory().AddItems(ItemType.Grain, 10000);
        burg.GetInventory().AddItems(ItemType.Wood, 1000);
       // burg.GetInventory().AddItems(ItemType.Fish, 10000);
        
        soko.GetInventory().AddItems(ItemType.Grain, 10000);
        soko.GetInventory().AddItems(ItemType.Wood, 100);
        //soko.GetInventory().AddItems(ItemType.Fish, 10000);
        
        Agent agent = new ProfitSeekingAgent(sili, gameMap);
        Agent agentTwo = new ProfitSeekingAgent(burg, gameMap);
        agents = new List<Agent> {agent, agentTwo};
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
            town.DoLaborersTurn();
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