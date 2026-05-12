using System;
using EconSim.data;

namespace EconSim.logic;

public class Laborer
{
    private CoinAmount wealth = new CoinAmount(0);
    private bool isEmployed = false;
    private double turnLengthConsumptionModifier = 1f / (double) TurnAndTimeManager.TURNS_IN_A_YEAR;
    private ConsumerBehavior consumerBehavior = new ConsumerBehavior();
    
    private const int foodConsumptionPerPersonPerYear = 20;

    public void pay(CoinAmount wage) {
        wealth.add(wage);
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
            market.tryBuyItems(getCheapestItem(market), 1);
        }
    }

    private ItemType getCheapestItem(Market market) {
        CoinAmount cheapestItemCost = CoinAmount.MAX_VALUE;
        ItemType cheapestItemType = ItemType.NONE;
        foreach (ItemType food in Items.ALL_FOOD_ITEMS) {
            CoinAmount foodCost = market.getPrice(food);
            if (foodCost.isLessThan(cheapestItemCost) ) {
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
            Optional<CoinAmount> buyResult = market.tryBuyItems(food, desiredFoodConsumption);
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