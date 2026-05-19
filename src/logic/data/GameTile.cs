using System;
using EconSim.data;

namespace EconSim.logic;


public class GameTile
{
    private TileType tileType;
    private Vector2Int position;
    private StructureType structure;
    
    public GameTile(TileType tileType, Vector2Int position, StructureType structure = StructureType.None)
    {
        this.tileType = tileType;
        this.position = position;
        this.structure = structure;
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

    public StructureType GetStructureType()
    {
        return structure;
    }
    
    public void SetStructureType(StructureType structure)
    {
        this.structure = structure;
    }
}

public enum TileType { //values from tile indexes
    Grass = 0,
    Path = 13,
    River = 3,
}

public enum StructureType
{
    None = -1,
    Hamlet = 22,
}