using System;
using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic.buildings;

public class SubsistenceFarm : Building
{   
    protected override double productionPerPersonyear { get; } = SimulationConstants.SubsistanceFarmValues.ProductionPerPersonyear;
    protected override ItemType itemConsumed { get; } = SimulationConstants.SubsistanceFarmValues.ItemConsumed;
    protected override double itemsConsumedPerUnitProduced { get; } = SimulationConstants.SubsistanceFarmValues.ConsumptionPerUnitProduced;
    protected override CoinAmount wagePerPersonyear { get; } = SimulationConstants.SubsistanceFarmValues.WagePerPersonyear;
    protected override ItemType itemProduced { get; } = SimulationConstants.SubsistanceFarmValues.ItemProduced;
    protected override CoinAmount buildCost { get; } = SimulationConstants.SubsistanceFarmValues.BuildCost;
    protected override int maxEmployees { get; } = SimulationConstants.SubsistanceFarmValues.maxEmployees;

    private double productionThisYear = 0;
    private TurnAndTimeManager turnAndTimeManager;

    public SubsistenceFarm(Town hostTown, TurnAndTimeManager turnAndTimeManager) : base(hostTown) {
        this.turnAndTimeManager = turnAndTimeManager;
    }
    
    public override void DoProductionTurn() {
        double productionPerPersonTurn = productionPerPersonyear / (double)TurnAndTimeManager.TurnsInAYear;
        productionThisYear += productionPerPersonTurn * this.employees.Count;

        if (turnAndTimeManager.IsHarvestTurn()) {
            hostTown.GetMarket().SellItems(wealth, ItemType.Grain, (int) productionThisYear);
            //TODO: split profit among employees
            //this will be kinda complicated because employees can change durring the year
        }
    }
    
    public void SetEmployees(List<Laborer> unemployedPopulation) {
        this.employees = unemployedPopulation;
    }
}