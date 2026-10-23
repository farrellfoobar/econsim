using System;
using EconSim.data;

namespace EconSim.logic;

public class RiverTile : GameTile
{
    private Direction direction;
    public RiverTile(TileType tileType, Vector2Int position, Direction direction, StructureType structureType = StructureType.River) : base(tileType, position, structureType)
    {
        this.direction = direction;
    }

    public Direction GetDirection()
    {
        return direction;
    }
}

public enum Direction
{
    None,
    North,
    NorthEast,
    SouthEast,
    South,
    SouthWest,
    NorthWest,
}