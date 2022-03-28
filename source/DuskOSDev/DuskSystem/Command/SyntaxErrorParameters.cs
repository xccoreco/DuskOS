using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Command
{
    //FOR COSMOS PORT, REMOVE EVENTARGS
    public class SyntaxErrorParameters : EventArgs
    {
        public string CommandNamePassed { get; internal set; }
        internal string[] GetLegendArr() => new string[]
        {
            "Legend:",
            "<>: Required Argument",
            "[]: Optional Argument",
            "|: One OR the other"
        };

        internal int GetLongestLine(string[] lines)
        {
            string longestLine = "";
            for (int i = 0; i < lines.Length; i++)
            {
                var x = lines[i];
                for (int j = 0; j < lines.Length; j++)
                {
                    var y = lines[j];
                    if (y.Length >= x.Length)
                        longestLine = y;
                    else if (x.Length >= x.Length)
                        longestLine = x;
                    else
                        continue;
                }
            }
            return longestLine.Length;
        }

        public string GetLegend(out int length)
        {
            var n = GetLegendArr();
            length = GetLongestLine(n);
            List<string> sL = new List<string>();
            foreach (string s in n)
            {
                sL.Add(s);
            }
            StringBuilder builder = new StringBuilder();
            var l = new string('-', length);
            var lxLen = "=|LEGEND|=".Length;
            var xlnLen = l.Length - lxLen;
            var xLen = lxLen / 2;
            builder.Append($"{new string('-', xLen)}=|LEGEND|={new string('-', xLen)}");
            foreach (string s in sL)
            {
                builder.AppendLine(s);
            }
            builder.Append(l);
            return builder.ToString();
        }
    }
}
