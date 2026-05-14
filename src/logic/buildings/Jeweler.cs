using EconSim.data;

namespace EconSim.logic.buildings;

public class Jeweler(Town hostTown) : Building(hostTown)
{
    protected override double productionPerPersonyear { get; } = SimulationConstants.JewelryValues.ProductionPerPersonyear;
    protected override ItemType itemConsumed { get; } = SimulationConstants.JewelryValues.ItemConsumed;
    protected override double itemsConsumedPerUnitProduced { get; } = SimulationConstants.JewelryValues.ConsumptionPerUnitProduced;
    protected override CoinAmount wagePerPersonyear { get; } = SimulationConstants.JewelryValues.WagePerPersonyear;
    protected override ItemType itemProduced { get; } = SimulationConstants.JewelryValues.ItemProduced;
}