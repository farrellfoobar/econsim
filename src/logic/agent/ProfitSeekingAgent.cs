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
            Optional<TradeRoute> wagonTradeRoute = pathfinder.GetMostProfitableWagonTradeRoute(town, wealth);
            Optional<TradeRoute> boatTradeRoute = pathfinder.GetMostProfitableWaterTradeRoute(town, wealth);
            Optional<TradeRoute> mostProfitable = Optional<TradeRoute>.Empty();
            MerchantType merchantType = MerchantType.None;
            
            if (wagonTradeRoute.IsPresent() && boatTradeRoute.IsPresent()) {
                if (wagonTradeRoute.Get().GetProfitFrom().IsGreaterThan(boatTradeRoute.Get().GetProfitFrom())) {
                    mostProfitable = wagonTradeRoute;
                    merchantType = MerchantType.Wagon;
                }
                else {
                    mostProfitable = boatTradeRoute;
                    merchantType = MerchantType.Boat;
                }
            } else if (wagonTradeRoute.IsPresent()) {
                mostProfitable = wagonTradeRoute;
                merchantType = MerchantType.Wagon;
            } else if (boatTradeRoute.IsPresent()) {
                mostProfitable = boatTradeRoute;
                merchantType = MerchantType.Boat;
            }
            else {
                SimpleLogger.Debug("Agent could not find a trade route for merchant!");
            }
            
            merchants.Add(new Merchant(this, mostProfitable.Get(), map, merchantType));
        }

        foreach (Merchant merchant in merchants) {
            merchant.DoTurn();
        }
    }
}