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
    private int turnCount = 0;
    
    public Simulation()
    {
        gameMap = new GameMap(27, 13);
        
        Town sili = new Town("San Silicio", 40, turnAndTimeManager);
        Town burg = new Town("Burgherville", 20, turnAndTimeManager);
        Town soko = new Town("Sokotra", 60, turnAndTimeManager);
        
        gameMap.addTown(new Vector2Int(3, 3), sili);
        gameMap.addTown(new Vector2Int(13, 7), burg);
        gameMap.addTown(new Vector2Int(20, 9), soko);
        
        sili.getInventory().addItems(ItemType.GRAIN, 1000);
        sili.getInventory().addItems(ItemType.WOOD, 100);
        sili.getInventory().addItems(ItemType.FISH, 1000);
        
        burg.getInventory().addItems(ItemType.GRAIN, 1000);
        burg.getInventory().addItems(ItemType.WOOD, 1000);
        burg.getInventory().addItems(ItemType.FISH, 1000);
        Building burgLumberYard = new LumberYard(burg); 
        burg.addBuilding(burgLumberYard);
        burgLumberYard.employWorkers(4);
        
        soko.getInventory().addItems(ItemType.GRAIN, 100);
        soko.getInventory().addItems(ItemType.WOOD, 100);
        soko.getInventory().addItems(ItemType.FISH, 1000);
        // This setup should get the merchant to trade GRAIN for WOOD from sili to soko without using mind control (setOnJourneyTo) 

        Merchant thisOneGuy = new Merchant(new Vector2Int(3, 3), gameMap);
        merchants = new List<Merchant> {thisOneGuy};
        
        thisOneGuy.setOnJourneyTo(new Vector2Int(20, 9));
    }

    public void doTurn() {
        turnAndTimeManager.nextTurn();
        Console.WriteLine("TURN " + turnAndTimeManager.getTurnCount());
        
        foreach (Merchant merchant in merchants)
        {
            merchant.DoTurn();
        }
        
        foreach (Town town in gameMap.getTowns()) {
            town.doProductionTurn();
            town.doConsumptionTurn();
            Console.WriteLine(town);
        }
    }
    
    public GameMap getGameMap() { return gameMap; }
    public List<Merchant> getMerchants() { return merchants; }
    
}