using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BernulliGraphics
{    
    public class Strade
    {
        public List<PointF> path = new List<PointF>();
        public List<double> values = new List<double>();

        //Dalla lista di valori passo ai punti ;
        public Strade(List<double> values)
        {
            this.values = values;
            double sqrt1n = (double)1 / values.Count;
                Math.Sqrt(sqrt1n);
            double jump=0;

            for (int i=0; i < values.Count; i++)
            {
                
                    jump += (sqrt1n)*values[i];
               
                path.Add(new PointF(i + 1, (float) jump));
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
