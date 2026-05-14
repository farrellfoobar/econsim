using System;

namespace EconSim.data;

public class SimpleLogger
{
    public static bool IsDebug = true;
    private static String prepend = "*** LOG ***:\t\t";
    private static String debugPrepend = "*** DEBUG LOG ***:\t";

    public static void Log(String message) {
        Console.WriteLine(prepend + message);
    }
    
    public static void Debug(String message) {
        if (IsDebug) {
            Console.WriteLine(debugPrepend+ message);
        }
    }
    
    public static void Debug(String message, bool condition) {
        if (IsDebug && condition) {
            Console.WriteLine(debugPrepend+message);
        }
    }
}