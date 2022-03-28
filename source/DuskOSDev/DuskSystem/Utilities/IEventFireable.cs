using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Utilities
{
    public interface IEventFireable
    {
        void FireEvent(string eventName);
    }
}
