namespace EconSim.logic.buildings;

public class LumberYard : Building
{
    private const int PRODUCTION_PER_PERSONYEAR = 100;
    private const float PRODDUCTION_PER_PERSONTURN = PRODUCTION_PER_PERSONYEAR / TURNS_IN_A_YEAR;

    private float productionCarryover = 0;
    
    public LumberYard(Town hostTown) : base(hostTown) {}

    public override void doProductionTurn() {
        productionCarryover += PRODDUCTION_PER_PERSONTURN * workers;
        int production = (int)productionCarryover;
        productionCarryover -= (int) productionCarryover;

        this.hostTown.getInventory().addItem(ItemType.LUMBER, production);
    }
}