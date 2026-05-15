namespace EconSim.logic;

public class TurnAndTimeManager
{
    public const int TurnsInAYear = 4;
    private int turnCount = 0;
    private int years = 0;    
    
    public int GetTurnCount() {
        return turnCount;
    }
    
    public int GetYear() {
        return years;
    }

    public void NextTurn() {
        if (turnCount == TurnsInAYear) {
            years++;
            turnCount = 1;
        }
        turnCount++;
    }

    public bool IsHarvestTurn() {
        return turnCount == TurnsInAYear;
    }

}