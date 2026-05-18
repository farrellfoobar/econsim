using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

public class Agent
{
    private CoinAmount wealth = SimulationConstants.AgentStartingWealth;
    private List<Merchant> merchants = new List<Merchant>();
    
    private AStarPathfinder pathfinder;
    private Town town;
    private GameMap map;

    public Agent(Town town, GameMap map)
    {
        this.town = town;
        this.map = map;
        pathfinder = new AStarPathfinder(map);
    }
    
    public void DoTurn()
    {
        //todo: figure out a better way to limit the number of merchants an agent can have, prob based on wealth
        if (merchants.Count == 0) {
            TradeRoute tradeRoute = pathfinder.GetMostProfitableTradeRoute(town, wealth);
            merchants.Add(new Merchant(this, tradeRoute, map));
        }

        foreach (Merchant merchant in merchants) {
            merchant.DoTurn();
        }
    }
    
    public List<Merchant> GetMerchants()
    {
        return merchants;
    }

    public CoinAmount GetWealth()
    {
        return wealth;
    }

    public Vector2Int GetPosition()
    {
        return town.GetPosition();
    }

    public void RemoveMerchant(Merchant merchant)
    {
        merchants.Remove(merchant);
    }
}