namespace EconSim.tst;

public class TestExecutor
{
    public static void Main()
    {
        PathfinderTests.Run();
        TestCanInstaniateGodotTypesInTests.Run();
        LaborersBuyAppropreatly.Run();
        FactoryAndBasePriceSanityCheck.Run();
    }
}