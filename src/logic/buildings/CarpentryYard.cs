using EconSim.data;

namespace EconSim.logic.buildings;

public class CarpentryYard(Town hostTown) : Building(hostTown)
{
    protected override double productionPerPersonyear { get; } = SimulationConstants.CarpentryYardValues.ProductionPerPersonyear;
    protected override ItemType itemConsumed { get; } = SimulationConstants.CarpentryYardValues.ItemConsumed;
    protected override double itemsConsumedPerUnitProduced { get; } = SimulationConstants.CarpentryYardValues.ConsumptionPerUnitProduced;
    protected override CoinAmount wagePerPersonyear { get; } = SimulationConstants.CarpentryYardValues.WagePerPersonyear;
    protected override ItemType itemProduced { get; } = SimulationConstants.CarpentryYardValues.ItemProduced;
    protected override CoinAmount buildCost { get; } = SimulationConstants.CarpentryYardValues.BuildCost;
    protected override int maxEmployees { get; } = SimulationConstants.CarpentryYardValues.maxEmployees;
}