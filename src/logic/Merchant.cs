using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;


public class Merchant
{
    private Goal goal = Goal.NONE;
    private Vector2Int position;
    private Vector2Int destination;
    private AStarPathfinder pathfinder;
    private Stack<Vector2Int> path;

    public Merchant(Vector2Int position, GameMap map)
    {
        this.position = position;
        this.pathfinder = new AStarPathfinder(map);
    }
    
    public void DoTurn()
    {
        switch (goal)
        {
            case Goal.GOTO:
                if (position.Equals(destination))
                    goal = Goal.NONE; //todo
                else
                    position = path.Pop();
                break;
            case Goal.PLAN_ROUTE:
                path = pathfinder.findPath(position, destination);
                goal = Goal.GOTO;
                break;
            default:
                break;
        }
    }

    public void setOnJourneyTo(Vector2Int destination)
    {
        this.destination = destination;
        this.goal = Goal.PLAN_ROUTE;
    }

    public Vector2Int getPosition()
    {
        return position;
    }
}

public enum Goal{
    NONE,
    PLAN_ROUTE,
    GOTO,
}