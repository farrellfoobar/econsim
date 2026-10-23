using System;
using EconSim.data;
using Godot;

namespace EconSim.logic;


public class GameTile
{
    private TileType tileType;
    private Vector2Int position;
    private StructureType structureType;
    
    public GameTile(TileType tileType, Vector2Int position, StructureType structureType = StructureType.None)
    {
        this.tileType = tileType;
        this.position = position;
        this.structureType = structureType;
    }

    public Vector2Int GetPosition() {
        return position;
    }

    public int GetWagonPathfindingWeight()
    {
        int ret = 1;

        if (tileType.Equals(TileType.Sea) || tileType.Equals(TileType.ShallowSea)) {
            ret = int.MaxValue;
        } else if (structureType.Equals(StructureType.River)) {
            ret = 5;
        }

        return ret;
    }
    
    public int GetBoatPathfindingWeight()
    {
        int ret = Int32.MaxValue;
        
        if (tileType.Equals(TileType.Sea)) {
            ret = 5;
        } else if (tileType.Equals(TileType.ShallowSea) || structureType.Equals(StructureType.River) || structureType.Equals(StructureType.Hamlet)) {
            ret = 1;
        }
        
        return ret;
    }
    
    public bool IsWagonPassable()
    {
        return !tileType.Equals(TileType.Sea) && !tileType.Equals(TileType.ShallowSea);
    }
    
    public bool IsBoatPassable()
    {
        bool ret = tileType.Equals(TileType.Sea) || tileType.Equals(TileType.ShallowSea) || structureType.Equals(StructureType.River) || structureType.Equals(StructureType.Hamlet); //hamlet so boats can reach towns, maybe change this in the future
        
        return ret;
    }

    public bool isRiver()
    {
        return structureType.Equals(StructureType.River);
    }

    public StructureType GetStructureType()
    {
        return structureType;
    }
}

public enum TileType { //values from atlas cood cardinal
    None = -1,
    Snow = 0,
    SnowFertile = 1, //shouldnt be possible crossed out in atlas
    SnowMountain = 2,
    Temperate = 3,
    TemperateFertile = 4,
    TemperateMountain = 5,
    Subtropic = 6,
    SubtropicFertile = 7,
    SubtropicMountain = 8,
    Tropic = 9,
    TropicFertile = 10,
    TropicMountain = 11,
    
    
    Sea = 12,
    ShallowSea = 13,
}

public enum StructureType
{
    None = -1,
    River = 14,
    Hamlet = 22,
}