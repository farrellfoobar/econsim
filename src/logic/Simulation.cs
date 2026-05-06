using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;


public class Simulation
{
    private GameMap gameMap;
    private List<Merchant> merchants;
    
    public Simulation()
    {
        gameMap = new GameMap(27, 13);
        
        Town sili = new Town("San Silicio");
        Town burg = new Town("Burgherville");
        Town soko = new Town("Sokotra");
        
        gameMap.addTown(new Vector2Int(3, 3), sili);
        gameMap.addTown(new Vector2Int(13, 7), burg);
        gameMap.addTown(new Vector2Int(20, 9), soko);
        
        sili.getInventory().addItem(ItemType.GRAIN, 1000);
        sili.getInventory().addItem(ItemType.WOOD, 100);
        sili.getInventory().addItem(ItemType.FISH, 1000);
        
        burg.getInventory().addItem(ItemType.GRAIN, 1000);
        burg.getInventory().addItem(ItemType.WOOD, 1000);
        burg.getInventory().addItem(ItemType.FISH, 1000);
        
        soko.getInventory().addItem(ItemType.GRAIN, 100);
        soko.getInventory().addItem(ItemType.WOOD, 1000);
        soko.getInventory().addItem(ItemType.FISH, 1000);
        // This setup should get the merchant to trade GRAIN for WOOD from sili to soko without using mind control (setOnJourneyTo) 

        Merchant thisOneGuy = new Merchant(new Vector2Int(3, 3), gameMap);
        merchants = new List<Merchant> {thisOneGuy};
        
        thisOneGuy.setOnJourneyTo(new Vector2Int(20, 9));
    }

    public void doTurn()
    {
        foreach (Merchant merchant in merchants)
        {
            merchant.DoTurn();
        }
    }
    
    public GameMap getGameMap() { return gameMap; }
    public List<Merchant> getMerchants() { return merchants; }
    
}