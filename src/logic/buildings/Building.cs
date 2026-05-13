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
    }

    public virtual void doProductionTurn() {        
        double productionPerPersonTurn = PRODUCTION_PER_PERSONYEAR / TurnAndTimeManager.TURNS_IN_A_YEAR;
        CoinAmount wagePerPersonTurn = CoinAmount.getDivideBy(WAGE_PER_PERSONYEAR, TurnAndTimeManager.TURNS_IN_A_YEAR);

        double production = productionOverflow + productionPerPersonTurn * employees.Count;
        double consumption = consumptionOverflow + (production * ITEMS_CONSUMED_PER_UNIT_PRODUCED);

        Optional<CoinAmount> consumptionCostMaybe = hostTown.getMarket().tryBuyItems(ITEM_CONSUMED, getIntegerComponent(consumption));

        if (consumptionCostMaybe.IsPresent()) {
            profit.subtract(consumptionCostMaybe.get());
            consumptionOverflow = getNonIntegerComponent(consumption);

            CoinAmount turnIncome = hostTown.getMarket().sellItems(ITEM_PRODUCED, getIntegerComponent(production));
            profit.add(turnIncome);
                        
            productionOverflow = getNonIntegerComponent(production);
            
            foreach (Laborer employee in employees) {
                employee.pay(wagePerPersonTurn);
                profit.subtract(wagePerPersonTurn);
            }
        }
    }

    private double getNonIntegerComponent(double production) {
        return production - (int) Math.Truncate(production);
    }

    private int getIntegerComponent(double consumptionRequired) {
        return(int) consumptionRequired;
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

    public Town getTown() {
        return hostTown;
    }
}