namespace EconSim.logic.buildings;

public class Jeweler(Town hostTown) : Building(hostTown)
{
    protected override int PRODUCTION_PER_PERSONYEAR { get; } = 24;
    protected override ItemType ITEM_CONSUMED { get; } = ItemType.SILVER_ORE;
    protected override int ITEMS_CONSUMED_PER_UNIT_PRODUCED { get; } = 2;
    protected override int WAGE_PER_PERSONYEAR { get; } = 120;
    protected override ItemType ITEM_PRODUCED { get; } = ItemType.JEWELRY;
}