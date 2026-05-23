using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

public abstract class Agent
{
    protected CoinAmount wealth = SimulationConstants.AgentStartingWealth;
    protected List<Merchant> merchants = new List<Merchant>();
    
    protected AStarPathfinder pathfinder;
    protected Town town;
    protected GameMap map;

    public Agent(Town town, GameMap map)
    {
        this.town = town;
        this.map = map;
        pathfinder = new AStarPathfinder(map);
    }

    public abstract void DoTurn();
    
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