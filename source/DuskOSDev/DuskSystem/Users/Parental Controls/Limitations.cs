using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Users.Parental_Controls
{
    public class Limitations
    {
        public sealed class DailyTimeLimits
        {
            private bool enabled = true;
            private List<int> timeLimits;

            public DailyTimeLimits()
            {
                timeLimits = new List<int>(7)
                {
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    -1,
                    -1
                };

            }

            private int GetDay(DayOfWeek day)
                => (int)day;

            public void SetLimitDayOfWeek(DayOfWeek day, int value)
            {
                if (value < 15 && !(value == -1))
                    value = 15;
                if (value > short.MaxValue)
                    value = short.MaxValue;
                timeLimits[GetDay(day)] = value;
            }

            public int GetLimitDayOfWeek(DayOfWeek day)
                => timeLimits[GetDay(day)];
        }

        public sealed class CerfewTimes
        {
           // public List<>
        }
    }
}
