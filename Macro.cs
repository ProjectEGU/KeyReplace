using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyReplace
{
    [Serializable]
    public partial class Macro
    {
        string _name = "";
        List<Trigger> _triggers = new List<Trigger>();
        public Macro(string name)
        {
            Name = name;
            _triggers = new List<Trigger>();
        }
        /// <summary>
        /// Initializes a blank macro, triggered by something
        /// </summary>
        /// <param name="trigger"></param>
        public Macro(string name, Trigger[] triggers)
        {
            Name = name;
            _triggers = new List<Trigger>(triggers);
        }

        /// <summary>
        /// Initializes a blank macro, triggered by something
        /// </summary>
        /// <param name="trigger"></param>
        public Macro(string name, Trigger[] triggers, InputEvent[] events)
        {
            Name = name;

            _triggers = new List<Trigger>(triggers);
            _events = new List<InputEvent>(events);
        }

        public bool HasTableSwitch = false;
        public string TableSwitchName = "";

        public IEnumerable<InputEvent> Events {  get { return _events.AsEnumerable(); } }
        public IEnumerable<Trigger> Triggers { get { return _triggers.AsEnumerable(); } }

        public int TriggerCount {  get { return _triggers.Count;  } }
        public int EventCount {  get { return _events.Count; } }

        public bool PassOriginalKey = false;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public void AddInputEvent(InputEvent evt)
        {
            
            _events.Add(evt); //Declared in Macro.CompileExecute.cs
        }

        public void RemoveInputEvent(int idx)
        {
            _events.RemoveAt(idx);
        }

        public void ClearInputEvents()
        {
            _events.Clear();
        }
       
        public void AddTrigger(Trigger trig)
        {
            _triggers.Add(trig);
        }

        public void RemoveTrigger(int idx)
        {
            _triggers.RemoveAt(idx);
        }

        public void ClearTriggers()
        {
            _triggers.Clear();
        }

    }

}
