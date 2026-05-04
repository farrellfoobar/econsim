using Godot;

namespace EconSim;

public class Simulation
{
    private GameMap gameMap;
    
    public Simulation(Node parent)
    {
        gameMap = new GameMap(27, 13, parent);
        gameMap.setTileType(new Vector2I(3, 3), TileType.HAMLET);
        gameMap.setTileType(new Vector2I(13, 7), TileType.HAMLET);
        gameMap.setTileType(new Vector2I(20, 9), TileType.HAMLET);
    }
}