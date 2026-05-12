using EconSim.data;

namespace EconSim.logic.buildings;

public class Jeweler(Town hostTown) : Building(hostTown)
{
    protected override int PRODUCTION_PER_PERSONYEAR { get; } = SimulationConstants.JewelryValues.PRODUCTION_PER_PERSONYEAR;
    protected override ItemType ITEM_CONSUMED { get; } = SimulationConstants.JewelryValues.ITEM_CONSUMED;
    protected override int ITEMS_CONSUMED_PER_UNIT_PRODUCED { get; } = SimulationConstants.JewelryValues.CONSUMPTION_PER_UNIT_PRODUCED;
    protected override CoinAmount WAGE_PER_PERSONYEAR { get; } = SimulationConstants.JewelryValues.WAGE_PER_PERSONYEAR;
    protected override ItemType ITEM_PRODUCED { get; } = SimulationConstants.JewelryValues.ITEM_PRODUCED;
}