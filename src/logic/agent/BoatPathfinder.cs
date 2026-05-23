using System;
using EconSim.data;
using Godot;

namespace EconSim.logic;

public partial class BoatPathfinder : AStar2D
{
    public const float BOAT_PATHFINDING_WEIGHT_WITH_CURRENT = 0.1f;
    public const float BOAT_PATHFINDING_WEIGHT_AGAINST_CURRENT = 2f;
    public const float OCEAN_PATHFINDING_WEIGHT = 0.5f;
    private GameMap map;

    public BoatPathfinder(GameMap map)
    {
        this.map = map;
    }
    
    public void SetMap(GameMap map)
    {
        this.map = map;
    }
    
    public override float _ComputeCost(long fromId, long toId)
    {
        float ret = 0;
        GameTile fromTile = map.GetTileAt(new Vector2Int(GetPointPosition(fromId)));
        GameTile toTile = map.GetTileAt(new Vector2Int(GetPointPosition(toId)));
        
        if (fromTile.GetTileType().Equals(TileType.Ocean)) {
            ret = OCEAN_PATHFINDING_WEIGHT;
        } else if (fromTile.GetTileType().Equals(TileType.River)) {
            RiverTile riverTile = fromTile as RiverTile;
            bool withCurrent = map.GetNeighborTiles(fromTile)[riverTile.GetDirection()].Equals(toTile);
            ret = withCurrent ? BOAT_PATHFINDING_WEIGHT_WITH_CURRENT : BOAT_PATHFINDING_WEIGHT_AGAINST_CURRENT;
        } 
        
        return ret;
    }

    public override float _EstimateCost(long fromId, long endId)
    {
        return _ComputeCost(fromId, endId);
    }
}