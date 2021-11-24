using BernulliGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BernulliGraphics
{
    public class NormalPathfinder: Pathfinder
    {
       

        int m;      //number of paths
        int n;      //number of points 
        double p;   //probability
        public List<Strade> paths = new List<Strade>();
        public List<List<Double>> values = new List<List<Double>>();
        private Random R;

        public NormalPathfinder(int n, int m)
        {
            this.m = n;
            this.n = m;
            this.p = 0.5;

            this.R = new Random();

            for (int i=0; i < m; i++)
            {
                List<Double> list = createNormalList();
                paths.Add(new Strade(list));
                values.Add(list);
            }
            
        

        }

        private bool normal_Result(double p,out double ou)
        {
           double random_outcome =( R.NextDouble()+R.NextDouble());
            double normal_distrbAtOut;
            double v = R.NextDouble();
            //create a value between 1 and -1
            random_outcome = random_outcome  - 1;
            //get the standard normal for that point 
            normal_distrbAtOut= Math.Pow(Math.E, (Math.Pow(-random_outcome, 2) / 2))/Math.Sqrt(2*Math.PI) ;
            //then use the other generated random
            if (v <= normal_distrbAtOut*Math.Sqrt(2*Math.PI))
            {
                ou = random_outcome;
                return true;
            }

            else { ou = 0; return false; }

        }

        private List<double> createNormalList()
        {
            List<double> normal = new List<double>();

            for (int i = 0; i < n; i++)
            {
                double j;
                if (normal_Result(p, out j))
                    normal.Add(j);
                else i--;
            }

            return normal;
        }

       public List<Double> getnormale(int n) 
        {
            List<Double> outlist = new List<double>();
            for (double i = -1.000; i <= 1.000; i = i + 2 / n) 
                outlist.Add(Math.Pow(Math.E, (Math.Pow(-i, 2) / 2)) / Math.Sqrt(2 * Math.PI) );
            return outlist;
        }

       public static double NormalDistribution(double value)
        {
            return (1 / Math.Sqrt(2 * Math.PI)) * Math.Exp(-Math.Pow(value, 2) / 2);
        }


       public List<Strade> Get_paths()
        {
            return this.paths;
        }

         }
}
