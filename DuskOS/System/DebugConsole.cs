/*
 * SOURCE:          Aura Operating System Development
 *
 * PROJECT:         Dusk Operating System Development
 * CONTENT          System/DebugConsole.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using System;

namespace DuskOS.System
{
    public static class DebugConsole
    {
        public static void WriteLnOK(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[OK]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text + "\n"); //writeline?
            Kernel.debugger.Send("[OK]: " + text);
        }

        public static void WriteLnInfo(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[INFO]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text + "\n"); //writeline?
            Kernel.debugger.Send("[INFO]: " + text);
        }

        public static void WriteLnWarning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[WARNING]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text + "\n"); //writeline?
            Kernel.debugger.Send("[WARNING]: " + text);
        }

        public static void WriteLnError(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("[ERROR]: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text + "\n"); //writeline?
            Kernel.debugger.Send("[ERROR]: " + text);
        }
    }
}
