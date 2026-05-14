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

    private double productionThisYear = 0;
    private TurnAndTimeManager turnAndTimeManager;

    public SubsistenceFarm(Town hostTown, TurnAndTimeManager turnAndTimeManager) : base(hostTown) {
        this.turnAndTimeManager = turnAndTimeManager;
    }
    
    public override void DoProductionTurn() {
        double productionPerPersonTurn = productionPerPersonyear / (double)TurnAndTimeManager.TurnsInAYear;
        productionThisYear += productionPerPersonTurn * this.employees.Count;

        if (turnAndTimeManager.IsHarvestTurn()) {
            this.profit.Add(hostTown.GetMarket().SellItems(ItemType.Grain, (int) productionThisYear));
            //TODO: split profit among employees
            //this will be kinda complicated because employees can change durring the year
        }
    }
    
    public void SetEmployees(Stack<Laborer> unemployedPopulation) {
        this.employees = unemployedPopulation;
    }
    
    public virtual bool EmployWorkers(int amount) {
        throw new NotImplementedException("Cannot employ workers at SubsistenceFarm. subsistence farmers are by " +
                                          "definition unemployed. ");
    }
}