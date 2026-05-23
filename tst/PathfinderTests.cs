using System;
using System.Collections.Generic;
using EconSim.data;
using EconSim.logic;

namespace EconSim.tst;

public class PathfinderTests
{
    public static void Run()
    {
        GameMap gameMap = new GameMap(27, 13);
        TurnAndTimeManager turnAndTimeManager = new TurnAndTimeManager();
        
        Town from = new Town("fromTown", 0, new Vector2Int(3, 3), turnAndTimeManager);
        Town to = new Town("toTown", 0, new Vector2Int(13, 7), turnAndTimeManager);
        Town redHering = new Town("RedHeringTown", 0, new Vector2Int(20, 9), turnAndTimeManager);

        FixedPriceMarket fromMarket = new FixedPriceMarket(turnAndTimeManager);
        from.SetMarket(fromMarket);
        to.SetMarket(new FixedPriceMarket(turnAndTimeManager));
        FixedPriceMarket toMarket = new FixedPriceMarket(turnAndTimeManager);
        from.SetMarket(fromMarket);
        redHering.SetMarket(new FixedPriceMarket(turnAndTimeManager));
        FixedPriceMarket redHeringMarket = new FixedPriceMarket(turnAndTimeManager);
        from.SetMarket(fromMarket);
        
        gameMap.AddTown(from);
        gameMap.AddTown(to);
        gameMap.AddTown(redHering);
        
        from.GetInventory().AddItems(ItemType.Grain, 1000);
        to.GetInventory().AddItems(ItemType.Grain, 1000);
        redHering.GetInventory().AddItems(ItemType.Grain, 1000);
        
        from.GetInventory().AddItems(ItemType.Jewelry, 1000);
        to.GetInventory().AddItems(ItemType.Fish, 1000);
        
        fromMarket.SetPrice(ItemType.Jewelry, CoinAmount.GetMultiplyBy(SimulationConstants.BasePrice[ItemType.Jewelry], 0.5));
        toMarket.SetPrice(ItemType.Jewelry, CoinAmount.GetMultiplyBy(SimulationConstants.BasePrice[ItemType.Jewelry], 2));
        
        toMarket.SetPrice(ItemType.Fish, CoinAmount.GetMultiplyBy(SimulationConstants.BasePrice[ItemType.Fish], 0.5));
        fromMarket.SetPrice(ItemType.Fish, CoinAmount.GetMultiplyBy(SimulationConstants.BasePrice[ItemType.Fish], 2));
        
        redHeringMarket.SetPrice(ItemType.Grain, CoinAmount.GetMultiplyBy(SimulationConstants.BasePrice[ItemType.Grain], 2));

        AStarPathfinder pathfinder = new AStarPathfinder(gameMap);

        CoinAmount maxCost = CoinAmount.Gold(1000);
        Optional<TradeRoute> ret = pathfinder.GetMostProfitableWagonTradeRoute(from, maxCost);
        
        Stack<Vector2Int> expectedPath = new Stack<Vector2Int>(new [] {
            new Vector2Int(13,7),
            new Vector2Int(12,7),
            new Vector2Int(11,6),
            new Vector2Int(10,6),
            new Vector2Int(9,5),
            new Vector2Int(8,5),
            new Vector2Int(7,4),
            new Vector2Int(6,4),
            new Vector2Int(5,3),
            new Vector2Int(4,3),
            new Vector2Int(3,3),
        });

        int expectedPathCost = expectedPath.Count;
        OneWayTradeRoute expectedExport = new OneWayTradeRoute(from, to, ItemType.Jewelry, expectedPath, expectedPathCost, maxCost);
        OneWayTradeRoute expectedImport = new OneWayTradeRoute(to, from, ItemType.Fish, new Stack<Vector2Int>(expectedPath), expectedPathCost, maxCost);

        TradeRoute expectedRoute = new TradeRoute(expectedExport, expectedImport);
        
        Console.WriteLine(expectedRoute);
        Console.WriteLine(ret);
        
        Util.Assert(ret.IsPresent(), "pathfinder.GetMostProfitableTradeRoute could not find a trade route.");
        
        Util.Assert(ret.Get().Equals(expectedRoute),
            "pathfinder.GetMostProfitableTradeRoute did not find expected route.");
    }
}