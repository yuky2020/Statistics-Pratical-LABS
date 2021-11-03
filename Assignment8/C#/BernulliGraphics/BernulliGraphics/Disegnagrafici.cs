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

        private int SCALE = 10;

        private Random R = new Random();

        public List<Strade> viewPortPaths;
        public List<Strade> viewPortAbsolute;

        private Rectangle viewPort;
        Matrix m1;
        BernulliPathfinder bernulli;

        public DisegnaGrafici(int m, int n, int j, Graphics graphics, double p, double epsilon, Rectangle vPort, int dinamicleft, int dinamictop, int contgw, int contgh, TextBox boxassfreq, TextBox boxrelfreq, bool is_abs)
        {
            this.g2 = graphics;
            this.viewPort = vPort;
            this.viewPort.X = dinamicleft;
            this.viewPort.Y = dinamictop;
            this.viewPort.Width = contgw;
            this.viewPort.Height = contgh;
            this.m1 = new Matrix();
            g2.Clear(Color.Transparent);
            g2.FillRectangle(Brushes.Transparent, this.viewPort);
            g2.DrawRectangle(new Pen(Color.Black), this.viewPort);
            //genero la marice per le trasformazioni
            m1.Reset();
            m1.Translate(-0, -(int)0, MatrixOrder.Append);
            m1.Scale((int)(viewPort.Width / (n - 0)), (int)(-viewPort.Height / (1 - 0)), MatrixOrder.Append);
            m1.Translate(viewPort.Left, viewPort.Top + viewPort.Height, MatrixOrder.Append);

            //genero le "strade"
            bernulli = new BernulliPathfinder(n, m, p);

            //disegno i vari path convertendoli per il viewport
            if (is_abs)
                disegnaABSPaths(fromPathstoAbsViewport(bernulli.paths, viewPort,n));
            else
            {
                disegnaPaths(fromPathstoViewport(bernulli.paths, viewPort));

                disegnaHistogramma(viewPort, getDistribution(bernulli.paths, m / SCALE, j), n, j);
                disegnaHistogramma(viewPort, getDistribution(bernulli.paths, m / SCALE, n), n, n);
            }

            getAbsoluteFrequencies(bernulli.paths, n, m, p, epsilon, boxassfreq, boxrelfreq);


        }

        private void disegnaABSPaths(List<List<PointF>> viewPortABSPaths)
        {
            for (int i = 0; i < viewPortABSPaths.Count; i++)
            {
                Pen pen = new Pen(Color.FromArgb(R.Next(0, 255), R.Next(0, 255), R.Next(0, 255)));


                for (int j = 0; j < viewPortABSPaths[i].Count - 1; j++)
                {
                    g2.DrawLine(pen, (float)viewPortABSPaths[i][j].X, (float)viewPortABSPaths[i][j].Y,
                        (float)viewPortABSPaths[i][j + 1].X, (float)viewPortABSPaths[i][j + 1].Y);
                }
            }
        }

        public List<List<PointF>> fromPathstoAbsViewport(List<Strade> paths, Rectangle viewPort,int n)
        {
            List<List<PointF>> viewPortAbsPaths = new List<List<PointF>>();
           
            float tmpy = 0;
            float tmpx = 0;

            foreach (Strade path in paths)
            {
                tmpy = 0;
                tmpx = 0;
                List<PointF> abtmp = new List<PointF>();
                foreach (int p in path.values)
                {
                    if (p == 1) tmpy++;
                    tmpx++;
                    //only for graphic it rapidly
                    abtmp.Add(new PointF(tmpx, (tmpy / n)));

                }
                    PointF[] viewPortArrayPath = abtmp.ToArray();

                    this.m1.TransformPoints(viewPortArrayPath);

                    viewPortAbsPaths.Add(viewPortArrayPath.ToList());
                }
            

            return viewPortAbsPaths;
        }

        public void getAbsoluteFrequencies(List<Strade> paths, int n, int m, double p, double epsilon, TextBox boxassfreq, TextBox boxrelfreq)
        {
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
                x = (int)(this.viewPort.Left + this.viewPort.Width * (j) / n);
                y = (int)(viewPort.Top + viewPort.Height * ((100 - intervals[i].UpperBound * 100) / 100));


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

            for (int i = 0; i < noIntervals; i++)
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
