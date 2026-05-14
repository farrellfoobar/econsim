using System;
using System.Collections.Generic;
using EconSim.data;

namespace EconSim.logic;

public class ConsumerBehavior
{
    private const int decimalPrecisionOfDoubleToIntProbability = 2;
    private Random random = new Random(0); // 0 so we get the same random numbers from run to run
    
    public int QuantityPurchasedPerTurn(ItemType itemType, CoinAmount wealth, CoinAmount itemPrice) {
        double quantityDouble = quantityPurchasedPerYear(itemType, wealth, itemPrice) / TurnAndTimeManager.TurnsInAYear;

        return doubleToIntAndProbability(quantityDouble);
    }

    /*
     * Convert a double value to an integer by treating the decimal component of the double as a probability so that
     * theoretical consumption values are respected while also consuming strictly integer values of items.
     * Ex: If QuantityPurchasedPerYear(fish, ...) := 15, and TURNS_IN_YEAR:=4 then quantityDouble = 3.75. We
     * determine if this particular individual consumes 3 or 4 fish by rolling a die with 0.75 odds.
     */
    private int doubleToIntAndProbability(double quantityDouble) {
        int integerComponent = (int) Math.Truncate(quantityDouble);
        double doubleComponent = quantityDouble - integerComponent;

        int powerOfTen = (int) Math.Pow(10, decimalPrecisionOfDoubleToIntProbability);
        int thresh = (int) ( doubleComponent * powerOfTen);
        if (random.Next(0, powerOfTen) <= thresh) {
            integerComponent++;
        }
        
        return integerComponent;
    }

    private double quantityPurchasedPerYear(ItemType itemType, CoinAmount wealth, CoinAmount price) {
        //TODO: take wealth into consideration
 
        double m = SimulationConstants.DemandSlope[itemType];
        double elasticity = SimulationConstants.DemandElasticity[itemType];
        
        double quantityDemanded = m / Math.Pow(price.AsInt(), elasticity);
        
        return quantityDemanded;
    }
}