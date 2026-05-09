using System;

namespace EconSim.logic.buildings;

public class LumberYard : Building
{
    private const int PRODUCTION_PER_PERSONYEAR = 100;
    private const float PRODDUCTION_PER_PERSONTURN = PRODUCTION_PER_PERSONYEAR / (float) TurnAndTimeManager.TURNS_IN_A_YEAR;
    private const int WAGE_PER_YEAR = 400;
    private const float WAGE_PER_TURN = WAGE_PER_YEAR / (float) TurnAndTimeManager.TURNS_IN_A_YEAR;
    
    private float productionCarryover = 0;
    private int profit = 0;
    
    public LumberYard(Town hostTown) : base(hostTown) {}

    public override void doProductionTurn() {
        productionCarryover += PRODDUCTION_PER_PERSONTURN * workers;
        int production = (int)productionCarryover;
        productionCarryover -= (int) productionCarryover;

        if (production > 0) {
            this.profit += hostTown.getMarket().sellItems(ItemType.LUMBER, production);
            this.profit -= (int) WAGE_PER_TURN * workers; //todo: fix the types for this stuff, this might get ugly with large TURNS_IN_YEAR
        }
    }
}