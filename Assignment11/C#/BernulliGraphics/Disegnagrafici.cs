using BernulliGraphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace BernulliGraphics
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
        NormalPathfinder distrubution;

        public DisegnaGrafici(int m, int n, int j, Graphics graphics, Rectangle vPort, int dinamicleft, int dinamictop, int contgw, int contgh, bool is_norml, int index13a)
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
            m1.Scale((float)(viewPort.Width / m), (float)(-viewPort.Height / 1), MatrixOrder.Append);
            m1.Translate(viewPort.Left, viewPort.Top + viewPort.Height, MatrixOrder.Append);

            //Matrix for the cdf
            m2.Reset();
            m2.Translate((float)-0, -(float)0, MatrixOrder.Append);
            m2.Scale((float)(viewPort.Width / 14), (float)(-viewPort.Height / 1), MatrixOrder.Append);
            m2.Translate(viewPort.Left, viewPort.Top + viewPort.Height, MatrixOrder.Append);
            distrubution = new NormalPathfinder(n, m);
            //genero le "strade"
            if (is_norml == true)
            {

                //disegno i vari path convertendoli per il viewport                  {
                disegnaPaths(fromPathstoViewport(distrubution.Get_paths(), viewPort));
                disegnaHistogramma(viewPort, getDistribution(distrubution.Get_paths(), m / SCALE, j), n, j);
                disegnaHistogramma(viewPort, getDistribution(distrubution.Get_paths(), m / SCALE, n), n, n);
            }
            else
            {
                disgnaIstogrammi(viewPort, valueToDictionarys(n, m, index13a));

                // getAbsoluteFrequencies(distrubution.Get_paths(), n, m, epsilon, boxassfreq, boxrelfreq);
            }

        }

        private void disgnaIstogrammi(Rectangle viewPort, Tuple<Dictionary<double, int>, Dictionary<decimal, int>, Dictionary<decimal, int>> tuple)
        {
            int i = 0;
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));

            foreach (var v in tuple.Item1)
            {
                int x, y;
                int width, height;
                // in this case on the fly trasformation is way faster
                x = (int)(this.viewPort.Left + 20 * i);
                y = (int)(viewPort.Top + viewPort.Height / 4 - v.Value / 5);


                width = 15;
                height = v.Value / 5;

                Rectangle rectangle = new Rectangle(x, y, width, height);

                g2.DrawRectangle(Pens.Black, rectangle);
                g2.FillRectangle(semiTransBrush, rectangle);

                g2.FillRectangle(Brushes.Cyan, rectangle);
                g2.DrawString(((decimal)v.Key).ToString(), new Font("Calibri", 10.0f,
                                 FontStyle.Regular, GraphicsUnit.Pixel), semiTransBrush, new Point(x, y + v.Value / 10 + 1));
                i++;
            }
            g2.DrawString("Realization of the Normal ", new Font("Calibri", 13.0f,
                               FontStyle.Italic, GraphicsUnit.Pixel), semiTransBrush, new Point(this.viewPort.Left - 140, (viewPort.Top + viewPort.Height / 4)));

            i = 0;

            foreach (var v in tuple.Item2)
            {
                int x, y;
                int width, height;
                // in this case on the fly trasformation is way faster
                x = (int)(this.viewPort.Left + 20 * i);
                y = (int)(viewPort.Top + viewPort.Height / 2 - v.Value / 2);


                width = 15;
                height = v.Value / 2;

                Rectangle rectangle = new Rectangle(x, y, width, height);

                g2.DrawRectangle(Pens.Black, rectangle);
                g2.FillRectangle(semiTransBrush, rectangle);

                g2.FillRectangle(Brushes.Cyan, rectangle);
                g2.DrawString(v.Key.ToString(), new Font("Calibri", 10.0f,
                                 FontStyle.Regular, GraphicsUnit.Pixel), semiTransBrush, new Point(x, y + v.Value / 2));
                i++;
            }
            g2.DrawString("Mean ", new Font("Calibri", 13.0f,
                               FontStyle.Italic, GraphicsUnit.Pixel), semiTransBrush, new Point(this.viewPort.Left - 50, (viewPort.Top + viewPort.Height / 2)));



            i = 0;

            foreach (var v in tuple.Item3)
            {
                int x, y;
                int width, height;
                // in this case on the fly trasformation is way faster
                x = (int)(this.viewPort.Left + 20 * i);
                y = (int)(viewPort.Top + viewPort.Height - 30 - v.Value / 2);


                width = 15;
                height = v.Value / 2;

                Rectangle rectangle = new Rectangle(x, y, width, height);

                g2.DrawRectangle(Pens.Black, rectangle);
                g2.FillRectangle(semiTransBrush, rectangle);

                g2.FillRectangle(Brushes.Cyan, rectangle);
                g2.DrawString(v.Key.ToString(), new Font("Calibri", 10.0f,
                                 FontStyle.Regular, GraphicsUnit.Pixel), semiTransBrush, new Point(x, y + v.Value / 2));
                i++;
            }
            g2.DrawString("variance ", new Font("Calibri", 13.0f,
                               FontStyle.Italic, GraphicsUnit.Pixel), semiTransBrush, new Point(this.viewPort.Left - 50, (viewPort.Top + viewPort.Height - 30)));



        }


        public List<PointF> CDFtoViewport(Rectangle viewPort, int n)
        {
            List<PointF> cdfs = new List<PointF>();
            List<int> values = new List<int>();
            for (int i = 0; i < n; i++)
            {
                values.Add(R.Next(149, 220));
            }

            float tmpy = 0;
            float tmpx = -1;

            for (int i = 150; i <= 220; i = i + 5)
            {
                tmpy = 0;
                tmpx = tmpx + 1;

                foreach (int p in values)
                {
                    if (p < i) tmpy++;

                    //empirical CDF


                }
                cdfs.Add(new PointF(tmpx, tmpy / n));


            }
            PointF[] viewPortArraycdf = cdfs.ToArray();
            this.m2.TransformPoints(viewPortArraycdf);
            return (viewPortArraycdf.ToList());

        }

        public void getAbsoluteFrequencies(List<Strade> paths, int n, int m, double epsilon, TextBox boxassfreq, TextBox boxrelfreq)
        {
            double p = 0.5;
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
                y = (int)(viewPort.Top + viewPort.Height / 2 * ((100 - (intervals[i].UpperBound) * 100) / 100));


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

            for (int i = -noIntervals; i < noIntervals; i++)
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
                for (int i = 0; i < viewPortArrayPath.Length; i++)
                {

                    viewPortArrayPath[i].Y = (viewPortArrayPath[i].Y + 1) / 2;

                }

                this.m1.TransformPoints(viewPortArrayPath);
                viewPortPaths.Add(new Strade(viewPortArrayPath.ToList()));
            }

            return viewPortPaths;
        }


        public Tuple<Dictionary<double, int>, Dictionary<decimal, int>, Dictionary<decimal, int>> valueToDictionarys(int m, int n, int index13a)
        {
            Dictionary<double, List<double>> randomvalues = new Dictionary<double, List<double>>();
            int i = 0;
            int j = 0;


            //calcolation of the  random ditribution
            for (i = 0; i < n; i++)
            {
                List<Double> tmp = new List<double>();
                tmp = distrubution.values[i];
                randomvalues.Add(i, tmp);
            }
            //controllo se è da fare exp squred o squared/ squared 
            if (index13a != 0)
            {
                for (i = 0; i < n; i++)
                    for (j = 0; j < m; j++)

                    {
                        if (index13a == 1) randomvalues[i][j] = Math.Pow(Math.E, randomvalues[i][j]);
                        if (index13a == 2) randomvalues[i][j] = Math.Pow(randomvalues[i][j], 2);
                        if (index13a == 3) randomvalues[i][j] = Math.Pow(randomvalues[i][j], 2);

                    }
                if (index13a == 3)
                {
                    NormalPathfinder distribution2 = new NormalPathfinder(n, m);
                    Dictionary<double, List<double>> otherones = new Dictionary<double, List<double>>();
                    for (i = 0; i < n; i++)
                    {
                        List<Double> tmp = new List<double>();
                        tmp = distribution2.values[i];
                        otherones.Add(i, tmp);
                         for (j = 0; j < m; j++)
                        {
                            otherones[i][j]= Math.Pow(otherones[i][j], 2);
                            randomvalues[i][j] = randomvalues[i][j] / otherones[i][j]; 

                        }

                    }


                }
            }

            Dictionary<double, int> randomdistrib = new Dictionary<double, int>();
            Dictionary<decimal, int> meanValues = new Dictionary<decimal, int>();
            Dictionary<decimal, int> varValues = new Dictionary<decimal, int>();

            for (double k = -1.0; k <= 1.0000; k = k + 0.100)
            {
                foreach (var list in randomvalues)
                {
                    foreach (var elem in list.Value)
                    {
                        if (elem > k && elem <= (k + 0.1))
                        {
                            int tmp = 1;
                            if (randomdistrib.TryGetValue(k, out tmp)) { randomdistrib.Remove(k); tmp++; }
                            randomdistrib.Add(k, tmp);
                        }
                    }
                }
            }

            foreach (var list in randomvalues)
            {
                double mean = 0;
                double variance = 0;
                foreach (var elem in list.Value)
                {
                    mean = mean + (elem - mean);

                }
                foreach (var elem in list.Value)
                {
                    variance = variance + ((elem - mean) * (elem - mean));
                }
                mean = mean - (mean % 0.1);
                variance = variance - (variance % 2);
                int tmp2 = 1;
                if (meanValues.TryGetValue((decimal)mean, out tmp2)) { meanValues.Remove((decimal)mean); tmp2++; }
                meanValues.Add((decimal)mean, tmp2);
                tmp2 = 1;
                if (varValues.TryGetValue((decimal)variance, out tmp2)) { varValues.Remove((decimal)variance); tmp2++; }
                varValues.Add((decimal)variance, tmp2);

            }

            return new Tuple<Dictionary<double, int>, Dictionary<decimal, int>, Dictionary<decimal, int>>(randomdistrib, meanValues, varValues);
        }

        public Graphics getGrapichs()
        {
            return this.g2;
        }
    }
}
