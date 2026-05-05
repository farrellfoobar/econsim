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
        gameMap.setTileType(new Vector2Int(3, 3), TileType.HAMLET);
        gameMap.setTileType(new Vector2Int(13, 7), TileType.HAMLET);
        gameMap.setTileType(new Vector2Int(20, 9), TileType.HAMLET);
        
        merchants = new List<Merchant> {new Merchant(new Vector2Int(3,3))};
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