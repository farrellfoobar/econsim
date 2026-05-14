using EconSim.data;

namespace EconSim.logic;


public class GameTile
{
    public TileType tileType;
    public Vector2Int position;
    
    public GameTile(TileType tileType, Vector2Int position)
    {
        this.tileType = tileType;
        this.position = position;
    }
    
}

public enum TileType { //values from tile indexes
    Grass = 1,
    Path = 13,
    Hamlet = 14,
}