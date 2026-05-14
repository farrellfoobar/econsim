using System;
using System.Collections.Generic;
using EconSim.data;
using EconSim.logic.buildings;

namespace EconSim.logic;

public abstract class Building
{
    protected abstract CoinAmount wagePerPersonyear { get; }
    protected abstract ItemType itemProduced { get; }
    protected abstract double productionPerPersonyear { get; }
    protected abstract ItemType itemConsumed { get; }
    protected abstract double itemsConsumedPerUnitProduced { get; }
    protected Town hostTown;
    protected CoinAmount profit = new CoinAmount(0);
    protected Stack<Laborer> employees = new Stack<Laborer>();
    protected double productionOverflow = 0;
    protected double consumptionOverflow = 0;
    
    protected Building(Town hostTown) {
        this.hostTown = hostTown;
    }

    public virtual void DoProductionTurn() {        
        double productionPerPersonTurn = productionPerPersonyear / TurnAndTimeManager.TurnsInAYear;
        CoinAmount wagePerPersonTurn = CoinAmount.GetDivideBy(wagePerPersonyear, TurnAndTimeManager.TurnsInAYear);

        double production = productionOverflow + productionPerPersonTurn * employees.Count;
        double consumption = consumptionOverflow + (production * itemsConsumedPerUnitProduced);

        Optional<CoinAmount> consumptionCostMaybe = hostTown.GetMarket().TryBuyItems(itemConsumed, getIntegerComponent(consumption));

        if (consumptionCostMaybe.IsPresent()) {
            profit.Subtract(consumptionCostMaybe.Get());
            consumptionOverflow = getNonIntegerComponent(consumption);

            CoinAmount turnIncome = hostTown.GetMarket().SellItems(itemProduced, getIntegerComponent(production));
            profit.Add(turnIncome);
                        
            productionOverflow = getNonIntegerComponent(production);
            
            foreach (Laborer employee in employees) {
                employee.Pay(wagePerPersonTurn);
                profit.Subtract(wagePerPersonTurn);
            }
        }
    }

    private double getNonIntegerComponent(double production) {
        return production - (int) Math.Truncate(production);
    }

    private int getIntegerComponent(double consumptionRequired) {
        return(int) consumptionRequired;
    }

    public CoinAmount GetProfit() {
        return profit;
    }
    
    public virtual bool EmployWorkers(int amount) {
        if (amount > hostTown.GetUnemployedPopulation().Count) {
            return false;
        }

        for (int i = 0; i < amount; i++) {
            Laborer laborer = hostTown.GetUnemployedPopulation().Pop();
            laborer.SetEmployed(true);
            employees.Push(laborer);
        }
        return true;
    }

    public virtual bool UnemployWorkers(int amount, Stack<Laborer> unemployedPopulation) {
        if (amount > unemployedPopulation.Count)
            return false;

        for (int i = 0; i < amount; i++) {
            Laborer laborer = employees.Pop();
            laborer.SetEmployed(false);
            unemployedPopulation.Push(laborer);
        }
        return true;
    }

    public int GetEmployeeCount() {
        return employees.Count;
    }
    
    public CoinAmount GET_WAGE_PER_PERSONYEAR() {
        return wagePerPersonyear; 
    }
    
    public ItemType GET_ITEM_PRODUCED() {
        return itemProduced; 
    }

    public double GET_PRODUCTION_PER_PERSONYEAR() {
        return productionPerPersonyear; 
    }

    public ItemType GET_ITEM_CONSUMED() {
        return itemConsumed; 
    }

    public double GET_ITEMS_CONSUMED_PER_UNIT_PRODUCED() {
        return itemsConsumedPerUnitProduced; 
    }

    public Town GetTown() {
        return hostTown;
    }
}