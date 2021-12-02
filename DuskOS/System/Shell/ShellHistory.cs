using System;
using System.Collections.Generic;
using System.Text;

namespace DuskOS.System.Shell
{
    public class ShellHistory
    {
        private int _stackSize;

        public ShellHistory(int stackSize)
        {
            _stackSize = stackSize;
        }

        private List<string> history = new List<string>();

        public void AddToHistory(string v)
        {
            if (v.Trim() == "") return;

            while (history.Count >= _stackSize)
            {
                history.RemoveAt(0);
            }

            history.Add(v);
        }

        public string GetHistory(int index)
        {
            int i = history.Count - index;
            //t.Terminal.WriteLine(i.ToString());

            //i = Math.Min(i, 0);
            //t.Terminal.WriteLine(i.ToString());

            return history[i];
        }

        public int Max() => history.Count;
    }
}
