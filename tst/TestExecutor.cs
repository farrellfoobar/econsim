using System;
using EconSim.logic;
using EconSim.logic.buildings;

namespace EconSim.tst;

public class TestExecutor
{
    public static void Main() {
        TestCanInstaniateGodotTypesInTests.Run();
        
        LaborersBuyAppropreatly test1 = new LaborersBuyAppropreatly();
        test1.run();
        
        FactoryAndBasePriceSanityCheck test2 = new FactoryAndBasePriceSanityCheck();
        test2.Run();
    }
}