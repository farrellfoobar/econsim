using System;
using EconSim.logic;
using EconSim.logic.buildings;

namespace EconSim.tst;

public class TestExecutor
{
    public static void Main() {
        FactoryAndBasePriceSanityCheck test = new FactoryAndBasePriceSanityCheck();
        test.Run();
        FoodCostConsumptionMakesSenseOnPaper test2 = new FoodCostConsumptionMakesSenseOnPaper();
        test2.Run();
    }
}