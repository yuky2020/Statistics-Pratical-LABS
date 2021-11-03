using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BernulliChart
{    
    public class Strade
    {
        public List<PointF> path = new List<PointF>();
        public List<int> values = new List<int>();

        //Dalla lista di valori passo ai punti ;
        public Strade(List<int> values)
        {
            this.values = values;
            double mean = 0;

            for (int i=0; i < values.Count; i++)
            {
                if (i == 0)
                {
                    mean = values[0];
                }
                else
                {
                    mean += (values[i] - mean) / (double)(i + 1);
                }

                path.Add(new PointF(i + 1, (float) mean));
            }
        }

        public Strade(List<PointF> points) 
        {
            path = points;
        }

        public List<double> getXs()
        {
            List<double> x_coordinates = new List<double>();

            foreach (PointF p in this.getPath())
            {
                x_coordinates.Add(p.X);
            }

            return x_coordinates;
        }

        public List<double> getYs()
        {
            List<double> y_coordinates = new List<double>();

            foreach (PointF p in this.getPath())
            {
                y_coordinates.Add(p.Y);
            }

            return y_coordinates;
        }

        public List<PointF> getPath()
        {
            return this.path;
        }

        public void setPath(List<PointF> path)
        {
            this.path = path;
        }
    }
}
