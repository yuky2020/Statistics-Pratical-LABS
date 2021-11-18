using BernulliGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BernulliChart
{
    public class NormalPathfinder: Pathfinder
    {
       

        int m;      //number of paths
        int n;      //number of points 
        double p;   //probability
        public List<Strade> paths = new List<Strade>();
        private Random R;

        public NormalPathfinder(int n, int m)
        {
            this.m = n;
            this.n = m;
            this.p = 0.5;

            this.R = new Random();

            for (int i=0; i < m; i++)
            {
                paths.Add(new Strade(createNormalList()));
            }
            
        

        }

        private double normal_Result(double p)
        {
           double random_outcome = R.NextDouble();
            double normal_distrbAtOut;
            double v = R.NextDouble();
            //create a value between 1 and -1
            random_outcome = random_outcome * 2 - 1;
            //get the standard normal for that point 
            normal_distrbAtOut= Math.Pow(Math.E, (Math.Pow(-random_outcome, 2) / 2))/Math.Sqrt(2*Math.PI) ;
            //then use the other generated random
            if (v < normal_distrbAtOut) return random_outcome;
            else return 0;

        }

        private List<double> createNormalList()
        {
            List<double> normal = new List<double>();

            for (int i = 0; i < n; i++)
            {
               normal.Add(normal_Result(p));
            }

            return normal;
        }


       public List<Strade> Get_paths()
        {
            return this.paths;
        }


    }
}
