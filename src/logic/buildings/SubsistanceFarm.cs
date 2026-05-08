namespace EconSim.logic.buildings;

public class SubsistanceFarm : Building
{
    private const int PRODUCTION_PER_PERSONYEAR = 20;
    private const float PRODDUCTION_PER_PERSONTURN = PRODUCTION_PER_PERSONYEAR / TURNS_IN_A_YEAR;
    
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

    public override bool addWorker(int amount) {
        //Subsistance farmers are by definition unemployed, so dont reduce the 
        if (this.hostTown.getPopulation() > amount) {
            return false;
        }
        
        workers += amount;
        return true;
    }
    
    //TODO: remove this and do a real time system, but this is fine for now. 
    private bool isHarvestTime() {
        return turnCount == TURNS_IN_A_YEAR;
    }
}