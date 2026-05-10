using System;

namespace EconSim.logic.buildings;

public class CarpentryYard(Town hostTown) : Building(hostTown)
{
    protected override int PRODUCTION_PER_PERSONYEAR { get; } = 60;
    protected override ItemType ITEM_CONSUMED { get; } = ItemType.WOOD;
    protected override int ITEMS_CONSUMED_PER_UNIT_PRODUCED { get; } = 2;
    protected override int WAGE_PER_PERSONYEAR { get; } = 60;
    protected override ItemType ITEM_PRODUCED { get; } = ItemType.FURNITURE;
}