using EconSim.data;

namespace EconSim;

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
    GRASS = 1,
    PATH = 13,
    HAMLET = 14,
}