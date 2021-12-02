using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using IL2CPU.API.Attribs;
using XSharp;
using XSharp.Assembler;

namespace DuskOS.System.Drivers.Executable.DuskCOM
{
    //Dusk COm File or DCOM
    //TODO: After fully developed might replace some stuff, it has a long way to go.
    public unsafe class PlainBinaryProgram
    {
        public static uint ProgramAddress;

        /// <summary>
        /// Loads a program
        /// </summary>
        /// <param name="pfile">file path</param>
        public static void LoadProgram(string pfile)
        {
            byte[] bytes = File.ReadAllBytes(pfile); 
            LoadProgram(bytes);
        }

        public static void LoadProgram(byte[] code)
        {
            byte* data = (byte*)Cosmos.Core.Memory.Old.Heap.MemAlloc((uint)code.Length);
            
            ProgramAddress = (uint)&data[0];

            for (int i = 0; i < code.Length; i++)
            {
                data[i] = code[i];
            }

            Caller caller = new Caller();
            caller.CallCode((uint)&data[0]);
        }

        public class Caller
        {
            [PlugMethod(Assembler = typeof(CallerPlug))]
            public void CallCode(uint address) { }
        }

        [Plug(Target = typeof(Caller))]
        public class CallerPlug : AssemblerMethod
        {
            public override void AssembleNew(Assembler aAssembler, object aMethodInfo)
            {
                XS.Set(XSRegisters.EAX, XSRegisters.EBP, sourceDisplacement: 8);
                XS.Call(XSRegisters.EAX);
            }
        }
    }
}
