using System;
using System.Collections.Generic;
using System.Linq;
using EconSim.data;

namespace EconSim.logic;

public class ProfitSeekingAgent : Agent
{
    private CoinAmount minDesiredWealth = CoinAmount.Gold(1);
    private CoinAmount profitThreshold = CoinAmount.Gold(1);
    public ProfitSeekingAgent(Town town, GameMap map) : base(town, map)
    {}

    public override void DoTurn()
    {
        if (wealth.IsGreaterThan(minDesiredWealth)) {
            considerBuildingBuilding();
            //todo: figure out a better way to limit the number of merchants an agent can have, prob based on wealth
            if (merchants.Count == 0) {
                considerCreatingMerchant();
            }
        }
        
        foreach (Merchant merchant in merchants) {
            merchant.DoTurn();
        }
    }

    private void considerCreatingMerchant() {
        Optional<TradeRoute> mostProfitableCanAfford = getMostProfitableTradeRoute(wealth);
        if (mostProfitableCanAfford.IsPresent()) {
            Optional<TradeRoute> mostProfitable = getMostProfitableTradeRoute(CoinAmount.Gold(Int32.MaxValue));

            CoinAmount mostProfit = mostProfitable.Get().GetProfitFrom();
            CoinAmount profitCanAffort = mostProfitableCanAfford.Get().GetProfitFrom();
            bool worthOurTime = CoinAmount.GetMultiplyBy(profitCanAffort, 2).IsGreaterThan(mostProfit);
            
            if ( worthOurTime ) {
                merchants.Add(new Merchant(this, mostProfitableCanAfford.Get(), map,
                    mostProfitableCanAfford.Get().GetMerchantType()));
            }
        }
        else {
            SimpleLogger.Debug("Agent could not find a trade route for merchant!");
        }
    }
    
    private void considerBuildingBuilding() {
        Dictionary<Building, CoinAmount> buildingProfits = Building.GetBuildingProfitability(town);
        KeyValuePair<Building, CoinAmount> mostProfitableBuilding = buildingProfits.OrderBy(x => x.Value).ToList()[0];

        //Todo: move this logic into GetBuildingProfitability and make them more complex, maybe overriden for each building type???
        bool isProfitableEnough = mostProfitableBuilding.Value.IsGreaterThan(profitThreshold);
        bool wouldHaveWorkers = town.GetUnemployedPopulationCount() > mostProfitableBuilding.Key.GET_MAX_EMPLOYEES();
        bool canBuyRawMaterials = town.GetMarket().GetStock(mostProfitableBuilding.Key.GET_ITEM_CONSUMED()) > 100;
        
        if (isProfitableEnough && wouldHaveWorkers && canBuyRawMaterials) {
            town.BuildBuilding(wealth, mostProfitableBuilding.Key);
        }
    }

    private Optional<TradeRoute> getMostProfitableTradeRoute(CoinAmount budget)
    {
        Optional<TradeRoute> wagonTradeRoute = pathfinder.GetMostProfitableWagonTradeRoute(town, budget);
        Optional<TradeRoute> boatTradeRoute = pathfinder.GetMostProfitableWaterTradeRoute(town, budget);
        Optional<TradeRoute> mostProfitable = Optional<TradeRoute>.Empty();
        MerchantType merchantType = MerchantType.None;
            
        if (wagonTradeRoute.IsPresent() && boatTradeRoute.IsPresent()) {
            if (wagonTradeRoute.Get().GetProfitFrom().IsGreaterThan(boatTradeRoute.Get().GetProfitFrom())) {
                mostProfitable = wagonTradeRoute;
                mostProfitable.Get().SetMerchantType(MerchantType.Wagon);
            }
            else {
                mostProfitable = boatTradeRoute;
                mostProfitable.Get().SetMerchantType(MerchantType.Boat);
            }
        } else if (wagonTradeRoute.IsPresent()) {
            mostProfitable = wagonTradeRoute;
            mostProfitable.Get().SetMerchantType(MerchantType.Wagon);
        } else if (boatTradeRoute.IsPresent()) {
            mostProfitable = boatTradeRoute;
            mostProfitable.Get().SetMerchantType(MerchantType.Boat);
        }

        return mostProfitable;
    }
}