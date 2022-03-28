using Sys = Cosmos.System;
using Cosmos.System;
namespace DuskOS_Boot
{
    public class Kernel : Sys.Kernel
    {
        protected override void BeforeRun()
        {
            DuskOSDev.Kernel.BeforeRun();
        }

        protected override void Run()
        {
            DuskOSDev.Kernel.Run();
        }
    }
}
