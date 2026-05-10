namespace EconSim.logic.buildings;

public class Brewery(Town hostTown) : Building(hostTown)
{
    protected override int PRODUCTION_PER_PERSONYEAR { get; } = 1000;
    protected override int WAGE_PER_PERSONYEAR { get; } = 60;
    protected override ItemType ITEM_PRODUCED { get; } = ItemType.BEER;
}