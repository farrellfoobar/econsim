using System;
using System.Collections.Generic;
using System.Linq;
using EconSim.data;
using Godot;

namespace EconSim.logic;

public class AStarPathfinder
{
    //TODO: replace this with AStar2D and manually connect adjacent tiles for correct hex neighbor logic
    // see https://github.com/godotengine/godot-demo-projects/blob/3.5-9e68af3/2d/navigation_astar/pathfind_astar.gd
    private AStar2D pathfinder;
    
    public AStarPathfinder(GameMap map)
    {
        pathfinder = new AStar2D();

        foreach (GameTile tile in map.GetTiles()) {
            if (tile.IsPassable()) {
                pathfinder.AddPoint(positionToId(tile), tile.GetPosition().AsGodotVector(), tile.GetPathfindingWeight());
            }
        }
        
        foreach (GameTile tile in map.GetTiles()) {
            foreach (GameTile neighborTile in map.GetNeighborTiles(tile)) {

                if (positionToId(tile.GetPosition()).Equals(positionToId(neighborTile.GetPosition()))) {
                    Console.WriteLine("BREAKPOINT");
                }

                if (neighborTile.IsPassable()) {
                    pathfinder.ConnectPoints(positionToId(tile), positionToId(neighborTile));
                }
            }
        }
    }

    private int positionToId(GameTile tile)
    {
        return positionToId(tile.GetPosition());
    }
    
    private int positionToId(Vector2Int tile)
    {
        int x = tile.GetX();
        int y = tile.GetY();
        return (y + x) * (y + x + 1) / 2 + y;
        //https://en.wikipedia.org/wiki/Pairing_function#Cantor_pairing_function
    }

    public Stack<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        Vector2[] godotPath = pathfinder.GetPointPath(positionToId(start), positionToId(goal));
        
        Stack<Vector2Int> path = new Stack<Vector2Int>(godotPath.Length);
        for(int i = godotPath.Length-1; i >= 0 ; i--){
            path.Push(new Vector2Int(godotPath[i]));
        }
        
        return path;
    }
}