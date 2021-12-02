using System;
using System.Collections.Generic;
using System.Text;

namespace DuskOS.System.Shell
{
    public static class Shell
    {
        private static ShellHistory history;

        internal static string GetHistoryItem(int historyIndex)
        {
            return history.GetHistory(historyIndex);
        }

        internal static int GetHistoryMax()
        {
            return history.Max();
        }
    }
}
