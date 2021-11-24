using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BernulliGraphics
{
    class Distribuzione

    {
        private Dictionary<String, SortedDictionary<Tuple<double, double>, int>> distrN;
        private Dictionary<string, SortedDictionary<String, int>> distrS;
        double intervall = 10;

        public Distribuzione()
        {
            distrN = new Dictionary<String, SortedDictionary<Tuple<double, double>, int>>();
            distrS = new Dictionary<String, SortedDictionary<String, int>>();

        }
        //intervall standard 10
        public void addAttributNDef(String s, double i)
        {
            addAttributeN(s, i, intervall);
        }
        public void addAttributeN(String s, double value, double inte)
        {
            SortedDictionary<Tuple<double, double>, int> actualdistr;
            double min, max;
            int i = 1;
            min = value - (value % inte);
            max = value + (inte - (value % inte));
            Tuple<double, double> tmp = new Tuple<double, double>(min, max);

            if (!distrN.TryGetValue(s, out actualdistr))
            {
                actualdistr = new SortedDictionary<Tuple<double, double>, int>();
                actualdistr.Add(tmp, 1);
                distrN.Add(s, actualdistr);

            }
            else
            {
                if (!actualdistr.TryGetValue(tmp, out i)) actualdistr.Add(tmp, 1); 
                else { i++; actualdistr.Remove(tmp); actualdistr.Add(tmp, i); }
                distrN.Remove(s); 
                distrN.Add(s, actualdistr);
            }
        }


        private void addAttributeS(string key, String value)
        {
            SortedDictionary<String, int> actualdistrS;
            int i=1;
            if (!distrS.TryGetValue(key, out actualdistrS))
            {
               
                actualdistrS = new SortedDictionary<String, int>();
                actualdistrS.Add(value, 1);
                distrS.Add(key, actualdistrS);

            }
            else
            {
                if (!actualdistrS.TryGetValue(value, out i)) actualdistrS.Add(value, 1);
                else { i++; actualdistrS.Remove(value); actualdistrS.Add(value, i); }
                distrS.Remove(key);
                distrS.Add(key, actualdistrS);
            }

        }

        public void addElemento(ElementoDisribuzione e, double inter)
        {
            foreach (var item in e.getVariabili()) {
                if (item.Value.Item2 == typeof(string))
                this.addAttributeS(item.Key, (String)item.Value.Item1);
                else
                this.addAttributeN(item.Key, (double)item.Value.Item1, inter);
            }


        }

      

        public void addElementoDef(ElementoDisribuzione e)
        {
            addElemento(e, intervall);
        }



        public bool getdistribuzioneN(string s, out SortedDictionary<Tuple<double, double>, int> req)
        {
     
            if (distrN.TryGetValue(s, out req)) return true;
            else return false;

        }
        public bool getdistribuzioneS(string s, out SortedDictionary<String, int> req)
        {

            if (distrS.TryGetValue(s, out req)) return true;
            else return false;

        }



        public string[,] getbivariantmatrix(List<string> bivariante, Dictionary<int, ElementoDisribuzione>.ValueCollection values, out int numeroRighe, out int numeroColonnne)
        { string[] variabili = bivariante.ToArray();
            SortedDictionary<Tuple<double, double>, int> columns;
            SortedDictionary<Tuple<Double, double>, int> rows;
            int i, j = 0;

            distrN.TryGetValue(variabili[0], out columns);
            distrN.TryGetValue(variabili[1], out rows);
            //mi ricavo gli intervalli
            double intervalloRow = 0;
            while (intervalloRow == 0) { intervalloRow = rows.ElementAt(j).Key.Item2 - rows.ElementAt(j).Key.Item1; j++; }

            j = 0;
            double intervalloCol = 0;
            while (intervalloCol == 0) {intervalloCol = columns.ElementAt(j).Key.Item2 - columns.ElementAt(j).Key.Item1; j++; }

             numeroColonnne = columns.Count();
             numeroRighe = rows.Count();

            String[,] outputM = new String[numeroRighe+1,numeroColonnne+1];
            int[,] outputMV = new int[numeroRighe, numeroColonnne];
            //inizilizo la matrice outputMatriceValori
            for (i = 0; i < numeroRighe; i++)
                for (j = 0; j < numeroColonnne; j++) outputMV[i,j] = 0;

            i = 1;
            j = 1;
            outputM[0, 0] = " ";

            // riempo gli estremi della tabella
            foreach(var column in columns.Keys)
            {
                outputM[0, i] = column.ToString();
                i++;


            }
            foreach (var row in rows.Keys)
                {
                 outputM[j, 0] = row.ToString();
                 j++;


                }

            //cerco l'intervallo giusto per entrambi e li carico
            foreach (var elm in values)
            {
                bool trov0 = false; 
                bool trov1 = false;
                Tuple<Object, Type> tmpv0;
                Tuple<Object, Type> tmpv1;
                elm.getVariable(variabili[0], out tmpv0);
                elm.getVariable(variabili[1], out tmpv1);
                 i = 0;
                 j = 0 ;
                while (!trov0&& i<numeroColonnne)
                {
                    if (((double)tmpv0.Item1 - columns.Keys.ElementAt(i).Item1) <= intervalloCol) trov0 = true;
                    else i++;
                }

                while (!trov1&&j<numeroRighe)
                {
                    if ((double)tmpv1.Item1 - rows.Keys.ElementAt(j).Item1 <= intervalloCol) trov1 = true;
                    else j++;
                }

                if(trov0 && trov1)outputMV[j, i] += 1;
               
                

            }
            for (i = 1; i <= numeroColonnne; i++)
                for (j = 1; j <= numeroRighe; j++) outputM[j, i] = outputMV[j - 1, i - 1].ToString();
            return outputM;

        }
    }  
    }

