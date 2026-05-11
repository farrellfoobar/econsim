using System;

namespace EconSim.tst;

public class Scratch
{
    public void run() {
        AccessorsDoNotDoWhatIThoughtWowThatWasDumb();
    }
    
    class Thing()
    {
        public int val
        {
            get { return val;} //get { return val;} causes stack overflow
            set {}
        }

        public Thing(int value) : this() {
            this.val = value;
        }
    }

    public void AccessorsDoNotDoWhatIThoughtWowThatWasDumb() {
        Thing thing = new Thing(100);
        int fuckyou = thing.val;
        
        Console.WriteLine(thing.val);
    }
}