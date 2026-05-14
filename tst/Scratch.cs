using System;

namespace EconSim.tst;

public class Scratch
{
    public void Run() {
        AccessorsDoNotDoWhatIThoughtWowThatWasDumb();
    }
    
    class Thing()
    {
        public int Val
        {
            get { return Val;} //get { return val;} causes stack overflow
            set {}
        }

        public Thing(int value) : this() {
            this.Val = value;
        }
    }

    public void AccessorsDoNotDoWhatIThoughtWowThatWasDumb() {
        Thing thing = new Thing(100);
        int fuckyou = thing.Val;
        
        Console.WriteLine(thing.Val);
    }
}