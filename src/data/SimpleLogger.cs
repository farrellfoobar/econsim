using System;

namespace EconSim.data;

public class SimpleLogger
{
    public static bool isDebug = true;
    private static String prepend = "*** LOG ***:\t\t";
    private static String debugPrepend = "*** DEBUG LOG ***:\t";

    public static void log(String message) {
        Console.WriteLine(prepend + message);
    }
    
    public static void debug(String message) {
        if (isDebug) {
            Console.WriteLine(debugPrepend+ message);
        }
    }
    
    public static void debug(String message, bool condition) {
        if (isDebug && condition) {
            Console.WriteLine(debugPrepend+message);
        }
    }
}