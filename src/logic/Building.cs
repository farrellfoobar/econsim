using System;

namespace EconSim.logic;

public abstract class Building
{
    protected int workers = 0;
    protected Town hostTown;
    protected int profit = 0;
    
    protected Building(Town hostTown) {
        this.hostTown = hostTown;
    }

    public abstract void doProductionTurn();

    public int getProfit() {
        return profit;
    }
    
    public virtual bool addWorker(int amount) {
        int unemployedPopulation = this.hostTown.getUnemployedPopulation();
        if (amount > unemployedPopulation) {
            return false;
        }

        this.hostTown.setUnemployedPopulation(unemployedPopulation-amount);
        workers += amount;
        return true;
    }
}