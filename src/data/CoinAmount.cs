using System;

namespace EconSim.data;

public class CoinAmount
{
    public static readonly CoinAmount MaxValue = new CoinAmount(Int32.MaxValue);
    public static readonly CoinAmount MinValue = new CoinAmount(1);
    
    private int cents = 0;

    public CoinAmount() {}
    
    public CoinAmount(int cents) {
        this.cents = cents;
    }
    
    public CoinAmount(CoinAmount that) {
        this.cents = that.cents;
    }

    public void Add(CoinAmount that) {
        this.cents += that.cents;
    }
    
    public void Subtract(CoinAmount that) {
        this.cents -= that.cents;
    }

    public int AsInt() {
        return this.cents;
    }

    public override string ToString() {
        String ret = "";
        if (cents >= 100*100) {
            ret = "g" + (cents / (100*100)).ToString("0.##");
        } else if (cents >= 100) {
            ret = "$" + cents.ToString("0.##");
        }
        else {
            ret = "c" + cents.ToString("0.");
        }
        return ret;
    }

    public bool IsLessThan(CoinAmount that) {
        return this.cents < that.cents;
    }
    
    public bool IsGreaterThan(CoinAmount that) {
        return this.cents > that.cents;
    }

    public static CoinAmount Copper(int copperCoinCount) {
        return new CoinAmount(copperCoinCount);
    }
    
    public static CoinAmount Silver(int silverCoinCount) {
        return new CoinAmount(silverCoinCount * 100);
    }
    
    public static CoinAmount Gold(int goldCoinCount) {
        return new CoinAmount(goldCoinCount * 100 * 100);
    }
    
    public static CoinAmount UnsafeGetDivideBy(CoinAmount coinAmount, int denominator) {        
        return new CoinAmount(coinAmount.cents / denominator);
    }
    
    public static double UnsafeGetDivideBy(CoinAmount numerator, CoinAmount denominator) {
        return numerator.cents / denominator.cents;
    }
    
    public static CoinAmount GetMultiplyBy(CoinAmount coinAmount, double factor)
    {
        double cents = coinAmount.cents * factor;
        return new CoinAmount( (int) cents);
    }
}