using System;
using EconSim.data;
using EconSim.logic;
using EconSim.logic.buildings;

namespace EconSim.tst;

public class FactoryAndBasePriceSanityCheck
{

    public static void Run() {
        testBrewery();
        testCarpentryYard();
        testJewelry();
    }

    private static void testBrewery() {
        testBuildingMakesMoneyOnPaper(new Brewery(getTestTown()));
        testBuildingEmployeesMakeMoneyOnPaper(new Brewery(getTestTown()));
        testBuildingDoesntMakeMoneyIfItCantBuyIngredients(new Brewery(getTestTown()));
        testBuildingMakesMoneyWithWages(new Brewery(getTestTown()));
    }
    
    private static void testCarpentryYard() {
        testBuildingMakesMoneyOnPaper(new CarpentryYard(getTestTown()));
        testBuildingEmployeesMakeMoneyOnPaper(new CarpentryYard(getTestTown()));
        testBuildingDoesntMakeMoneyIfItCantBuyIngredients(new CarpentryYard(getTestTown()));
        testBuildingMakesMoneyWithWages(new CarpentryYard(getTestTown()));
    }
    
    private static void testJewelry() {
        testBuildingMakesMoneyOnPaper(new Jeweler(getTestTown()));
        testBuildingEmployeesMakeMoneyOnPaper(new Jeweler(getTestTown()));
        testBuildingDoesntMakeMoneyIfItCantBuyIngredients(new Jeweler(getTestTown()));
        testBuildingMakesMoneyWithWages(new Jeweler(getTestTown()));
    }

    private static Town getTestTown() {
        TurnAndTimeManager turnManager = new TurnAndTimeManager();
        Town town = new Town("TestTown", 1, new Vector2Int(1,1), turnManager);
        town.SetMarket(new FixedPriceMarket(turnManager));
        return town;
    }    

    private static void testBuildingMakesMoneyOnPaper(Building building) {
        Util.Assert(
            !building.GET_ITEM_CONSUMED().Equals(building.GET_ITEM_PRODUCED()),
            building.GetType().Name + " cannot consume and produce the same item.");
        
        CoinAmount unitCost = CoinAmount.GetMultiplyBy(
            SimulationConstants.BasePrice[building.GET_ITEM_CONSUMED()], 
            building.GET_ITEMS_CONSUMED_PER_UNIT_PRODUCED()
        );
        
        CoinAmount unitIncome = SimulationConstants.BasePrice[building.GET_ITEM_PRODUCED()];

        double roi = (double) unitIncome.AsInt() / unitCost.AsInt();
        
        Util.Assert(roi > 1, 
            building.GetType().Name + " cannot have roi <1. (Unit Income,Unit Cost) = ( " + unitIncome + ", " + unitCost + ")");

        SimpleLogger.Debug(building.GetType().Name + " has (Unit Income,Unit Cost) = ( " + unitIncome + ", " + unitCost + ")");
        SimpleLogger.Debug(building.GetType().Name + " has on paper ReturnOnInvestment of " + roi);
    }

    private static void testBuildingEmployeesMakeMoneyOnPaper(Building building)
    {
        int totalYearlyConsumption = SimulationConstants.FoodConsumptionPerTurn * TurnAndTimeManager.TurnsInAYear;
        CoinAmount totalYearlyCost = CoinAmount.GetMultiplyBy(SimulationConstants.BasePrice[ItemType.Grain], totalYearlyConsumption);
        CoinAmount yearlyWage = building.GET_WAGE_PER_PERSONYEAR();

        Util.Assert(yearlyWage.IsGreaterThan(totalYearlyCost), "Yearly wage: " + yearlyWage + 
                                                               " must be greater than yearly food cost: " + totalYearlyCost);

    }

    private static void testBuildingDoesntMakeMoneyIfItCantBuyIngredients(Building building) {
        building.EmployWorkers(1);

        int pollLength = 100;
        for (int i = 0; i < pollLength; i++) {
            building.DoProductionTurn();
        }
        
        Util.Assert(building.GetProfit().Equals(building.GET_BUILD_COST()),
            building.GetType().Name + " makes money without buying any ingredients.");
    }
    
    private static void testBuildingMakesMoneyWithWages(Building building) {
        Town town = building.GetTown();
        ItemType itemConsumed = building.GET_ITEM_CONSUMED();
        building.EmployWorkers(1);
        int pollLength = 4;
        
        double yearlyConsumption = building.GET_PRODUCTION_PER_PERSONYEAR() * building.GET_ITEMS_CONSUMED_PER_UNIT_PRODUCED();
        double yearsInPoll = (double) pollLength / TurnAndTimeManager.TurnsInAYear;
        double totalConsumption = yearlyConsumption * yearsInPoll;

        town.GetMarket().SellItems(new CoinAmount(), itemConsumed, (int) Double.Round(Math.Ceiling(totalConsumption)));
        for (int i = 0; i < pollLength; i++) {
            building.DoProductionTurn();
        }

        CoinAmount profitPerTurn = new CoinAmount(building.GetProfit().AsInt() / pollLength);
        
        CoinAmount profitPerYear = CoinAmount.GetMultiplyBy(
            profitPerTurn, 
            TurnAndTimeManager.TurnsInAYear
        );

        CoinAmount totalWages = CoinAmount.GetMultiplyBy(
            building.GET_WAGE_PER_PERSONYEAR(),
            (double) pollLength / TurnAndTimeManager.TurnsInAYear
       );
        
        double wageFractionOfProfit = (double) totalWages.AsInt() / building.GetProfit().AsInt();
        
        SimpleLogger.Debug(building.GetType().Name + " has income per person year of " + profitPerYear);
        SimpleLogger.Debug(building.GetType().Name + " has wage fraction of profit of " + wageFractionOfProfit.ToString("P0"));
        
        Util.Assert( profitPerTurn.AsInt() > CoinAmount.MinValue.AsInt(),
            building.GetType().Name + " didnt make any money. Are wages too high?"
        );
        
        Util.Assert(wageFractionOfProfit > 0, building.GetType().Name + " did not pay any wages as fraction of profit.");        
    }
}