using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class MediaCalOnline
    {

        Dictionary<String, double> medieAritmetica;
        Dictionary<String, int> numeroElementi;


        public MediaCalOnline()
        {
            medieAritmetica = new Dictionary<string, double>();
            numeroElementi = new Dictionary<String, int>();

        }
        public void addAttribute(String nome, double value)
        {
            double tmp;
            int i;
            if (medieAritmetica.ContainsKey(nome))
            {
                medieAritmetica.TryGetValue(nome, out tmp);
                numeroElementi.TryGetValue(nome, out i);
                i++;
                tmp = tmp + (value - tmp) / i;
                numeroElementi.Remove(nome);
                medieAritmetica.Remove(nome);
                medieAritmetica.Add(nome, tmp);
                numeroElementi.Add(nome, i);



            }
            else
            {
                medieAritmetica.Add(nome, value);
                numeroElementi.Add(nome, 1);
            }
        }
            public bool getMedia(String name,out double i)
            {
            if (medieAritmetica.TryGetValue(name, out i)) return true;
            else return false;
            }
        public void addElemento(ElementoDisribuzione e)
        {
            foreach (var item in e.getVariabili())
                addAttribute(item.Key, item.Value);

        }
        


    }
}

