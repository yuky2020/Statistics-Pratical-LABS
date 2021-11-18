using BernulliChart;
using BernulliGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BernulliGraphics
{
    public class BernulliPathfinder: Pathfinder
    {
       

        int m;      //number of paths
        int n;      //number of points 
        double p;   //probability
        public List<Strade> paths = new List<Strade>();
        private Random R;

        public BernulliPathfinder(int n, int m, double p)
        {
            this.m = m;
            this.n = n;
            this.p = p;

            this.R = new Random();

            for (int i=0; i < m; i++)
            {
                paths.Add(new Strade(createBernulliList()));
            }
            
        

        }

        private int bernoulli_Result(double p,int n)
        {
           double random_outcome = R.NextDouble();

            if (random_outcome <= p/n) return 1;
            else return 0;
        }

        private List<double> createBernulliList()
        {
            List<double> bernoulli = new List<double>();

            for (int i = 0; i < n; i++)
            {
                bernoulli.Add(bernoulli_Result(p, n));
            }

            return bernoulli;
        }
       public List<Strade> Get_paths()
        {
            return this.paths;
        }



    }
}
