using System;
using EconSim.data;
using EconSim.logic;
using EconSim.logic.buildings;

namespace EconSim.tst;

public class FactoryAndBasePriceSanityCheck
{

    public void Run() {
        testBrewery();
        testCarpentryYard();
        testJewelry();
    }

    private void testBrewery() {
        testBuildingMakesMoneyOnPaper(new Brewery(getTestTown()));
        testBuildingDoesntMakeMoneyIfItCantBuyIngredients(new Brewery(getTestTown()));
        testBuildingMakesMoneyWithWages(new Brewery(getTestTown()));
    }
    
    private void testCarpentryYard() {
        testBuildingMakesMoneyOnPaper(new CarpentryYard(getTestTown()));
        testBuildingDoesntMakeMoneyIfItCantBuyIngredients(new CarpentryYard(getTestTown()));
        testBuildingMakesMoneyWithWages(new CarpentryYard(getTestTown()));
    }
    
    private void testJewelry() {
        testBuildingMakesMoneyOnPaper(new Jeweler(getTestTown()));
        testBuildingDoesntMakeMoneyIfItCantBuyIngredients(new Jeweler(getTestTown()));
        testBuildingMakesMoneyWithWages(new Jeweler(getTestTown()));
    }

    private Town getTestTown() {
        TurnAndTimeManager turnManager = new TurnAndTimeManager();
        Town town = new Town("TestTown", 1, turnManager);
        town.SetMarket(new FixedPriceMarket(turnManager));
        return town;
    }    

    void testBuildingMakesMoneyOnPaper(Building building) {
        Util.Assert(
            !building.GET_ITEM_CONSUMED().Equals(building.GET_ITEM_PRODUCED()),
            building.GetType() + "\t cannot consume and produce the same item.");
        
        CoinAmount unitCost = CoinAmount.GetMultiplyBy(
            SimulationConstants.BasePrice[building.GET_ITEM_CONSUMED()], 
            building.GET_ITEMS_CONSUMED_PER_UNIT_PRODUCED()
        );
        
        CoinAmount unitIncome = SimulationConstants.BasePrice[building.GET_ITEM_PRODUCED()];

        double roi = CoinAmount.UnsafeGetDivideBy(unitIncome , unitCost);
        
        Util.Assert(roi > 1, 
            building.GetType() + "\t cannot have roi <1. (Unit Income,Unit Cost) = ( " + unitIncome + ", " + unitCost + ")");

        SimpleLogger.Debug(building.GetType() + "\t has (Unit Income,Unit Cost) = ( " + unitIncome + ", " + unitCost + ")");
        SimpleLogger.Debug(building.GetType() + "\t has on paper ReturnOnInvestment of " + roi);
    }

    private void testBuildingDoesntMakeMoneyIfItCantBuyIngredients(Building building) {
        building.EmployWorkers(1);

        int pollLength = 100;
        for (int i = 0; i < pollLength; i++) {
            building.DoProductionTurn();
        }

        Util.Assert(building.GetProfit().AsInt().Equals(0d), building.GetType() + "\t makes money without buying any ingredients.");
    }
    
    private void testBuildingMakesMoneyWithWages(Building building) {
        Town town = building.GetTown();
        ItemType itemConsumed = building.GET_ITEM_CONSUMED();
        building.EmployWorkers(1);
        int pollLength = 4;
        
        double yearlyConsumption = building.GET_PRODUCTION_PER_PERSONYEAR() * building.GET_ITEMS_CONSUMED_PER_UNIT_PRODUCED();
        double yearsInPoll = (double) pollLength / TurnAndTimeManager.TurnsInAYear;
        double totalConsumption = yearlyConsumption * yearsInPoll;

        town.GetMarket().SellItems(itemConsumed, (int) Double.Round(Math.Ceiling(totalConsumption)));
        for (int i = 0; i < pollLength; i++) {
            building.DoProductionTurn();
        }

        CoinAmount profitPerTurn = CoinAmount.UnsafeGetDivideBy(
            building.GetProfit(), 
            pollLength
        );
        
        CoinAmount profitPerYear = CoinAmount.GetMultiplyBy(
            profitPerTurn, 
            TurnAndTimeManager.TurnsInAYear
        );

        CoinAmount totalWages = CoinAmount.GetMultiplyBy(
            building.GET_WAGE_PER_PERSONYEAR(),
            (double) pollLength / TurnAndTimeManager.TurnsInAYear
       );
        
        double wageFractionOfProfit = CoinAmount.UnsafeGetDivideBy(totalWages, building.GetProfit());
        
        SimpleLogger.Debug(building.GetType() + "\t has income per person year of " + profitPerYear);
        SimpleLogger.Debug(building.GetType() + "\t has wage fraction of profit of " + wageFractionOfProfit.ToString("P0"));
        
        Util.Assert( profitPerTurn.AsInt() > CoinAmount.MinValue.AsInt(),
            building.GetType() + "\t didnt make any money. Are wages too high?"
        );
        
        Util.Assert(wageFractionOfProfit > 0, building.GetType() + "\t did not pay any wages as fraction of profit.");        
    }
}