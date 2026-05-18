using System;
using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

public abstract class Building
{
    protected abstract CoinAmount wagePerPersonyear { get; }
    protected abstract ItemType itemProduced { get; }
    protected abstract double productionPerPersonyear { get; }
    protected abstract ItemType itemConsumed { get; }
    protected abstract double itemsConsumedPerUnitProduced { get; }
    protected Town hostTown;
    protected CoinAmount wealth = SimulationConstants.BuildingStaringWealth;
    protected Stack<Laborer> employees = new Stack<Laborer>();
    protected double productionOverflow = 0;
    protected double consumptionOverflow = 0;

    private CoinAmount wagePerPersonTurn;
    
    protected Building(Town hostTown) {
        this.hostTown = hostTown;

        if (wagePerPersonyear.AsInt() % TurnAndTimeManager.TurnsInAYear != 0)
            throw new ArgumentException("Wage per person year must be divisible by TurnsInAYear: " + 
                                        wagePerPersonyear.AsInt() + "/" + TurnAndTimeManager.TurnsInAYear + " != 0");
        
        wagePerPersonTurn = new CoinAmount(wagePerPersonyear.AsInt() / TurnAndTimeManager.TurnsInAYear);
    }

    public virtual void DoProductionTurn() {        
        double productionPerPersonTurn = productionPerPersonyear / TurnAndTimeManager.TurnsInAYear;

        double production = productionOverflow + productionPerPersonTurn * employees.Count;
        double consumption = consumptionOverflow + (production * itemsConsumedPerUnitProduced);

        PurchaseResult result = hostTown.GetMarket().TryBuyItems(wealth, itemConsumed, getIntegerComponent(consumption));

        if (result == PurchaseResult.Success) {
            consumptionOverflow = getNonIntegerComponent(consumption);

            hostTown.GetMarket().SellItems(wealth, itemProduced, getIntegerComponent(production));
                        
            productionOverflow = getNonIntegerComponent(production);
            
            foreach (Laborer employee in employees) {
                employee.Pay(wagePerPersonTurn);
                wealth.Subtract(wagePerPersonTurn);
            }
        } else if (result == PurchaseResult.FailedNotInStock) {
            SimpleLogger.Debug(this.GetType().Name + " in " + this.hostTown.getName() + " has no input to produce.");
        } else if (result == PurchaseResult.FailedCantAfford) {
            SimpleLogger.Log(this.GetType().Name + " in " + this.hostTown.getName() + " cannot afford to produce.");
        }
    }

    private double getNonIntegerComponent(double production) {
        return production - (int) Math.Truncate(production);
    }

    private int getIntegerComponent(double consumptionRequired) {
        return(int) consumptionRequired;
    }

    public CoinAmount GetProfit() {
        return wealth;
    }
    
    public virtual bool EmployWorkers(int amount) {
        if (amount > hostTown.GetUnemployedPopulation().Count) {
            return false;
        }

        for (int i = 0; i < amount; i++) {
            Laborer laborer = hostTown.GetUnemployedPopulation().Pop();
            laborer.Employ(this);
            employees.Push(laborer);
        }
        return true;
    }

    public virtual bool UnemployWorkers(int amount, Stack<Laborer> unemployedPopulation) {
        if (amount > unemployedPopulation.Count)
            return false;

        for (int i = 0; i < amount; i++) {
            Laborer laborer = employees.Pop();
            laborer.Unemploy();
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