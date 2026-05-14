using System;
using EconSim.data;

namespace EconSim.logic;

public class Laborer
{
    private CoinAmount wealth = CoinAmount.Silver(10);
    private Optional<Building> workplace = Optional<Building>.Empty();
    private ConsumerBehavior consumerBehavior = new ConsumerBehavior();
    
    public void Pay(CoinAmount wage) {
        wealth.Add(wage);
    }

    public void ConsumeAtMarket(Market market) {
        if (IsEmployed())
            consumeFood(market);

        //todo consume everything else
    }

    private void consumeFood(Market market) {
        int foodConsumed = consumeDesiredFood(market);
        foodConsumed += consumeRequiredFood(market, foodConsumed);
        
        if (foodConsumed < SimulationConstants.FoodConsumptionPerTurn) {
            SimpleLogger.Log("Im starving!");
        }
    }

    private int consumeDesiredFood(Market market) {
        int foodConsumed = 0;
        foreach (ItemType food in Items.AllFoodItems) {
            int desiredFoodConsumption = consumerBehavior.QuantityPurchasedPerTurn(food, wealth, market.GetPrice(food));
            foodConsumed = tryBuy(market, food, desiredFoodConsumption);
        }
        
        return foodConsumed;
    }
    
    private int consumeRequiredFood(Market market, int foodAlreadyConsumed)
    {
        int foodConsumed = 0;
        int foodToConsume = SimulationConstants.FoodConsumptionPerTurn - foodAlreadyConsumed;
        
        Optional<ItemType> cheapestFood = getCheapestFoodItem(market);
        
        while (foodToConsume > 0 && cheapestFood.IsPresent()) {
            bool bought = tryBuy(market, cheapestFood.Get(), 1) == 1;
            foodToConsume--;
            
            if (bought)
                foodConsumed++;
            
            cheapestFood = getCheapestFoodItem(market);
        }
        
        return foodConsumed;
    }

    private int tryBuy(Market market, ItemType item, int number) {
        int bought = 0;
        while (market.IsInStock(item) && canAffort(market, item, 1) && bought < number)
        {
            Optional<CoinAmount> buyResult = market.TryBuyItems(item, 1);
            if (buyResult.IsPresent()) {
                wealth.Subtract(buyResult.Get());
                bought++;
            }
        }

        return bought;
    }

    private bool canAffort(Market market, ItemType item, int numDesired)
    {
        return wealth.IsGreaterThan(CoinAmount.GetMultiplyBy(market.GetPrice(item), numDesired));
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

    public CoinAmount GetWealth() {
        return wealth;
    }
    
    public void TestingSetWealth(CoinAmount wealth) {
        this.wealth = wealth;
    }

    private bool IsEmployed()
    {
        return workplace.IsPresent();
    }
    
    public void Employ(Building building)
    {
        workplace = new Optional<Building>(building);
    }

    public void Unemploy()
    {
        workplace = Optional<Building>.Empty();
    }
}