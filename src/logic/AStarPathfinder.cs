using System.Collections.Generic;
using System.Linq;
using EconSim.data;
using Godot;

namespace EconSim.logic;

public class AStarPathfinder
{
    //TODO: replace this with AStar2D and manually connect adjacent tiles for hex logic
    private AStarGrid2D pathfinder;
    
    public AStarPathfinder(GameMap map)
    {
        pathfinder = new AStarGrid2D();
        pathfinder.SetRegion(new Rect2I(0, 0, map.getWidth(), map.getHeight()));
        //cellsize
        //offset
        //default_compute_heuristic 
        //default_estimate_heuristic 
        pathfinder.SetDiagonalMode(AStarGrid2D.DiagonalModeEnum.Always);
        pathfinder.Update();
    }

    public Stack<Vector2Int> findPath(Vector2Int start, Vector2Int goal)
    {
        Vector2[] godotPath = pathfinder.GetPointPath(start.asGodotVector(), goal.asGodotVector());
        
        //i hate this. maybe just use some godot type. but also godot's a* does use floats so we have to convert somewhere
        //foreach also doesnt work because it returns an enumerable which I would have to cast
        Stack<Vector2Int> path = new Stack<Vector2Int>(godotPath.Length);
        for(int i = godotPath.Length-1; i >= 0 ; i--){
            path.Push(new Vector2Int(godotPath[i]));
        }
        
        return path;
    }
}