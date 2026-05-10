using System;
using System.Collections.Generic;

namespace EconSim.logic;

public abstract class Building
{
    protected abstract int PRODUCTION_PER_PERSONYEAR { get; }
    protected abstract int WAGE_PER_PERSONYEAR { get; }
    protected abstract ItemType ITEM_PRODUCED { get; }
    protected Town hostTown;
    protected int profit = 0;
    protected Stack<Laborer> employees = new Stack<Laborer>();
    protected float productionCarryover = 0;
    
    protected Building(Town hostTown) {
        this.hostTown = hostTown;
        
        //todo: fix the types for this stuff, this might get ugly with large TURNS_IN_YEAR
        if (PRODUCTION_PER_PERSONYEAR % TurnAndTimeManager.TURNS_IN_A_YEAR != 0)
            throw new ArgumentException("PRODUCTION_PER_PERSONYEAR must be divisible by TurnAndTimeManager.TURNS_IN_A_YEAR");

        if (WAGE_PER_PERSONYEAR % TurnAndTimeManager.TURNS_IN_A_YEAR != 0)
            throw new ArgumentException("WAGE_PER_PERSONYEAR must be divisible by TurnAndTimeManager.TURNS_IN_A_YEAR");
    }

    public virtual void doProductionTurn() {        
        int productionPerPersonTurn = PRODUCTION_PER_PERSONYEAR / TurnAndTimeManager.TURNS_IN_A_YEAR;
        int wagePerPersonTurn = WAGE_PER_PERSONYEAR / TurnAndTimeManager.TURNS_IN_A_YEAR;
        
        productionCarryover += productionPerPersonTurn * employees.Count;
        int production = (int)productionCarryover;
        productionCarryover -= (int) productionCarryover;
        
        if (production > 0) {
            this.profit += hostTown.getMarket().sellItems(ITEM_PRODUCED, production);
            foreach (Laborer employee in employees) {
                employee.pay(wagePerPersonTurn);
                profit -= wagePerPersonTurn;
            }
        }
    }

    public int getProfit() {
        return profit;
    }
    
    public virtual bool employWorkers(int amount) {
        if (amount > hostTown.getUnemployedPopulation().Count) {
            return false;
        }

        for (int i = 0; i < amount; i++) {
            Laborer laborer = hostTown.getUnemployedPopulation().Pop();
            laborer.setEmployed(true);
            employees.Push(laborer);
        }
        return true;
    }

    public virtual bool unemployWorkers(int amount, Stack<Laborer> unemployedPopulation) {
        if (amount > unemployedPopulation.Count)
            return false;

        for (int i = 0; i < amount; i++) {
            Laborer laborer = employees.Pop();
            laborer.setEmployed(false);
            unemployedPopulation.Push(laborer);
        }
        return true;
    }

    public int getEmployeeCount() {
        return employees.Count;
    }
}