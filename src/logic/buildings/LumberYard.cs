using System;

namespace EconSim.logic.buildings;

public class LumberYard : Building
{
    public LumberYard(Town hostTown) : base(hostTown) {}
    protected override int PRODUCTION_PER_PERSONYEAR { get; } = 100;
    protected override int WAGE_PER_PERSONYEAR { get; } = 60;
    protected override ItemType ITEM_PRODUCED { get; } = ItemType.LUMBER;
}