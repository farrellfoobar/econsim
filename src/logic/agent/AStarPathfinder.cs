using System;
using System.Collections.Generic;
using EconSim.data;
using Godot;

namespace EconSim.logic;

public class AStarPathfinder
{
    private AStar2D wagonPathfinder = new AStar2D();
    private BoatPathfinder boatPathfinder;
    private GameMap map;
    
    public AStarPathfinder(GameMap map)
    {
        this.map = map;
        boatPathfinder = new BoatPathfinder(map);

        foreach (GameTile tile in map.GetTiles()) {
            if (tile.IsWagonPassable()) {
                wagonPathfinder.AddPoint(positionToId(tile), tile.GetPosition().AsGodotVector(), tile.GetWagonPathfindingWeight());
            }

            if (tile.IsBoatPassable()) {
                boatPathfinder.AddPoint(positionToId(tile), tile.GetPosition().AsGodotVector(), tile.GetBoatPathfindingWeight());
            }
        }
        
        foreach (GameTile tile in map.GetTiles()) {
            foreach (GameTile neighborTile in map.GetNeighborTiles(tile).Values) {
                if (tile.IsWagonPassable() && neighborTile.IsWagonPassable()) {
                    wagonPathfinder.ConnectPoints(positionToId(tile), positionToId(neighborTile));
                }
                if (tile.IsBoatPassable() && neighborTile.IsBoatPassable()) {
                    boatPathfinder.ConnectPoints(positionToId(tile), positionToId(neighborTile));
                }
            }
        }
    }
    
    public Optional<TradeRoute> GetMostProfitableWagonTradeRoute(Town fromTown, CoinAmount maxCost)
    {
        return getMostProfitableTradeRoute(fromTown, maxCost, wagonPathfinder);
    }
    
    public Optional<TradeRoute> GetMostProfitableWaterTradeRoute(Town fromTown, CoinAmount maxCost)
    {
        return getMostProfitableTradeRoute(fromTown, maxCost, boatPathfinder);
    }

    private Optional<TradeRoute> getMostProfitableTradeRoute(Town fromTown, CoinAmount maxCost, AStar2D pathfinder)
    {
        List<Town> otherTowns = map.GetTowns();
        otherTowns.Remove(fromTown);
        
        CoinAmount bestProfit = new CoinAmount(-1);
        Optional<TradeRoute> bestRoute = Optional<TradeRoute>.Empty();
        foreach (Town toTown in otherTowns) {
            //TODO: ideally pathfinder.FindPath() would return a path & a cost, but since Godot didnt implement that Ill just path.Count for now
            Tuple<Stack<Vector2Int>, int> pathAndCost = findPath(map.GetTownPosition(fromTown), map.GetTownPosition(toTown), pathfinder);
            Stack<Vector2Int> path = pathAndCost.Item1;
            if (path.Count > 0) {
                TradeRoute route = getBestRouteForTown(fromTown, toTown, path, pathAndCost.Item2, maxCost);
                CoinAmount routeProfit = route.GetProfitFrom();

                if (routeProfit.IsGreaterThan(bestProfit)) {
                    bestProfit = routeProfit;
                    bestRoute = new Optional<TradeRoute>(route);
                }
            }
        }

        return bestRoute;
    }

    private TradeRoute getBestRouteForTown(Town fromTown, Town toTown, Stack<Vector2Int> path, int pathCost, CoinAmount maxCost)
    {
        CoinAmount bestExportProfit = new CoinAmount(-1);
        CoinAmount bestImportProfit = new CoinAmount(-1);
        OneWayTradeRoute bestExportRoute = null;
        OneWayTradeRoute bestImportRoute = null;
        
        foreach (ItemType item in Items.ALL_ITEMS) {
            OneWayTradeRoute exportRoute = new OneWayTradeRoute(fromTown, toTown, item, path, pathCost, maxCost);
            //Im not really sure why I dont have to .Reverse the path for import route. Probably because of how Stack is enumerated, which is frustrating
            OneWayTradeRoute importRoute = new OneWayTradeRoute(toTown, fromTown, item, new Stack<Vector2Int>(path), pathCost, maxCost);
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

    private Tuple<Stack<Vector2Int>, int> findPath(Vector2Int start, Vector2Int goal, AStar2D pathfinder)
    {
        //todo: handle empty stack i.e. no path
        Vector2[] godotPath = pathfinder.GetPointPath(positionToId(start), positionToId(goal));
        
        Stack<Vector2Int> path = new Stack<Vector2Int>(godotPath.Length);
        int pathCost = 0;
        for(int i = godotPath.Length-1; i >= 0 ; i--) {
            Vector2Int pos = new Vector2Int(godotPath[i]);
            path.Push(pos);
            pathCost += map.GetTilePathfindingWeight(pos);
        }
        
        return new Tuple<Stack<Vector2Int>, int>(path, pathCost);
    }
}