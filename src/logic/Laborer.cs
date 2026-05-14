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

    public void consumeAtMarket(Market market) {
        bool isSubsistanceFamerWhoFeedsSelf = !isEmployed;
        if (!isSubsistanceFamerWhoFeedsSelf)
            consumeFood(market);

        //todo consume everything else
    }

    private void consumeFood(Market market) {
        int foodConsumed = consumeDesiredFood(market);
        int remainingRequiredFoodConsmption = SimulationConstants.FOOD_CONSUMPTION_PER_TURN - foodConsumed;

        Optional<ItemType> cheapestFood = getCheapestFoodItem(market);
        while (remainingRequiredFoodConsmption > 0 && cheapestFood.IsPresent()) {
            bool bought = tryBuy(market, cheapestFood.get());
            remainingRequiredFoodConsmption--;
            
            if (bought)
                foodConsumed++;
            
            cheapestFood = getCheapestFoodItem(market);
        }

        if (foodConsumed < SimulationConstants.FOOD_CONSUMPTION_PER_TURN) {
            SimpleLogger.log("Im starving!");
        }
    }

    private bool tryBuy(Market market, ItemType item) {
        bool ret = false;
        Optional<CoinAmount> buyResult = market.tryBuyItems(item, 1);
        if (buyResult.IsPresent()) {
            wealth.subtract(buyResult.get()); //TODO: right now we cant 'not afford' anything
            ret = true;
        }

        return ret;
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
    
    private Optional<ItemType> getCheapestFoodItem(Market market) {
        CoinAmount cheapestItemCost = CoinAmount.MAX_VALUE;
        ItemType cheapestItemType = ItemType.NONE;
        foreach (ItemType food in Items.ALL_FOOD_ITEMS) {
            CoinAmount foodCost = market.getPrice(food);
            if (market.isInStock(food) && foodCost.isLessThan(cheapestItemCost)) {
                cheapestItemCost = foodCost;
                cheapestItemType = food;
            }
        }

        return cheapestItemType.Equals(ItemType.NONE) ? Optional<ItemType>.EMPTY() : new Optional<ItemType>(cheapestItemType);
    }
    
    public void setEmployed(bool isEmployed) {
        this.isEmployed = isEmployed;
    }

    public CoinAmount getWealth() {
        return wealth;
    }
}