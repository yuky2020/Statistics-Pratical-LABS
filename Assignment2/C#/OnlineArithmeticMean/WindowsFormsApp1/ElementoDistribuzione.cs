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
       private  Dictionary<String,Double> variabili;
    public ElementoDisribuzione(String nome)
        {
            this.name = nome;
            this.variabili = new Dictionary<string, double>();

        }

    public void setVariable(String name,double value) {
            this.variabili.Add(name, value);
        }

    public bool getVariable(String name,out double ret)
        { 
            if (this.variabili.TryGetValue(name, out ret)) return true;
            else return false;
        }
    public Dictionary<String, Double> getVariabili() {
            return this.variabili;
        }

    }

}
