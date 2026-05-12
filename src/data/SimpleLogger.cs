using System;

namespace EconSim.data;

public class SimpleLogger
{
    public static bool isDebug = true;
    private static String prepend = "*** DEBUG LOG ***: ";
    
    public static void debug(String message) {
        if (isDebug) {
            Console.WriteLine(prepend+ message);
        }
    }
    
    public static void debug(String message, bool condition) {
        if (isDebug && condition) {
            Console.WriteLine(prepend+message);
        }
    }
}