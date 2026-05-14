using System;

namespace EconSim.data;

public class CoinAmount
{
    public static readonly CoinAmount MaxValue = new CoinAmount(Double.MaxValue);
    public static readonly CoinAmount MinValue = new CoinAmount(0.01f);
    
    private double cents = 0;

    public CoinAmount() {}
    
    public CoinAmount(double cents) {
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
        if (this.cents < 0f) {
            //throw new NotImplementedException("Coin Amount is negative, maybe it shouldnt do that?");
        }
    }

    public double AsDouble() {
        return this.cents;
    }

    public override string ToString() {
        String ret = "";
        if (cents >= 100) {
            ret = "g" + (cents / 100).ToString("0.##");
        } else if (cents >= 1) {
            ret = "$" + cents.ToString("0.##");
        }
        else if (cents >= MinValue.AsDouble()) {
            ret = "c" + (cents * 100).ToString("0.");
        }
        else {
            ret = ">c" + (cents * 100).ToString("0.");
            SimpleLogger.Debug("Coin amount < 1c: " + cents);
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
        return new CoinAmount(copperCoinCount * 0.01);
    }
    
    public static CoinAmount Silver(int silverCoinCount) {
        return new CoinAmount(silverCoinCount);
    }
    
    public static CoinAmount Gold(int goldCoinCount) {
        return new CoinAmount(goldCoinCount * 100);
    }
    
    public static CoinAmount GetDivideBy(CoinAmount coinAmount, int denominator) {        
        return new CoinAmount(coinAmount.cents / denominator);
    }
    
    public static double GetDivideBy(CoinAmount numerator, CoinAmount denominator) {
        return numerator.cents / denominator.cents;
    }
    
    public static CoinAmount GetMultiplyBy(CoinAmount coinAmount, double factor) {
        return new CoinAmount(coinAmount.cents * factor);
    }

    public static CoinAmount GetAdd(CoinAmount a, CoinAmount b) {
        return new CoinAmount(a.cents + b.cents);
    }
}