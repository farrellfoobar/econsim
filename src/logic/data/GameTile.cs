using System;
using EconSim.data;
using Godot;

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

    public int GetWagonPathfindingWeight()
    {
        int ret = 0;
        switch (tileType) {
            case TileType.River:
                ret = 5;
                break;
            case TileType.Ocean:
                ret = int.MaxValue;
                break;
            default:
                ret = 1;
                break;
        }

        return ret;
    }
    
    public int GetBoatPathfindingWeight()
    {
        int ret = 0;
        switch (tileType) {
            case TileType.River:
                ret = 1;
                break;
            case TileType.Ocean:
                ret = 1;
                break;
            default:
                ret = int.MaxValue;
                break;
        }

        if (structure.Equals(StructureType.Hamlet)) {
            ret = 1; //so boats can go from a river next to town to a town, maybe TODO: replace with dock structure in river, updating pathfinding logic
        }
        
        return ret;
    }
    
    public bool IsWagonPassable()
    {
        return !tileType.Equals(TileType.Ocean);
    }
    
    public bool IsBoatPassable()
    {
        bool ret = tileType.Equals(TileType.Ocean) || tileType.Equals(TileType.River) || structure.Equals(StructureType.Hamlet);
        
        return ret;
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
    Ocean = 23,
}

public enum StructureType
{
    None = -1,
    Hamlet = 22,
}