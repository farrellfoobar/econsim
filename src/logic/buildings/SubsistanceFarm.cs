namespace EconSim.logic.buildings;

public class SubsistanceFarm : Building
{
    private const int PRODUCTION_PER_PERSONYEAR = 20;
    private const float PRODDUCTION_PER_PERSONTURN = PRODUCTION_PER_PERSONYEAR / (float) TurnAndTimeManager.TURNS_IN_A_YEAR;
    
    private float productionThisYear = 0;
    private TurnAndTimeManager turnAndTimeManager;

    public SubsistanceFarm(Town hostTown, TurnAndTimeManager turnAndTimeManager) : base(hostTown) {
        this.turnAndTimeManager = turnAndTimeManager;
    }
    
    public override void doProductionTurn() {
        productionThisYear += PRODDUCTION_PER_PERSONTURN * workers;

        if (turnAndTimeManager.isHarvestTurn()) {
            hostTown.getInventory().addItem(ItemType.GRAIN, (int) productionThisYear);
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
    
}