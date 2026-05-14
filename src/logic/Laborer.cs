using System;
using EconSim.data;

namespace EconSim.logic;

public class Laborer
{
    private CoinAmount wealth = new CoinAmount(0);
    private bool isEmployed = false;
    private double turnLengthConsumptionModifier = 1f / (double) TurnAndTimeManager.TurnsInAYear;
    private ConsumerBehavior consumerBehavior = new ConsumerBehavior();
    
    public void Pay(CoinAmount wage) {
        wealth.Add(wage);
    }

    public void ConsumeAtMarket(Market market) {
        bool isSubsistanceFamerWhoFeedsSelf = !isEmployed;
        if (!isSubsistanceFamerWhoFeedsSelf)
            consumeFood(market);

        //todo consume everything else
    }

    private void consumeFood(Market market) {
        int foodConsumed = consumeDesiredFood(market);
        int remainingRequiredFoodConsmption = SimulationConstants.FoodConsumptionPerTurn - foodConsumed;

        Optional<ItemType> cheapestFood = getCheapestFoodItem(market);
        while (remainingRequiredFoodConsmption > 0 && cheapestFood.IsPresent()) {
            bool bought = tryBuy(market, cheapestFood.Get());
            remainingRequiredFoodConsmption--;
            
            if (bought)
                foodConsumed++;
            
            cheapestFood = getCheapestFoodItem(market);
        }

        if (foodConsumed < SimulationConstants.FoodConsumptionPerTurn) {
            SimpleLogger.Log("Im starving!");
        }
    }

    private bool tryBuy(Market market, ItemType item) {
        bool ret = false;
        Optional<CoinAmount> buyResult = market.TryBuyItems(item, 1);
        if (buyResult.IsPresent()) {
            wealth.Subtract(buyResult.Get()); //TODO: right now we cant 'not afford' anything
            ret = true;
        }

        return ret;
    }


    private int consumeDesiredFood(Market market) {
        int foodConsumed = 0;
        foreach (ItemType food in Items.AllFoodItems) {
            int desiredFoodConsumption = consumerBehavior.QuantityPurchasedPerTurn(food, wealth, market.GetPrice(food));
            Optional<CoinAmount> buyResult = market.TryBuyItems(food, desiredFoodConsumption);
            if (buyResult.IsPresent()) {
                foodConsumed += desiredFoodConsumption;
            }
        }
        
        return foodConsumed;
    }
    
    private Optional<ItemType> getCheapestFoodItem(Market market) {
        CoinAmount cheapestItemCost = CoinAmount.MaxValue;
        ItemType cheapestItemType = ItemType.None;
        foreach (ItemType food in Items.AllFoodItems) {
            CoinAmount foodCost = market.GetPrice(food);
            if (market.IsInStock(food) && foodCost.IsLessThan(cheapestItemCost)) {
                cheapestItemCost = foodCost;
                cheapestItemType = food;
            }
        }

        return cheapestItemType.Equals(ItemType.None) ? Optional<ItemType>.Empty() : new Optional<ItemType>(cheapestItemType);
    }
    
    public void SetEmployed(bool isEmployed) {
        this.isEmployed = isEmployed;
    }

    public CoinAmount GetWealth() {
        return wealth;
    }
}