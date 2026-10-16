using System;
using EconSim.logic;

namespace EconSim.tst;

public class TurnsPerSecondSanityTest
{
    public static void Run()
    {
        if (Root2dScene.GODOT_PHYSICS_TICKS_PER_SECOND % TurnAndTimeManager.BASE_TURNS_IN_SECOND != 0)
            throw new ArgumentException("Turns in second % 60 (godot ticks per second) does not equal 0");
        
        if (Root2dScene.GODOT_PHYSICS_TICKS_PER_SECOND % TurnAndTimeManager.BASE_TURNS_IN_SECOND * 2 != 0)
            throw new ArgumentException("Turns in second times two % 60 (godot ticks per second) does not equal 0 (times two required for fast forward speed)");
    }
}