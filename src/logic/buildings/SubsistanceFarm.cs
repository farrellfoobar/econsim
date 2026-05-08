using System;

namespace EconSim.logic.buildings;

public class SubsistanceFarm : Building
{
    private const int PRODUCTION_PER_PERSONYEAR = 100;
    private const int TURNS_IN_A_YEAR = 4;//365;
    private const float PRODDUCTION_PER_PERSONTURN = PRODUCTION_PER_PERSONYEAR / TURNS_IN_A_YEAR;
    
    private int workers = 0;
    private float productionThisYear = 0;

    private int turnCount = 0; //todo: remove me, see isHarvestTime()
    
    public SubsistanceFarm(Town hostTown) : base(hostTown) {}
    
    public override void doProductionTurn() {
        productionThisYear += PRODDUCTION_PER_PERSONTURN * workers;
        turnCount++;

        if (isHarvestTime()) {
            hostTown.getInventory().addItem(ItemType.GRAIN, (int) productionThisYear);
            productionThisYear = 0;
            turnCount = 0;
        }
    }

    //TODO: remove this and do a real time system, but this is fine for now. 
    private bool isHarvestTime() {
        return turnCount == TURNS_IN_A_YEAR;
    }

    public override void addWorker(int amount) {
        workers += amount;
    }
    
    public override void removeWorker(int amount) {
        if(workers-amount < 0) 
            throw new ArgumentException("");

        workers -= amount;
    }
}