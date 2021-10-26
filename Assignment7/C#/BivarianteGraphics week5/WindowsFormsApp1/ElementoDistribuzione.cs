using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class ElementoDisribuzione
    {
        private String name;
        private Dictionary<String, Tuple<Object,Type>> variabili;
        public ElementoDisribuzione(String nome)
        {
            this.name = nome;
            this.variabili = new Dictionary<string, Tuple<Object, Type>>();

        }

        public void setVariable(String name, Tuple<Object, Type>s)
        {
            this.variabili.Add(name,s);
        }

        public bool getVariable(String name, out Tuple<Object, Type> ret)
        {
            if (this.variabili.TryGetValue(name, out ret)) return true;
            else return false;
        }
        public Dictionary<String, Tuple<Object, Type>> getVariabili()
        {
            return this.variabili;
        }

    }

}
