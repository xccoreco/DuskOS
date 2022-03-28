using DuskOSDev.DuskSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Process
{
    public class Process : IEventFireable
    {
        private string processName;
        private ushort processID;
        private bool canStart;
        private ProcessType processType;
        private RunMode requiredRunMode;

        public event Action Started;
        public event Action Closing;
        public event Action Killed;
        public event Action Update;

        //public bool Initialized { get; protected set; }
        //public bool IsRunning { get; private set; }

        public Process()
        {
            Started += () => OnStart();
            Closing += () => OnClosing();
            Killed += () => OnKilled();
            Update += () => OnUpdate();
        }

        public virtual void OnStart() { }
        public virtual void OnClosing() { }
        public virtual void OnKilled() { }
        public virtual void OnUpdate() { }

        public string GetProcessName() => processName;
        public void SetProcessName(string name) { processName = name; }

        public ushort GetProcessID() => processID;
        public void SetProcessID(ushort id) { processID = id; }


        public bool IsProcessRunning()
            //=> Kernel.ProcessManager.IsProcessRunning(this);
            => ProcessManager.INSTANCE.IsProcessRunning(this);
        //public bool IsRespondng() => isResponding;

        public ProcessType GetProcessType() => processType;

        public void SetProcessType(ProcessType type) { processType = type; }

        public RunMode GetRequiredRunMode() => requiredRunMode;

        protected void SetRequiredRunMode(RunMode mode) { requiredRunMode = mode; }

        internal void SetCanStart(bool canStart)
            => this.canStart = canStart;

        //internal void SetIsResponding(bool isResponding)
            //=> this.isResponding = isResponding;

        internal void SetPID(short id)
            => processID = (ushort)id;

        internal void HandleStart()
        {
            //Assign a PID.
            AssignPID();
            FireEvent("Started");
            
        }

        /*
         * Maybe have it start at 127 (rand next)
         * becuase we can use 1-127 for system processes 
         * and 0 can be the kernel itself doesn't even appear as process just an idea.
         * 
         * We also need a system in here that runs into blue screen of death someday
         * for when a user shuts down a system process this can be partly real and partly fake.
         * It would make more sense to just restart it because threading would be a thing for a while.
         */
        private void AssignPID()
        {
            Utilities.Random rand = new Utilities.Random();
            var x = rand.Next(100, short.MaxValue);

            if (!ProcessManager.INSTANCE.IsPIDInUse(x))
            //if (!Kernel.ProcessManager.IsPIDInUse(x))
                processID = (ushort)x;
            else
                AssignPID();
        }

        private void FireEvent(string eventName)
        {
            if (eventName.Equals(nameof(Update)))
                Update();
            else if (eventName.Equals(nameof(Started)))
                Started();
            else if (eventName.Equals(nameof(Closing)))
                Closing();
            else if (eventName.Equals(nameof(Killed)))
                Killed();
            else
                throw new Exception("That event does not exist.");
        }

        /*
         * Might be easier to do it this way
         * Unsure though what goal was with string names.
         * --- UNUSED ---
         */
        private void FireEvent(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.Update:
                    Update();
                    break;
                case EventType.Started:
                    Started();
                    break;
                case EventType.Closing:
                    Closing();
                    break;
                case EventType.Killed:
                    Killed();
                    break;
            }

        }

        void IEventFireable.FireEvent(string eventName)
            => FireEvent(eventName);
    }

    public enum EventType
    {
        Update,
        Started,
        Closing,
        Killed
    }

    public enum ProcessType
    {
        KERNEL = 0,
        DRIVER = 1,
        SHELL = 2,
        GENERAL = 3,
        RECOVERY = -1
    }

    public enum ProcessRunLevel
    {
        SYSTEM = 2,
        ADMINISTRATOR = 1, 
        STANDARD = 0
    }
}
