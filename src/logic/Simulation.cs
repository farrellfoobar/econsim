using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;


public class Simulation
{
    private GameMap gameMap;
    private List<Merchant> merchants;
    private AStarPathfinder pathfinder;
    
    public Simulation()
    {
        gameMap = new GameMap(27, 13);
        pathfinder = new AStarPathfinder(gameMap);
        gameMap.addTown(new Vector2Int(3, 3), new Town());
        gameMap.addTown(new Vector2Int(13, 7), new Town());
        gameMap.addTown(new Vector2Int(20, 9), new Town());

        Merchant thisOneGuy = new Merchant(new Vector2Int(3, 3), pathfinder);
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