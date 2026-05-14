using EconSim.data;

namespace EconSim.logic.buildings;

public class Brewery(Town hostTown) : Building(hostTown)
{
    protected override double productionPerPersonyear { get; } = SimulationConstants.BreweryValues.ProductionPerPersonyear;
    protected override ItemType itemConsumed { get; } = SimulationConstants.BreweryValues.ItemConsumed;
    protected override double itemsConsumedPerUnitProduced { get; } = SimulationConstants.BreweryValues.ConsumptionPerUnitProduced;
    protected override CoinAmount wagePerPersonyear { get; } = SimulationConstants.BreweryValues.WagePerPersonyear;
    protected override ItemType itemProduced { get; } = SimulationConstants.BreweryValues.ItemProduced;
}