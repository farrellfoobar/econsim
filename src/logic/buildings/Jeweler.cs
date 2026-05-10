using EconSim.data;

namespace EconSim.logic.buildings;

public class Jeweler(Town hostTown) : Building(hostTown)
{
    protected override int PRODUCTION_PER_PERSONYEAR { get; } = 24;
    protected override ItemType ITEM_CONSUMED { get; } = ItemType.SILVER_ORE;
    protected override int ITEMS_CONSUMED_PER_UNIT_PRODUCED { get; } = 2;
    protected override CoinAmount WAGE_PER_PERSONYEAR { get; } = new CoinAmount(120);
    protected override ItemType ITEM_PRODUCED { get; } = ItemType.JEWELRY;
}