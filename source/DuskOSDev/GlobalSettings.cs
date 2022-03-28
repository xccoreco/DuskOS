using Cosmos.System.Graphics;
using DuskOSDev.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev
{
    public class GlobalSettings
    {
        static string teststr = "Test";

        public static void test()
        {
            teststr = teststr.ToSHA256();
        }
        
    }
}
