/*
* SOURCE:          Aura Operating System Development
*
* PROJECT:         Dusk Operating System Development
* CONTENT          System/Shell/Shell.cs
* PROGRAMMERS:
*                  Valentin Charbonnier <valentinbreiz@gmail.com>
* EDITORS: 
*                  ProfessorDJ/John Welsh <djlw78@gmail.com>
*
*/

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
