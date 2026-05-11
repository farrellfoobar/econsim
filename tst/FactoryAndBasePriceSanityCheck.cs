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

        testBuilding(brewery);
        testBuilding(carpentyYard);
        testBuilding(jeweler);
    }
        

    void testBuilding(Building building) {
        Util.Assert(
            !building.GET_ITEM_CONSUMED().Equals(building.GET_ITEM_PRODUCED()),
            building.GetType() + " cannot consume and produce the same item.");
        
        CoinAmount unitCost = Items.BASE_PRICE[building.GET_ITEM_CONSUMED()].multiply(building.GET_ITEMS_CONSUMED_PER_UNIT_PRODUCED());
        CoinAmount unitIncome = Items.BASE_PRICE[building.GET_ITEM_PRODUCED()];

        double roi = CoinAmount.getDivideBy(unitIncome , unitCost);
        
        Util.Assert(roi > 1, 
            building.GetType() + " cannot have roi <1. (Unit Income,Unit Cost) = ( " + unitIncome + ", " + unitCost + ")");

        if (debug) {
            Console.WriteLine(building.GetType() + " has ReturnOnInvestment of " + roi);
        }
    }
}