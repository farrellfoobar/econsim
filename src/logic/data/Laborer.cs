using System;
using EconSim.data;

namespace EconSim.logic;

public class Laborer
{
    private CoinAmount wealth = CoinAmount.Silver(10);
    private Optional<Building> workplace = Optional<Building>.Empty();
    private ConsumerBehavior consumerBehavior = new ConsumerBehavior();
    private Random random;

    public Laborer()
    {
        random = new Random(this.GetHashCode());
    }
    
    public void Pay(CoinAmount wage) {
        wealth.Add(wage);
    }

    public void ConsumeAtMarket(Market market) {
        if (IsEmployed())
            consumeFood(market);

        //todo consume everything else
    }

    private void consumeFood(Market market) {
        int foodConsumed = 0;
        if (!isPoor()) {
            foodConsumed += consumeDesiredFood(market);
        }
        foodConsumed += consumeRequiredFood(market, foodConsumed);

        if (foodConsumed < SimulationConstants.FoodConsumptionPerTurn) {
            int i = random.Next(0, 10);
            if (i.Equals(1)) {
                Unemploy();
            }
        }
    }

    private int consumeDesiredFood(Market market) {
        int foodConsumed = 0;
        foreach (ItemType food in Items.AllFoodItems) {
            int desiredFoodConsumption = consumerBehavior.QuantityPurchasedPerTurn(food, wealth, market.GetPrice(food));
            PurchaseResult result = market.TryBuyItems(wealth, food, desiredFoodConsumption);

            if (result == PurchaseResult.Success) {
                foodConsumed += desiredFoodConsumption;
            }
        }
        
        return foodConsumed;
    }
    
    private int consumeRequiredFood(Market market, int foodAlreadyConsumed)
    {
        int foodConsumed = 0;
        int foodToConsume = SimulationConstants.FoodConsumptionPerTurn - foodAlreadyConsumed;
        ItemType cheapestFood = getCheapestFoodItemInStock(market, foodToConsume);

        if (cheapestFood != ItemType.None) {
            PurchaseResult result = market.TryBuyItems(wealth, cheapestFood, foodToConsume);

            if (result == PurchaseResult.Success) {
                foodConsumed += foodToConsume;
            }
        }
        
        return foodConsumed;
    }

    private ItemType getCheapestFoodItemInStock(Market market, int quantity) {
        CoinAmount cheapestItemCost = CoinAmount.MaxValue;
        ItemType cheapestItemType = ItemType.None;
        
        foreach (ItemType food in Items.AllFoodItems) {
            CoinAmount foodCost = market.GetPrice(food);
            if (market.IsInStock(food, quantity) && foodCost.IsLessThan(cheapestItemCost)) {
                cheapestItemCost = foodCost;
                cheapestItemType = food;
            }
        }

        return cheapestItemType;
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

    private bool isPoor()
    {
        return wealth.IsGreaterThan(SimulationConstants.PovertyLineWealth);
    }
    
    public void Employ(Building building)
    {
        workplace = new Optional<Building>(building);
    }

    public void Unemploy()
    {
        workplace.Get().UnemployWorker(this);
        workplace = Optional<Building>.Empty();
    }
}