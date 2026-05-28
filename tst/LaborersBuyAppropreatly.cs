using EconSim.data;
using EconSim.logic;

namespace EconSim.tst;

public class LaborersBuyAppropreatly
{
    
    public static void Run()
    {
        Laborer laborer = new Laborer();
        TurnAndTimeManager turnAndTimeManager = new TurnAndTimeManager();
        Market market = new Market(turnAndTimeManager);
        market.GetInventory().AddItems(ItemType.Grain, 1000);
        Town testTown = new Town("", 1, new Vector2Int(0, 0), turnAndTimeManager);
        
        CoinAmount wealthBefore = new CoinAmount(550);
        laborer.TestingSetWealth(new CoinAmount(wealthBefore));
        laborer.DoTurn(testTown);
        
        CoinAmount minWealthAfter = new CoinAmount(wealthBefore);
        CoinAmount expectedSpend = CoinAmount.GetMultiplyBy(
            SimulationConstants.BasePrice[ItemType.Grain], 
            SimulationConstants.FoodConsumptionPerTurn);
        minWealthAfter.Subtract(expectedSpend);
        minWealthAfter.Subtract(CoinAmount.Copper(1));
        
        CoinAmount actualSpend = wealthBefore;
        actualSpend.Subtract(laborer.GetWealth());
        
        Util.Assert(laborer.GetWealth().IsGreaterThan(minWealthAfter), 
            "Expected to spend: " + expectedSpend + "but actually spent: " + actualSpend);
    }
    
}