using EconSim.data;

namespace EconSim.logic;

public class ProfitSeekingAgent : Agent
{
    public ProfitSeekingAgent(Town town, GameMap map) : base(town, map)
    {}

    public override void DoTurn()
    {
        //todo: figure out a better way to limit the number of merchants an agent can have, prob based on wealth
        if (merchants.Count == 0) {
            Optional<TradeRoute> mostProfitable = getMostProfitableTradeRoute();

            if (mostProfitable.IsPresent()) {
                merchants.Add(new Merchant(this, mostProfitable.Get(), map, mostProfitable.Get().GetMerchantType()));
            }
            else {
                SimpleLogger.Debug("Agent could not find a trade route for merchant!");
            }
            
        }

        foreach (Merchant merchant in merchants) {
            merchant.DoTurn();
        }
    }

    private Optional<TradeRoute> getMostProfitableTradeRoute()
    {
        Optional<TradeRoute> wagonTradeRoute = pathfinder.GetMostProfitableWagonTradeRoute(town, wealth);
        Optional<TradeRoute> boatTradeRoute = pathfinder.GetMostProfitableWaterTradeRoute(town, wealth);
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