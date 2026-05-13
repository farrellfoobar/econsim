using System;
using EconSim.data;

namespace EconSim.logic;

public class Laborer
{
    private CoinAmount wealth = new CoinAmount(0);
    private bool isEmployed = false;
    private double turnLengthConsumptionModifier = 1f / (double) TurnAndTimeManager.TURNS_IN_A_YEAR;
    private ConsumerBehavior consumerBehavior = new ConsumerBehavior();
    
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

        ItemType cheapestItem = getCheapestItem(market);
        for (int i = foodConsumed; i < SimulationConstants.FOOD_CONSUMPTION_PER_TURN; i++) {
            Optional<CoinAmount> result = market.tryBuyItems(cheapestItem, 1);
            if (result.IsPresent())
                foodConsumed++;
        }
        
        if(foodConsumed < SimulationConstants.FOOD_CONSUMPTION_PER_TURN)
            SimpleLogger.log("Im starving!!!");
    }

    private int consumeDesiredFood(Market market) {
        int foodConsumed = 0;
        foreach (ItemType food in Items.ALL_FOOD_ITEMS) {
            int desiredFoodConsumption = consumerBehavior.QuantityPurchasedPerTurn(food, wealth, market.getPrice(food));
            Optional<CoinAmount> buyResult = market.tryBuyItems(food, desiredFoodConsumption);
            if (buyResult.IsPresent()) {
                foodConsumed += desiredFoodConsumption;
            }
        }
        
        return foodConsumed;
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
    
    public void setEmployed(bool isEmployed) {
        this.isEmployed = isEmployed;
    }
}