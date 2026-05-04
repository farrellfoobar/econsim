using Godot;
using Vector2 = System.Numerics.Vector2;

namespace EconSim;

public class GameTile
{
    public TileType tileType;
    public Vector2I position;
    
    public GameTile(TileType tileType, Vector2I position)
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