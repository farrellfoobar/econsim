using EconSim.data;

namespace EconSim.logic.buildings;

public class Brewery(Town hostTown) : Building(hostTown)
{
    protected override double PRODUCTION_PER_PERSONYEAR { get; } = 1000;
    protected override ItemType ITEM_CONSUMED { get; } = ItemType.GRAIN;
    protected override double ITEMS_CONSUMED_PER_UNIT_PRODUCED { get; } = 1;
    protected override CoinAmount WAGE_PER_PERSONYEAR { get; } = new CoinAmount(60);
    protected override ItemType ITEM_PRODUCED { get; } = ItemType.BEER;
}