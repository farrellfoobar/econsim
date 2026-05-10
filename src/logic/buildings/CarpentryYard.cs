using System;

namespace EconSim.logic.buildings;

public class CarpentryYard(Town hostTown) : Building(hostTown)
{
    protected override int PRODUCTION_PER_PERSONYEAR { get; } = 60;
    protected override int WAGE_PER_PERSONYEAR { get; } = 60;
    protected override ItemType ITEM_PRODUCED { get; } = ItemType.FURNITURE;
}