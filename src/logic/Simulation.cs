using System;
using System.Collections.Generic;
using EconSim.data;
using EconSim.logic.buildings;

namespace EconSim.logic;


public class Simulation
{
    private GameMap gameMap;
    private List<Merchant> merchants;
    private TurnAndTimeManager turnAndTimeManager = new TurnAndTimeManager();

    public Simulation(bool debug = false)
    {
        gameMap = new GameMap(27, 13);
        
        Town sili = new Town("San Silicio", 40, turnAndTimeManager);
        Town burg = new Town("Burgherville", 20, turnAndTimeManager);
        Town soko = new Town("Sokotra", 60, turnAndTimeManager);
        
        gameMap.AddTown(new Vector2Int(3, 3), sili);
        gameMap.AddTown(new Vector2Int(13, 7), burg);
        gameMap.AddTown(new Vector2Int(20, 9), soko);
        
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
        
        soko.GetInventory().AddItems(ItemType.Grain, 1000);
        soko.GetInventory().AddItems(ItemType.Wood, 100);
        soko.GetInventory().AddItems(ItemType.Fish, 10000);
        Building sokoBrewery = new Brewery(sili);
        sokoBrewery.EmployWorkers(10);
        soko.AddBuilding(sokoBrewery);

        Merchant thisOneGuy = new Merchant(new Vector2Int(3, 3), gameMap);
        merchants = new List<Merchant> {thisOneGuy};
        
        thisOneGuy.setOnJourneyTo(new Vector2Int(20, 9));
    }

    public void DoTurn() {
        Console.WriteLine("TURN " + turnAndTimeManager.GetTurnCount() + " YEAR " + turnAndTimeManager.GetYear());
        turnAndTimeManager.NextTurn();
        
        foreach (Merchant merchant in merchants)
        {
            merchant.DoTurn();
        }
        
        foreach (Town town in gameMap.GetTowns()) {
            Console.WriteLine(town);
            town.DoProductionTurn();
            town.DoConsumptionTurn();
        }
    }
    
    public GameMap GetGameMap() { return gameMap; }
    public List<Merchant> GetMerchants() { return merchants; }
    
}