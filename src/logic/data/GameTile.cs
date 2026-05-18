using EconSim.data;

namespace EconSim.logic;


public class GameTile
{
    private TileType tileType;
    private Vector2Int position;
    
    public GameTile(TileType tileType, Vector2Int position)
    {
        this.tileType = tileType;
        this.position = position;
    }

    public Vector2Int GetPosition() {
        return position;
    }

    public void SetTileType(TileType tileType)
    {
        this.tileType = tileType;
    }

    public TileType GetTileType()
    {
        return tileType;
    }

    public int GetPathfindingWeight()
    {
        return 1;
    }
    
    public bool IsPassable() {
        return true;
    }
}

public enum TileType { //values from tile indexes
    Grass = 1,
    Path = 13,
    Hamlet = 14,
}