using System;
using System.Collections.Generic;
using EconSim.data;
using EconSim.logic.buildings;

namespace EconSim.logic;

public abstract class Building
{
    protected abstract CoinAmount WAGE_PER_PERSONYEAR { get; }
    protected abstract ItemType ITEM_PRODUCED { get; }
    protected abstract double PRODUCTION_PER_PERSONYEAR { get; }
    protected abstract ItemType ITEM_CONSUMED { get; }
    protected abstract double ITEMS_CONSUMED_PER_UNIT_PRODUCED { get; }
    protected Town hostTown;
    protected CoinAmount profit = new CoinAmount(0);
    protected Stack<Laborer> employees = new Stack<Laborer>();
    protected double productionOverflow = 0;
    protected double consumptionOverflow = 0;
    
    protected Building(Town hostTown) {
        this.hostTown = hostTown;
        
        //todo: fix the types for this stuff, this might get ugly with large TURNS_IN_YEAR
        if (PRODUCTION_PER_PERSONYEAR % TurnAndTimeManager.TURNS_IN_A_YEAR != 0)
            throw new ArgumentException("PRODUCTION_PER_PERSONYEAR must be divisible by TurnAndTimeManager.TURNS_IN_A_YEAR");
    }

    public virtual void doProductionTurn() {        
        double productionPerPersonTurn = PRODUCTION_PER_PERSONYEAR / TurnAndTimeManager.TURNS_IN_A_YEAR;
        CoinAmount wagePerPersonTurn = CoinAmount.getDivideBy(WAGE_PER_PERSONYEAR, TurnAndTimeManager.TURNS_IN_A_YEAR);
        
        //todo: this is breaking my brain already like three days after I wrote it, tear it out and make production, consumption floats
        productionOverflow += productionPerPersonTurn * employees.Count;
        int productionInt = (int) Math.Truncate(productionOverflow);
        productionOverflow -= productionInt;
        
        consumptionOverflow += productionInt * ITEMS_CONSUMED_PER_UNIT_PRODUCED;
        int consumptionInt = (int) Math.Truncate(productionOverflow);
        consumptionOverflow -= consumptionInt;

        Optional<CoinAmount> productionCostMaybe = hostTown.getMarket().tryBuyItems(ITEM_CONSUMED, consumptionInt);

        if (productionCostMaybe.IsPresent() && productionInt > 0) {
            profit.add(hostTown.getMarket().sellItems(ITEM_PRODUCED, productionInt));
            foreach (Laborer employee in employees) {
                employee.pay(wagePerPersonTurn);
                profit.subtract(wagePerPersonTurn);
            }
        }
    }

    public CoinAmount getProfit() {
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
    
    public CoinAmount GET_WAGE_PER_PERSONYEAR() {
        return WAGE_PER_PERSONYEAR; 
    }
    
    public ItemType GET_ITEM_PRODUCED() {
        return ITEM_PRODUCED; 
    }

    public double GET_PRODUCTION_PER_PERSONYEAR() {
        return PRODUCTION_PER_PERSONYEAR; 
    }

    public ItemType GET_ITEM_CONSUMED() {
        return ITEM_CONSUMED; 
    }

    public double GET_ITEMS_CONSUMED_PER_UNIT_PRODUCED() {
        return ITEMS_CONSUMED_PER_UNIT_PRODUCED; 
    }
}