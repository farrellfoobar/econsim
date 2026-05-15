using System;
using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;


public class Merchant
{
    private Goal goal = Goal.None;
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
            case Goal.Goto:
                if (position.Equals(destination))
                    goal = Goal.None;
                else {
                    position = path.Pop();
                }
                break;
            case Goal.PlanRoute:
                path = pathfinder.FindPath(position, destination);
                path.Pop();
                goal = Goal.Goto;
                break;
            default:
                break;
        }
    }

    public void setOnJourneyTo(Vector2Int destination)
    {
        this.destination = destination;
        this.goal = Goal.PlanRoute;
    }

    public Vector2Int GetPosition()
    {
        return position;
    }
}

public enum Goal{
    None,
    PlanRoute,
    Goto,
}