namespace EconSim.logic;

public class Laborer
{
    private int wealth = 0;
    private bool isEmployed = false;
    private float turnLengthConsumptionModifier = 1f / (float) TurnAndTimeManager.TURNS_IN_A_YEAR;
    
    private const int foodConsumptionPerPersonPerYear = 20;

    public void pay(int wage) {
        wealth += wage;
    }

    //For reference subsistance farmer makes $20/yr, LumberYard laborer makes 60/yr
    //For reference subsistance farmer makes $5/tr, LumberYard laborer makes 15/trn
    public void consumeAtMarket(Market market) {
        bool isSubsistanceFamerWhoFeedsSelf = !isEmployed;
        if (!isSubsistanceFamerWhoFeedsSelf)
            consumeFood(market);

        //todo consume everything else
    }

    private void consumeFood(Market market) {
        //todo: this
        //grainConsumed = wealth =< POOR ? foodConsumptionPerPersonPerYear/TurnsInAYear : f(price)
        //int fishConsumed = wealth > POOR ? ConsumerBehavior.probabilityOfPurchase(wealth, price) : 0;
    }

    public void setEmployed(bool isEmployed) {
        this.isEmployed = isEmployed;
    }
}