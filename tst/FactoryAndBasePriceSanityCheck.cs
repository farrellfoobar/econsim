using System;
using EconSim.data;
using EconSim.logic;
using EconSim.logic.buildings;

namespace EconSim.tst;

public class FactoryAndBasePriceSanityCheck
{

    public void run() {
        Town town = getTestTown();
        sanityCheckBaseFactoryIO(new Brewery(town), town);
        sanityCheckBaseFactoryIO(new CarpentryYard(town), town);
        sanityCheckBaseFactoryIO(new Jeweler(town), town);
    }
    
    private void sanityCheckBaseFactoryIO(Building building, Town town) {
        testBuildingMakesMoneyOnPaper(building);
        testBuildingDoesntMakeMoneyIfItCantBuyIngredients(building);
        testBuildingMakesMoneyWithWages(building, town, building.GET_ITEM_CONSUMED());
    }

    private Town getTestTown() {
        return new Town("TestTown", 100, new TurnAndTimeManager());
    }    

    void testBuildingMakesMoneyOnPaper(Building building) {
        Util.Assert(
            !building.GET_ITEM_CONSUMED().Equals(building.GET_ITEM_PRODUCED()),
            building.GetType() + "\t cannot consume and produce the same item.");
        
        CoinAmount unitCost = CoinAmount.getMultiplyBy(
            SimulationConstants.BASE_PRICE[building.GET_ITEM_CONSUMED()], 
            building.GET_ITEMS_CONSUMED_PER_UNIT_PRODUCED()
        );
        
        CoinAmount unitIncome = SimulationConstants.BASE_PRICE[building.GET_ITEM_PRODUCED()];

        double roi = CoinAmount.getDivideBy(unitIncome , unitCost);
        
        Util.Assert(roi > 1, 
            building.GetType() + "\t cannot have roi <1. (Unit Income,Unit Cost) = ( " + unitIncome + ", " + unitCost + ")");

        SimpleLogger.debug(building.GetType() + "\t has on paper ReturnOnInvestment of " + roi);
    }

    private void testBuildingDoesntMakeMoneyIfItCantBuyIngredients(Building building) {
        building.employWorkers(1);

        int pollLength = 100;
        for (int i = 0; i < pollLength; i++) {
            building.doProductionTurn();
        }

        Util.Assert(building.getProfit().asDouble().Equals(0d), building.GetType() + "\t makes money without buying any ingredients.");
    }
    
    private void testBuildingMakesMoneyWithWages(Building building, Town town, ItemType itemConsumed) {
        building.employWorkers(1);
        int pollLength = 100;
        
        double yearlyConsumption = building.GET_PRODUCTION_PER_PERSONYEAR() * building.GET_ITEMS_CONSUMED_PER_UNIT_PRODUCED();
        double yearsInPoll = (double) pollLength / TurnAndTimeManager.TURNS_IN_A_YEAR;
        double totalConsumption = yearlyConsumption * yearsInPoll;

        town.getMarket().sellItems(itemConsumed, (int) Double.Round(Math.Ceiling(totalConsumption)));
        for (int i = 0; i < pollLength; i++) {
            building.doProductionTurn();
        }

        CoinAmount profitPerTurn = CoinAmount.getDivideBy(
            building.getProfit(), 
            pollLength
        );

        CoinAmount totalWages = CoinAmount.getMultiplyBy(
            building.GET_WAGE_PER_PERSONYEAR(),
            (double) pollLength / TurnAndTimeManager.TURNS_IN_A_YEAR
       );
        
        double wageFractionOfProfit = CoinAmount.getDivideBy(totalWages, building.getProfit());
        
        Util.Assert( profitPerTurn.asDouble() > CoinAmount.MIN_VALUE.asDouble(),
            building.GetType() + "\t didnt make any money. Are wages too high?"
        );
        
        Util.Assert(wageFractionOfProfit > 0, building.GetType() + "\t did not make pay any wages as fraction of profit.");
        
        SimpleLogger.debug(building.GetType() + "\t has income per person turn of " + profitPerTurn);
        SimpleLogger.debug(building.GetType() + "\t has wage fraction of profit of " + wageFractionOfProfit);
    }
}