/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          System/Users/TimeLimitations.cs
 * PROGRAMMERS:     
 *                  WinMister332/Chris Emberley <cemberley@nerdhub.net>
 *
 */
using System;

namespace DuskOS.System.Users
{
    public class DailyTimeLength
    {
        public TimeSpan Monday { get; set; } = TimeSpan.FromMinutes(15);
        public TimeSpan Tuesday { get; set; } = TimeSpan.FromMinutes(15);
        public TimeSpan Wednesday { get; set; } = TimeSpan.FromMinutes(15);
        public TimeSpan Thursday { get; set; } = TimeSpan.FromMinutes(15);
        public TimeSpan Friday { get; set; } = TimeSpan.FromMinutes(15);
        public TimeSpan Saturday { get; set; } = TimeSpan.FromMinutes(15);
        public TimeSpan Sunday { get; set; } = TimeSpan.FromMinutes(15);
    }

    public class CurfewTimes
    {

    }

    public class ClockSpanRange
    {

    }

    public class ClockSpan
    {
        private int hour = 0;
        private int minute = 0;
        private int second = 0;
        private bool is12HourTime = true;

        public ClockSpan(int hour, int minute, bool is12HourTime = true)
        {
            if (hour < 1)
                hour = 1;
            if (hour > 12)
                hour = 12;

            if (minute < 0)
                minute = 0;
            if (minute > 59)
                minute = 59;

            this.hour = hour;
            this.minute = minute;
            this.is12HourTime = is12HourTime;
        }

        public ClockSpan(int hour, int minute, int second, bool is12HourTime = true)
        {
            if (hour < 1)
                hour = 1;
            if (hour > 12)
                hour = 12;

            if (minute < 0)
                minute = 0;
            if (minute > 59)
                minute = 59;

            if (second < 0)
                second = 0;
            if (second > 59)
                second = 59;

            this.hour = hour;
            this.minute = minute;
            this.second = second;
            this.is12HourTime = is12HourTime;
        }

        public ClockSpan(string h, bool is12HourTime = true)
        {
            if (!(h.Contains(":")))
                return;
            else
            {
                var x = Utilities.Utilities.Split(h, ":");
                var hr = x[0];
                var min = x[1];
                var sec = (x.Length == 3) ? x[2] : "00";

                var hour = int.Parse(hr);
                var minute = int.Parse(min);
                var second = int.Parse(sec);

                if (hour < 1)
                    hour = 1;
                if (hour > ((is12HourTime) ? 12 : 23))
                    hour = ((is12HourTime) ? 12 : 23);

                if (minute < 0)
                    minute = 0;
                if (minute > 59)
                    minute = 59;

                if (second < 0)
                    second = 0;
                if (second > 59)
                    second = 59;

                this.hour = hour;
                this.minute = minute;
                this.second = second;
                this.is12HourTime = is12HourTime;
            }
        }

        public void IncreaseHour()
        {
            if (is12HourTime)
            {
                hour += 1;
                if (hour >= 12)
                    hour -= 11;
            }
            else
            {
                hour += 1;
                if (hour >= 23)
                    hour -= 22;
            }
        }
        
        public void IncreaseMinute()
        {
            minute += 1;
            if (minute >= 59)
            {
                minute -= 58;
                IncreaseHour();
            }
        }

        public void IncreaseSecond()
        {
            second += 1;
            if (second >= 59)
            {
                second -= 58;
                IncreaseMinute();
            }
        }

        public int Hour => hour;
        public int Minute => minute;
        public int Second => second;

        public string ToString(bool includeSecond)
        {
            var s = (includeSecond) ? $":{second}" : "";
            var v = $"{hour}:{minute}{s}";
            return v;
        }

        public override string ToString()
            => ToString(true);
    }
}
