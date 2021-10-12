using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Distribuzione

    {
        private Dictionary<String, SortedDictionary<Tuple<double, double>, int>> distr;
        double intervall = 10;

        public Distribuzione()
        {
            distr = new Dictionary<String, SortedDictionary<Tuple<double, double>, int>>();

        }
        //intervall standard 10
        public void addAttributDef(String s, double i)
        {
            addAttribute(s, i, intervall);
        }
        public void addAttribute(String s, double value, double inte)
        {
            SortedDictionary<Tuple<double, double>, int> actualdistr;
            double min, max;
            int i = 1;
            min = value - (value % inte);
            max = value + (inte - (value % inte));
            Tuple<double, double> tmp = new Tuple<double, double>(min, max);

            if (!distr.TryGetValue(s, out actualdistr))
            {
                actualdistr = new SortedDictionary<Tuple<double, double>, int>();
                actualdistr.Add(tmp, 1);
                distr.Add(s, actualdistr);

            }
            else
            {
                if (!actualdistr.TryGetValue(tmp, out i)) actualdistr.Add(tmp, 1); 
                else { i++; actualdistr.Remove(tmp); actualdistr.Add(tmp, i); }
                distr.Remove(s); 
                distr.Add(s, actualdistr);
            }
        }

        public void addElemento(ElementoDisribuzione e, double inter)
        {
            foreach (var item in e.getVariabili()) {
                this.addAttribute(item.Key, item.Value, inter);
            }


        }
        public void addElementoDef(ElementoDisribuzione e)
        {
            addElemento(e, intervall);
        }



        public bool getdistribuzione(string s, out SortedDictionary<Tuple<double, double>, int> req)
        {
     
            if (distr.TryGetValue(s, out req)) return true;
            else return false;

        }
    }  
    }

