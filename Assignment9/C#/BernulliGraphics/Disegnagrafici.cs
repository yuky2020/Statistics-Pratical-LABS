using BernulliGraphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace BernulliChart
{
    public class DisegnaGrafici
    {
        public Bitmap bitmap;
        public Graphics g2;
        public PictureBox pictureBox;

        private int SCALE = 4;

        private Random R = new Random();

        public List<Strade> viewPortPaths;
        public List<Strade> viewPortAbsolute;

        private Rectangle viewPort;
        int n;
        Matrix m1;
        Matrix m2;
        Pathfinder distrubution;

        public DisegnaGrafici(int m, int n, int j, Graphics graphics, double epsilon, Rectangle vPort, int dinamicleft, int dinamictop, int contgw, int contgh, TextBox boxassfreq, TextBox boxrelfreq, bool is_abs,bool is_norml)
        {
            this.g2 = graphics;
            this.viewPort = vPort;
            this.viewPort.X = dinamicleft;
            this.viewPort.Y = dinamictop;
            this.viewPort.Width = contgw;
            this.viewPort.Height = contgh;
            this.n = n;
            this.m1 = new Matrix();
            this.m2 = new Matrix();
            g2.Clear(Color.Transparent);
            g2.FillRectangle(Brushes.Transparent, this.viewPort);
            g2.DrawRectangle(new Pen(Color.Black), this.viewPort);
            //genero la marice per le trasformazioni
            m1.Reset();
            m1.Translate((float)-0, -(float)0, MatrixOrder.Append);
            m1.Scale((float)(viewPort.Width /m ), (float)(-viewPort.Height /1), MatrixOrder.Append);
            m1.Translate(viewPort.Left, viewPort.Top + viewPort.Height, MatrixOrder.Append);

            //Matrix for the cdf
            m2.Reset();
            m2.Translate((float)-0, -(float)0, MatrixOrder.Append);
            m2.Scale((float)(viewPort.Width /14), (float)(-viewPort.Height / 1), MatrixOrder.Append);
            m2.Translate(viewPort.Left, viewPort.Top + viewPort.Height, MatrixOrder.Append);

            //genero le "strade"
            if (is_norml == false)
                distrubution = new RademacherPathfinder(n, m);
            else distrubution = new NormalPathfinder(n, m);
            //disegno i vari path convertendoli per il viewport
            if (is_abs)
                disegnaCDFPaths(CDFtoViewport(viewPort,n));
            else
            {
                disegnaPaths(fromPathstoViewport(distrubution.Get_paths(), viewPort));

                disegnaHistogramma(viewPort, getDistribution(distrubution.Get_paths(), m / SCALE, j), n, j);
                disegnaHistogramma(viewPort, getDistribution(distrubution.Get_paths(), m / SCALE, n), n, n);
            }

            getAbsoluteFrequencies(distrubution.Get_paths(), n, m, epsilon, boxassfreq, boxrelfreq);


        }

        private void disegnaCDFPaths(List<PointF> viewPortCDF)
        {

            Pen pen = new Pen(Color.Red);
            Pen pen2 = new Pen(Color.Green);

            //empirical cdf
            for (int j = 1; j < viewPortCDF.Count; j++)
            {
                g2.DrawLine(pen, (float)viewPortCDF[j-1].X, (float)viewPortCDF[j-1].Y,
                    (float)viewPortCDF[j].X, (float)viewPortCDF[j].Y);

                g2.DrawEllipse(pen, new Rectangle((int)viewPortCDF[j].X, (int)viewPortCDF[j].Y, 4,4));
            }

            //theorical cdf
           
                g2.DrawLine(pen2,(viewPort.Left+viewPort.Width), viewPort.Top,
                      viewPort.Left, viewPort.Top+viewPort.Height);
            

        }
                
        public List<PointF> CDFtoViewport(Rectangle viewPort,int n)
        {
            List<PointF> cdfs=new List<PointF>();
            List<int> values=new List<int>();
            for (int i=0; i < n; i++)
            {
              values.Add(R.Next(149, 220));
            }
                       
            float tmpy = 0;
            float tmpx = -1;

            for (int i = 150; i <= 220; i = i + 5)
            {
                tmpy = 0;
                tmpx = tmpx + 1 ;

                foreach (int p in values)
                {
                    if (p < i) tmpy++;

                    //empirical CDF


                }
                cdfs.Add(new PointF(tmpx, tmpy/n));
               

            }
            PointF[] viewPortArraycdf = cdfs.ToArray();
            this.m2.TransformPoints(viewPortArraycdf);
            return(viewPortArraycdf.ToList());
           
        }

        public void getAbsoluteFrequencies(List<Strade> paths, int n, int m, double epsilon, TextBox boxassfreq, TextBox boxrelfreq)
        {
            double p=0.5 ;
            Intervalli p_neighbourhood = new Intervalli(p - epsilon, p + epsilon);

            int absolute_frequency = 0;
            double relative_frequency = 0;

            foreach (Strade path in paths)
            {
                for (int i = 0; i < path.getPath().Count; i++)
                {
                    if (
                        (path.getPath()[i].X == n)
                        &&
                        (path.getPath()[i].Y >= p_neighbourhood.LowerBound)
                        &&
                        (path.getPath()[i].Y < p_neighbourhood.UpperBound)
                        )
                    {
                        absolute_frequency++;
                    }
                }
            }

            relative_frequency = (double)absolute_frequency / (double)m;

            boxassfreq.Text = absolute_frequency.ToString();
            boxrelfreq.Text = relative_frequency * 100 + "%";



        }


        public void disegnaPaths(List<Strade> viewPortPaths)
        {
            for (int i = 0; i < viewPortPaths.Count; i++)
            {
                Pen pen = new Pen(Color.FromArgb(R.Next(0, 255), R.Next(0, 255), R.Next(0, 255)));


                for (int j = 0; j < viewPortPaths[i].getPath().Count - 1; j++)
                {
                    g2.DrawLine(pen, (float)viewPortPaths[i].getPath()[j].X, (float)viewPortPaths[i].getPath()[j].Y,
                        (float)viewPortPaths[i].getPath()[j + 1].X, (float)viewPortPaths[i].getPath()[j + 1].Y);
                }
            }
        }

        public void disegnaHistogramma(Rectangle viewPort, List<Intervalli> intervals, int n, int j)
        {

            for (int i = 0; i < intervals.Count; i++)
            {
                int x, y;
                int width, height;
                // in this case on the fly trasformation is way faster
                x = (int)(this.viewPort.Left + this.viewPort.Width * (j) /n);
                y = (int)(viewPort.Top + viewPort.Height/2 * ((100 - (intervals[i].UpperBound) * 100) / 100));


                width = intervals[i].Counter;
                height = viewPort.Height / intervals.Count;

                Rectangle rectangle = new Rectangle(x, y, width, height);

                g2.DrawRectangle(Pens.Black, rectangle);
                SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(128, 150, 0, 0));
                g2.FillRectangle(semiTransBrush, rectangle);

                g2.FillRectangle(Brushes.Violet, rectangle);
            }
        }

        public List<Intervalli> getDistribution(List<Strade> paths, int noIntervals, int j)
        {


            List<double> frequencies = new List<double>();

            for (int i = 0; i < paths.Count; i++)
            {
                for (int k = 0; k < paths[i].getPath().Count; k++)
                {
                    if (paths[i].getPath()[k].X == j)
                    {
                        frequencies.Add(paths[i].getPath()[k].Y);
                    }
                }
            }

            List<Intervalli> intervals = new List<Intervalli>();

            double intervalLength = 1.0 / (double)noIntervals;

            for (int i= -noIntervals; i < noIntervals; i++)
            {
                intervals.Add(new Intervalli(i * intervalLength, (i + 1) * intervalLength));
            }

            for (int i = 0; i < intervals.Count; i++)
            {
                for (int k = 0; k < frequencies.Count; k++)
                {
                    if ((frequencies[k] >= intervals[i].LowerBound) && (frequencies[k] < intervals[i].UpperBound))
                    {
                        intervals[i].Counter++;
                    }
                }
            }

            return intervals;
        }

        public List<Strade> fromPathstoViewport(List<Strade> paths, Rectangle viewPort)
        {
            List<Strade> viewPortPaths = new List<Strade>();

            foreach (Strade path in paths)
            {

                PointF[] viewPortArrayPath = path.getPath().ToArray();
                for (int i = 0;i < viewPortArrayPath.Length; i++)
                {

                  viewPortArrayPath[i].Y= (viewPortArrayPath[i].Y + 1) / 2;
                    
                }

                this.m1.TransformPoints(viewPortArrayPath);
                viewPortPaths.Add(new Strade(viewPortArrayPath.ToList()));
            }

            return viewPortPaths;
        }




        public Graphics getGrapichs()
        {
            return this.g2;
        }
    }
}
