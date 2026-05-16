using EconSim.data;
using EconSim.logic;

namespace EconSim.tst;

public class LaborersBuyAppropreatly
{
    
    public static void Run()
    {
        Laborer laborer = new Laborer();
        Market market = new Market(new TurnAndTimeManager());
        market.GetInventory().AddItems(ItemType.Grain, 1000);
        
        CoinAmount wealthBefore = new CoinAmount(550);
        laborer.TestingSetWealth(new CoinAmount(wealthBefore));
        laborer.ConsumeAtMarket(market);
        
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