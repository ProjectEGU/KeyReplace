using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyReplace
{
    [Serializable]
    public class MacroPage
    {
        public string name = null;
        public List<Macro> macros = null;

        public MacroPage(string name, List<Macro> macros)
        {
            this.name = name;
            this.macros = macros;
        }

        public override string ToString()
        {
            return this.name;
        }

    }
}
