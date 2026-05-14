using EconSim.data;
using EconSim.logic;

namespace EconSim.tst;

public class FoodCostConsumptionMakesSenseOnPaper
{
    public void Run()
    {
        int totalYearlyConsumption = SimulationConstants.FoodConsumptionPerTurn * TurnAndTimeManager.TurnsInAYear;
        CoinAmount totalYearlyCost = CoinAmount.GetMultiplyBy(SimulationConstants.BasePrice[ItemType.Grain], totalYearlyConsumption);
        CoinAmount yearlyWage = SimulationConstants.BreweryValues.WagePerPersonyear;

        Util.Assert(yearlyWage.IsGreaterThan(totalYearlyCost), "Yearly wage: " + yearlyWage + 
                                                               " must be greater than yearly food cost: " + totalYearlyCost);
    }
}