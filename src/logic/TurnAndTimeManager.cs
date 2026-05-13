namespace EconSim.logic;

public class TurnAndTimeManager
{
    public const int TURNS_IN_A_YEAR = 4;
    private int turnCount = 0;
    private int years = 0;    
    
    public int getTurnCount() {
        return turnCount;
    }
    
    public int getYear() {
        return years;
    }

    public void nextTurn() {
        if (turnCount == TURNS_IN_A_YEAR) {
            years++;
            turnCount = 1;
        }
        turnCount++;
    }

    public bool isHarvestTurn() {
        return turnCount == TURNS_IN_A_YEAR;
    }

}