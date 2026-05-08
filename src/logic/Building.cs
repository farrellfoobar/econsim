using System;

namespace EconSim.logic;

public abstract class Building
{
    protected const int TURNS_IN_A_YEAR = 4;
    
    protected int workers = 0;
    
    protected Town hostTown;
    protected Building(Town hostTown) {
        this.hostTown = hostTown;
    }

    public abstract void doProductionTurn();
    
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