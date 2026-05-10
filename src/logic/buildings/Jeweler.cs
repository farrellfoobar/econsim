namespace EconSim.logic.buildings;

public class Jeweler(Town hostTown) : Building(hostTown)
{
    protected override int PRODUCTION_PER_PERSONYEAR { get; } = 25;
    protected override int WAGE_PER_PERSONYEAR { get; } = 120;
    protected override ItemType ITEM_PRODUCED { get; } = ItemType.JEWELRY;
}