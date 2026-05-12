using System;
using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic.buildings;

public class SubsistenceFarm : Building
{   
    protected override int PRODUCTION_PER_PERSONYEAR { get; } = SimulationConstants.SubsistanceFarmValues.PRODUCTION_PER_PERSONYEAR;
    protected override ItemType ITEM_CONSUMED { get; } = SimulationConstants.SubsistanceFarmValues.ITEM_CONSUMED;
    protected override int ITEMS_CONSUMED_PER_UNIT_PRODUCED { get; } = SimulationConstants.SubsistanceFarmValues.CONSUMPTION_PER_UNIT_PRODUCED;
    protected override CoinAmount WAGE_PER_PERSONYEAR { get; } = SimulationConstants.SubsistanceFarmValues.WAGE_PER_PERSONYEAR;
    protected override ItemType ITEM_PRODUCED { get; } = SimulationConstants.SubsistanceFarmValues.ITEM_PRODUCED;

    private double productionThisYear = 0;
    private TurnAndTimeManager turnAndTimeManager;

    public SubsistenceFarm(Town hostTown, TurnAndTimeManager turnAndTimeManager) : base(hostTown) {
        this.turnAndTimeManager = turnAndTimeManager;
    }
    
    public override void doProductionTurn() {
        double productionPerPersonTurn = PRODUCTION_PER_PERSONYEAR / (double)TurnAndTimeManager.TURNS_IN_A_YEAR;
        productionThisYear += productionPerPersonTurn * this.employees.Count;

        if (turnAndTimeManager.isHarvestTurn()) {
            this.profit.add(hostTown.getMarket().sellItems(ItemType.GRAIN, (int) productionThisYear));
            //TODO: split profit among employees
            //this will be kinda complicated because employees can change durring the year
        }
    }
    
    public void setEmployees(Stack<Laborer> unemployedPopulation) {
        this.employees = unemployedPopulation;
    }
    
    public virtual bool employWorkers(int amount) {
        throw new NotImplementedException("Cannot employ workers at SubsistenceFarm. subsistence farmers are by " +
                                          "definition unemployed. ");
    }
}