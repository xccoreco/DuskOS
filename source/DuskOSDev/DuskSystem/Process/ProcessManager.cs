using DuskOSDev.DuskSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Process
{
    public class ProcessManager
    {
        internal static RunMode OSRunMode { get; set; } = RunMode.TEXT;
        internal static ProcessManager INSTANCE = new ProcessManager();

        private List<Process> processes = null;
        private List<Process> runningProcesses = null;

        public ProcessManager() 
        {
            processes = new List<Process>();
            runningProcesses = new List<Process>();
        }

        public Process[] GetProcesses()
            => processes.ToArray();

        public Process[] GetRunningProcesses()
            => runningProcesses.ToArray();

        public bool IsPIDInUse(int PID)
        {
            foreach (Process p in GetRunningProcesses())
            {
                if (p.GetProcessID().Equals(PID))
                    return true;
                else continue;
            }
            return false;
        }

        public Process GetInstalledProcess(string name)
        {
            foreach (Process p in GetProcesses())
            {
                if (p.GetProcessName().EqualsLowerCase(name))
                    return p;
                else continue;
            }
            return null;
        }

        public bool IsProcessRunning(Process p)
            => runningProcesses.Contains(p);

        public void Register(Process p)
        {
            if (!processes.Contains(p))
                processes.Add(p);
        }
    }
}
