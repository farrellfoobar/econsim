using System;
using EconSim.data;

namespace EconSim.logic;

public class RiverTile : GameTile
{
    private Direction direction;
    public RiverTile(Vector2Int position, Direction direction, StructureType structure = StructureType.None) : base(TileType.River, position, structure)
    {
        if(structure != StructureType.None)
            throw new ArgumentException("River tile cannot have a structure on it.");
        
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