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
    private GameMap map;
    
    public AStarPathfinder(GameMap map)
    {
        this.map = map;
        pathfinder = new AStar2D();

        foreach (GameTile tile in map.GetTiles()) {
            if (tile.IsPassable()) {
                pathfinder.AddPoint(positionToId(tile), tile.GetPosition().AsGodotVector(), tile.GetPathfindingWeight());
            }
        }
        
        foreach (GameTile tile in map.GetTiles()) {
            foreach (GameTile neighborTile in map.GetNeighborTiles(tile)) {
                if (neighborTile.IsPassable()) {
                    pathfinder.ConnectPoints(positionToId(tile), positionToId(neighborTile));
                }
            }
        }
    }
    
    public TradeRoute GetMostProfitableTradeRoute(Town fromTown, CoinAmount maxCost)
    {
        List<Town> otherTowns = map.GetTowns();
        otherTowns.Remove(fromTown);
        
        CoinAmount bestProfit = new CoinAmount(-1);
        TradeRoute bestRoute = null;
        foreach (Town toTown in otherTowns) {
            //TODO: ideally pathfinder.FindPath() would return a path & a cost, but since Godot didnt implement that Ill just path.Count for now
            Stack<Vector2Int> path = FindPath(map.GetTownPosition(fromTown), map.GetTownPosition(toTown));
            TradeRoute route = getBestRouteForTown(fromTown, toTown, path, maxCost);
            CoinAmount routeProfit = route.GetProfitFrom();

            if (routeProfit.IsGreaterThan(bestProfit)) {
                bestProfit = routeProfit;
                bestRoute = route;
            }
        }

        return bestRoute;
    }

    private TradeRoute getBestRouteForTown(Town fromTown, Town toTown, Stack<Vector2Int> path, CoinAmount maxCost)
    {
        CoinAmount bestExportProfit = new CoinAmount(-1);
        CoinAmount bestImportProfit = new CoinAmount(-1);
        OneWayTradeRoute bestExportRoute = null;
        OneWayTradeRoute bestImportRoute = null;
        
        foreach (ItemType item in Items.ALL_ITEMS) {
            OneWayTradeRoute exportRoute = new OneWayTradeRoute(fromTown, toTown, item, path, maxCost);
            //Im not really sure why I dont have to .Reverse the path for import route. Probably because of how Stack is enumerated, which is frustrating
            OneWayTradeRoute importRoute = new OneWayTradeRoute(toTown, fromTown, item, new Stack<Vector2Int>(path), maxCost);
            CoinAmount exportProfit = exportRoute.GetProfitFrom();
            CoinAmount importProfit = importRoute.GetProfitFrom();
            
            if (exportProfit.IsGreaterThan(bestExportProfit)) {
                bestExportProfit = exportProfit;
                bestExportRoute = exportRoute;
            } else if (importProfit.IsGreaterThan(bestImportProfit)) {
                bestImportProfit = importProfit;
                bestImportRoute = importRoute;
            }
        }
        
        return new TradeRoute(bestExportRoute, bestImportRoute);
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

    private Stack<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        Vector2[] godotPath = pathfinder.GetPointPath(positionToId(start), positionToId(goal));
        
        Stack<Vector2Int> path = new Stack<Vector2Int>(godotPath.Length);
        for(int i = godotPath.Length-1; i >= 0 ; i--){
            path.Push(new Vector2Int(godotPath[i]));
        }
        
        return path;
    }
}