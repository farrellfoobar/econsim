using System;
using EconSim.data;

namespace EconSim.logic;

public class Laborer
{
    private int wealth = 0;
    private bool isEmployed = false;
    private float turnLengthConsumptionModifier = 1f / (float) TurnAndTimeManager.TURNS_IN_A_YEAR;
    private ConsumerBehavior consumerBehavior = new ConsumerBehavior();
    
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
        int foodConsumed = consumeDesiredFood(market);
        consumeRemainingFoodAsCheaplyAsPossible(market, foodConsumed);
    }

    private void consumeRemainingFoodAsCheaplyAsPossible(Market market, int foodConsumed) {
        int foodMustConsumeThisTurn = foodConsumptionPerPersonPerYear/TurnAndTimeManager.TURNS_IN_A_YEAR;

        for (int i = foodConsumed; i < foodMustConsumeThisTurn; i++) {
            market.buyItems(getCheapestItem(market), 1);
        }
    }

    private ItemType getCheapestItem(Market market) {
        int cheapestItemCost = Int32.MaxValue;
        ItemType cheapestItemType = ItemType.NONE;
        foreach (ItemType food in Items.ALL_FOOD_ITEMS) {
            int foodCost = market.getPrice(food);
            if (foodCost < cheapestItemCost) {
                cheapestItemCost = foodCost;
                cheapestItemType = food;
            }
        }

        return cheapestItemType;
    }

    private int consumeDesiredFood(Market market) {
        int foodConsumed = 0;
        foreach (ItemType food in Items.ALL_FOOD_ITEMS) {
            int desiredFoodConsumption = consumerBehavior.QuantityPurchasedPerTurn(food, wealth, market.getPrice(food)) / TurnAndTimeManager.TURNS_IN_A_YEAR;
            //TODO: allow buyItems to do a partial buy. This would require returning both the cost and the quantity purchased  
            Optional<int> buyResult = market.buyItems(food, desiredFoodConsumption);
            if (buyResult.IsPresent()) {
                foodConsumed += desiredFoodConsumption;
            }
        }
        
        return foodConsumed;
    }
    
    public void setEmployed(bool isEmployed) {
        this.isEmployed = isEmployed;
    }
}