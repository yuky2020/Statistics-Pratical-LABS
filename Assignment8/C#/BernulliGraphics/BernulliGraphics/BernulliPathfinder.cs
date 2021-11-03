using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BernulliChart
{
    public class BernulliPathfinder
    {
       

        int m;      //number of paths
        int n;      //number of points 
        double p;   //probability
        public List<Strade> paths = new List<Strade>();
        private Random R;

        public BernulliPathfinder(int n, int m, double p)
        {
            this.m = n;
            this.n = m;
            this.p = p;

            this.R = new Random();

            for (int i=0; i < m; i++)
            {
                paths.Add(new Strade(createBernulliList()));
            }
            
        

        }

        private int bernoulli_Result(double p)
        {
           double random_outcome = R.NextDouble();

            if (random_outcome <= p) return 1;
            else return 0;
        }

        private List<int> createBernulliList()
        {
            List<int> bernoulli = new List<int>();

            for (int i = 0; i < n; i++)
            {
                bernoulli.Add(bernoulli_Result(p));
            }

            return bernoulli;
        }
        

    }
}
