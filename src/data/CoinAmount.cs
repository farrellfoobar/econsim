using System;

namespace EconSim.data;

public class CoinAmount()
{
    public static readonly CoinAmount MAX_VALUE = new CoinAmount(Double.MaxValue);
    public static readonly CoinAmount MIN_VALUE = new CoinAmount(0.01f);
    
    private double cents = 0;
    
    public CoinAmount(double cents) : this() {
        this.cents = cents;
    }

    public void add(CoinAmount that) {
        this.cents += that.cents;
    }
    
    public void subtract(CoinAmount that) {
        this.cents -= that.cents;
        if (this.cents < 0f) {
            //throw new NotImplementedException("Coin Amount is negative, maybe it shouldnt do that?");
        }
    }

    public double asDouble() {
        return this.cents;
    }

    public override string ToString() {
        String ret = "";
        if (cents >= 100) {
            ret = "g" + (cents / 100).ToString("0.##");
        } else if (cents >= 1) {
            ret = "$" + cents.ToString("0.##");
        }
        else {
            ret = "c" + (cents * 100).ToString("0.");
        }
        return ret;
    }

    public bool isLessThan(CoinAmount that) {
        return this.cents < that.cents;
    }
    
    public bool isGreaterThan(CoinAmount that) {
        return this.cents > that.cents;
    }

    public static CoinAmount Copper(int copperCoinCount) {
        return new CoinAmount(copperCoinCount * 0.01);
    }
    
    public static CoinAmount Silver(int silverCoinCount) {
        return new CoinAmount(silverCoinCount);
    }
    
    public static CoinAmount Gold(int goldCoinCount) {
        return new CoinAmount(goldCoinCount * 100);
    }
    
    public static CoinAmount getDivideBy(CoinAmount coinAmount, int denominator) {
        //if (coinAmount.cents % denominator != 0)
            //throw new ArgumentException("Cant divide " + coinAmount.cents + " cents by " + denominator);
        
        return new CoinAmount(coinAmount.cents / denominator);
    }
    
    public static double getDivideBy(CoinAmount numerator, CoinAmount denominator) {
        return numerator.cents / denominator.cents;
    }
    
    public static CoinAmount getMultiplyBy(CoinAmount coinAmount, double factor) {
        return new CoinAmount(coinAmount.cents * factor);
    }

    public static CoinAmount getAdd(CoinAmount a, CoinAmount b) {
        return new CoinAmount(a.cents + b.cents);
    }
}