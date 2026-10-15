namespace EconSim.tst;

public class TestExecutor
{
    public static void Main()
    {
        TurnsPerSecondSanityTest.Run();
        PathfinderTests.Run();
        TestCanInstaniateGodotTypesInTests.Run();
        LaborersBuyAppropreatly.Run();
        FactoryAndBasePriceSanityCheck.Run();
    }
}