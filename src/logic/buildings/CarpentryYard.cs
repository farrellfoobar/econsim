using System;
using EconSim.data;

namespace EconSim.logic.buildings;

public class CarpentryYard(Town hostTown) : Building(hostTown)
{
    protected override int PRODUCTION_PER_PERSONYEAR { get; } = SimulationConstants.CarpentryYardValues.PRODUCTION_PER_PERSONYEAR;
    protected override ItemType ITEM_CONSUMED { get; } = SimulationConstants.CarpentryYardValues.ITEM_CONSUMED;
    protected override int ITEMS_CONSUMED_PER_UNIT_PRODUCED { get; } = SimulationConstants.CarpentryYardValues.CONSUMPTION_PER_UNIT_PRODUCED;
    protected override CoinAmount WAGE_PER_PERSONYEAR { get; } = SimulationConstants.CarpentryYardValues.WAGE_PER_PERSONYEAR;
    protected override ItemType ITEM_PRODUCED { get; } = SimulationConstants.CarpentryYardValues.ITEM_PRODUCED;
}