using System;
using System.Collections.Generic;
using EconSim.data;
using EconSim.logic;
using EconSim.logic.buildings;

namespace EconSim.tst;

public class FactoryAndBasePriceSanityCheck
{
    private bool debug;
    
    public void sanityCheckBaseFactoryIO(bool debug = false) {
        this.debug = debug;
        TurnAndTimeManager testTurnManager = new TurnAndTimeManager();
        Town testTown = new Town("TestTown", 100, testTurnManager);
        Brewery brewery = new Brewery(testTown);
        CarpentryYard carpentyYard = new CarpentryYard(testTown);
        Jeweler jeweler = new Jeweler(testTown);

        testBuildingMakesMoneyOnPaper(brewery);
        testBuildingMakesMoneyWithWages(brewery);
        
        testBuildingMakesMoneyOnPaper(carpentyYard);
        testBuildingMakesMoneyWithWages(carpentyYard);
        
        testBuildingMakesMoneyOnPaper(jeweler);
        testBuildingMakesMoneyWithWages(jeweler);
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

        if (debug) {
            Console.WriteLine(building.GetType() + "\t has ReturnOnInvestment of " + roi);
        }
    }
    
    private void testBuildingMakesMoneyWithWages(Building building) {
        Util.Assert( building.getProfit().asDouble().Equals(0), building.GetType() + "\t\t\t has profit before doing anything.");
        
        building.employWorkers(1);

        int pollLength = 100;
        for (int i = 0; i < pollLength; i++) {
            building.doProductionTurn();
        }

        CoinAmount profitPerTurn = CoinAmount.getDivideBy(building.getProfit(), pollLength);
        


        if (debug) {
            Console.WriteLine(building.GetType() + "\t has income per person turn of " + profitPerTurn);
        }
        
        CoinAmount wagesPaid = CoinAmount.getMultiplyBy(building.GET_WAGE_PER_PERSONYEAR(), pollLength);

        CoinAmount profitWithoutWages = CoinAmount.getAdd(wagesPaid, profitPerTurn);
        CoinAmount profitWithoutWagesPerTurn = CoinAmount.getDivideBy(profitWithoutWages, TurnAndTimeManager.TURNS_IN_A_YEAR);
        CoinAmount profitWithoutWagesPerYear = profitWithoutWagesPerTurn;
        
        double wageFractionOfProfit = CoinAmount.getDivideBy(building.GET_WAGE_PER_PERSONYEAR(), profitPerTurn);
        
        if (debug) {
            Console.WriteLine(building.GetType() + "\t has income per person turn of " + profitWithoutWagesPerYear + " without wages");
            Console.WriteLine(building.GetType() + "\t has wage fraction of profit of " + wageFractionOfProfit);
        }
        
        Util.Assert( profitPerTurn.asDouble() > CoinAmount.MIN_VALUE.asDouble(),
            building.GetType() + "\t didnt make any money. Are wages too high?"
        );
    }
}