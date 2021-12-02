using System;
using System.Collections.Generic;
using System.Text;
using XSharp;
using XSharp.Assembler;

namespace DuskOS.System.Drivers.Executable.DuskCOM
{
    public class RawExecution
    {
        public static void Execute(string file)
        {

        }

        public static void ExecuteInterp(string code)
        {
            XS.LiteralCode(code);
        }
    }
}
