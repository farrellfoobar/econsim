using System;

namespace EconSim.data;

public class CoinAmount()
{
    private double cents = 0;
    public static readonly CoinAmount MAX_VALUE = new CoinAmount(Double.MaxValue);

    public CoinAmount(double cents) : this() {
        this.cents = cents;
    }

    public void add(CoinAmount that) {
        this.cents += that.cents;
    }
    
    public void subtract(CoinAmount that) {
        this.cents -= that.cents;
        if (this.cents < 0) {
            throw new NotImplementedException("Coin Amount is negative, maybe it shouldnt do that?");
        }
    }
    
    public static explicit operator CoinAmount(double cents) {
        return new CoinAmount(cents);
    }
    
    public static explicit operator double(CoinAmount amount) {
        return amount.cents;
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

    //TODO: fix this error message, but I think this is the only way 
    public CoinAmount getDivideBy(int denominator) {
        if (cents % denominator != 0)
            throw new ArgumentException("Cant divide " + cents + " cents by " + denominator);
        
        return new CoinAmount(cents / denominator);
    }
}