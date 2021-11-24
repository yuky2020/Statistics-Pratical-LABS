using BernulliGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BernulliGraphics
{
    public class RademacherPathfinder : Pathfinder
    {
       

        int m;      //number of paths
        int n;      //number of points 
        double p;   //probability
        public List<Strade> paths = new List<Strade>();
        private Random R;

        public RademacherPathfinder(int n, int m)
        {
            this.m = n;
            this.n = m;
            this.p = 0.5;

            this.R = new Random();

            for (int i=0; i < m; i++)
            {
                paths.Add(new Strade(createRademacherList()));
            }
            
        

        }

        private int rademacher_Result(double p)
        {
           double random_outcome = R.NextDouble();

            if (random_outcome < p) return 1;

            else if (random_outcome == p) return 0;
                return -1;
        }

        private List<double> createRademacherList()
        {
            List<double> rademacher = new List<double>();

            for (int i = 0; i < n; i++)
            {
                rademacher.Add(rademacher_Result(p));
            }

            return rademacher;
        }
        
         public List<Strade> Get_paths()
        {
            return this.paths;
        }

    }
}
