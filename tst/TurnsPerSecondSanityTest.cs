using System;
using EconSim.logic;

namespace EconSim.tst;

public class TurnsPerSecondSanityTest
{
    public static void Run()
    {
        if (Root2dScene.GODOT_PHYSICS_TICKS_PER_SECOND % TurnAndTimeManager.TurnsInSecond != 0)
            throw new ArgumentException("Turns in second % 60 (godot ticks per second) does not equal 0");
    }
}