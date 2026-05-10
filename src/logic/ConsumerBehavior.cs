using System;
using EconSim.data;

namespace EconSim.logic;

public class ConsumerBehavior
{
    private const int DECIMAL_PRECISION_OF_FLOAT_TO_INT_PROBABILITY = 2;
    private Random random = new Random(0); // 0 so we get the same random numbers from run to run
    
    public int QuantityPurchasedPerTurn(ItemType itemType, CoinAmount wealth, CoinAmount getPrice) {
        float quantityFloat = QuantityPurchasedPerYear(itemType, wealth, getPrice) / TurnAndTimeManager.TURNS_IN_A_YEAR;

        return floatToIntAndProbability(quantityFloat);
    }

    /*
     * Convert a float value to an integer by treating the decimal component of the float as a probability so that
     * theoretical consumption values are respected while also consuming strictly integer values of items.
     * Ex: If QuantityPurchasedPerYear(fish, ...) := 15, and TURNS_IN_YEAR:=4 then quantityFloat = 3.75. We
     * determine if this particular individual consumes 3 or 4 fish by rolling a die with 0.75 odds.
     */
    private int floatToIntAndProbability(float quantityFloat) {
        int integerComponent = (int) Math.Truncate(quantityFloat);
        float floatComponent = quantityFloat - integerComponent;

        int powerOfTen = (int) Math.Pow(10, DECIMAL_PRECISION_OF_FLOAT_TO_INT_PROBABILITY);
        int thresh = (int) ( floatComponent * powerOfTen);
        if (random.Next(0, powerOfTen) <= thresh) {
            integerComponent++;
        }
        
        return integerComponent;
    }

    private int QuantityPurchasedPerYear(ItemType itemType, CoinAmount wealth, CoinAmount getPrice) {
        //TODO: Take price into consideration with realistic elasticity of demand values ex: QuantityDesired=Const-Elasticity*price
        //TODO: take wealth into consideration
        int quantity = 0;
        switch (itemType) {
            case ItemType.GRAIN:
                quantity = 15;
                break;
            case ItemType.FISH:
                quantity = 5;
                break;
        }
        
        return quantity;
    }
}