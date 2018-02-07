using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyReplace
{
    public class CompiledMacro
    {
        private INPUT[] _events = null;
        bool origKeyDown = false;

        string _tableSwitchName = null;
       public bool HasTableSwitch { get { return !string.IsNullOrEmpty(_tableSwitchName); } }
        int _tableSwitchIdx = -1;

        DateTime _lastTriggered = DateTime.MinValue;

        public bool OrigKeyDown
        { get { return origKeyDown; } }

        public int TableSwitchIdx //updated by the CompileAll() method.
        {
            get { return _tableSwitchIdx; }
            set { _tableSwitchIdx = value; }
        }

        public string TableSwitchName
        {
            get { return _tableSwitchName; }
            set { _tableSwitchName = value; }
        }

        public double TimeSinceLastRun
        {
            get { return (DateTime.Now - _lastTriggered).TotalMilliseconds; }
        }

        public bool PassOriginalKey { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="origKeyDown"></param>
        /// <param name="tableSwitchName">use string.Empty if no table.</param>
        public CompiledMacro(INPUT[] events, bool origKeyDown, string tableSwitchName, bool passKey)
        {
            this._events = events;
            this.origKeyDown = origKeyDown;
            this._tableSwitchName = tableSwitchName;
            this.PassOriginalKey = passKey;
        }

        public void Execute()
        {
            if (_events.Length > 0)
            {
                //InputSender.DispatchInput(_events); //showtime BOIS
                InputEventDispatcher.EnqueueEvents(_events);
                _lastTriggered = DateTime.Now;
            }
        }
    }
}
