using System;

namespace EconSim.tst;

public class Util
{
    public static void Assert(bool assertion, string exceptionMessage) {
        if (!assertion) {
            throw new Exception(exceptionMessage);
        }
    }
}