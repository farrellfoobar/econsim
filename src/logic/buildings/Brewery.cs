using EconSim.data;

namespace EconSim.logic.buildings;

public class Brewery(Town hostTown) : Building(hostTown)
{
    protected override double PRODUCTION_PER_PERSONYEAR { get; } = SimulationConstants.BreweryValues.PRODUCTION_PER_PERSONYEAR;
    protected override ItemType ITEM_CONSUMED { get; } = SimulationConstants.BreweryValues.ITEM_CONSUMED;
    protected override double ITEMS_CONSUMED_PER_UNIT_PRODUCED { get; } = SimulationConstants.BreweryValues.CONSUMPTION_PER_UNIT_PRODUCED;
    protected override CoinAmount WAGE_PER_PERSONYEAR { get; } = SimulationConstants.BreweryValues.WAGE_PER_PERSONYEAR;
    protected override ItemType ITEM_PRODUCED { get; } = SimulationConstants.BreweryValues.ITEM_PRODUCED;
}