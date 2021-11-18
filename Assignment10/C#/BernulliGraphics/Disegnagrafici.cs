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
        int n, m;
        Matrix m1;
        Matrix m2;
        Pathfinder distrubution;

        public DisegnaGrafici(int m, int n, int j, int lambda, Graphics graphics, Rectangle vPort, int dinamicleft, int dinamictop, int contgw, int contgh, bool is_secondex)
        {
            this.g2 = graphics;
            this.viewPort = vPort;
            this.viewPort.X = dinamicleft;
            this.viewPort.Y = dinamictop;
            this.viewPort.Width = contgw;
            this.viewPort.Height = contgh;
            this.n = n;
            this.m = m;
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

            //genero le "strade"
            if (is_secondex == false)
            {
                distrubution = new BernulliPathfinder(n, m, lambda);

                disegnaPaths(fromPathstoViewport(distrubution.Get_paths(), viewPort));

                disegnaHistogramma(viewPort, getDistribution(distrubution.Get_paths(), m / SCALE, j), n, j);
                disegnaHistogramma(viewPort, getDistribution(distrubution.Get_paths(), m / SCALE, n), n, n);
                //disegno le distanze
                disegnaDistanze(viewPort, individualJumpFromOriginD(distrubution.Get_paths()), 0, "Single jump distace ");
                disegnaDistanze(viewPort, doublejumpD(distrubution.Get_paths()), 80, "Double jump distance");

            }
            //disegno i vari path convertendoli per il viewport
            else
            {
                disgnaIstogrammi(viewPort, valueToDictionarys(n, m));

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
                y = (int)(viewPort.Top + viewPort.Height / 4 - v.Value / 10);


                width = 15;
                height = v.Value / 10;

                Rectangle rectangle = new Rectangle(x, y, width, height);

                g2.DrawRectangle(Pens.Black, rectangle);
                g2.FillRectangle(semiTransBrush, rectangle);

                g2.FillRectangle(Brushes.Cyan, rectangle);
                g2.DrawString(((decimal)v.Key).ToString(), new Font("Calibri", 10.0f,
                                 FontStyle.Regular, GraphicsUnit.Pixel), semiTransBrush, new Point(x, y+v.Value/10+1));
                i++;
            }
            g2.DrawString("Distribuzione originale ", new Font("Calibri", 13.0f,
                               FontStyle.Italic, GraphicsUnit.Pixel), semiTransBrush, new Point(this.viewPort.Left - 140, (viewPort.Top + viewPort.Height/4)));

            i = 0;

            foreach (var v in tuple.Item2)
            {
                int x, y;
                int width, height;
                // in this case on the fly trasformation is way faster
                x = (int)(this.viewPort.Left + 20 * i);
                y = (int)(viewPort.Top + viewPort.Height /2  - v.Value / 2);


                width = 15;
                height = v.Value / 2;

                Rectangle rectangle = new Rectangle(x, y, width, height);

                g2.DrawRectangle(Pens.Black, rectangle);
                g2.FillRectangle(semiTransBrush, rectangle);

                g2.FillRectangle(Brushes.Cyan, rectangle);
                g2.DrawString(v.Key.ToString(), new Font("Calibri", 10.0f,
                                 FontStyle.Regular, GraphicsUnit.Pixel), semiTransBrush, new Point(x, y+v.Value/2));
                i++;
            }
            g2.DrawString("Min ", new Font("Calibri", 13.0f,
                               FontStyle.Italic, GraphicsUnit.Pixel), semiTransBrush, new Point(this.viewPort.Left - 50, (viewPort.Top + viewPort.Height/2)));



            i = 0;

            foreach (var v in tuple.Item3)
            {
                int x, y;
                int width, height;
                // in this case on the fly trasformation is way faster
                x = (int)(this.viewPort.Left + 20 * i);
                y = (int)(viewPort.Top + viewPort.Height -30  - v.Value /2);


                width = 15;
                height = v.Value / 2;

                Rectangle rectangle = new Rectangle(x, y, width, height);

                g2.DrawRectangle(Pens.Black, rectangle);
                g2.FillRectangle(semiTransBrush, rectangle);

                g2.FillRectangle(Brushes.Cyan, rectangle);
                g2.DrawString(v.Key.ToString(), new Font("Calibri", 10.0f,
                                 FontStyle.Regular, GraphicsUnit.Pixel), semiTransBrush, new Point(x, y+v.Value/2));
                i++;
            }
            g2.DrawString("Max ", new Font("Calibri", 13.0f,
                               FontStyle.Italic, GraphicsUnit.Pixel), semiTransBrush, new Point(this.viewPort.Left - 50, (viewPort.Top + viewPort.Height-30 )));



        }

        private void disegnaDistanze(Rectangle viewPort, Dictionary<int, int> intervals, int offset, String text)
        {
            int i = 0;
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));

            foreach (var v in intervals)
            {
                int x, y;
                int width, height;
                // in this case on the fly trasformation is way faster
                x = (int)(this.viewPort.Left + 20 * i);
                y = (int)(viewPort.Top + viewPort.Height + offset);


                width = v.Value;
                height = viewPort.Height / intervals.Count;

                Rectangle rectangle = new Rectangle(x, y, width, height);

                g2.DrawRectangle(Pens.Black, rectangle);
                g2.FillRectangle(semiTransBrush, rectangle);

                g2.FillRectangle(Brushes.Violet, rectangle);
                g2.DrawString(v.Key.ToString(), new Font("Calibri", 10.0f,
                                 FontStyle.Regular, GraphicsUnit.Pixel), semiTransBrush, new Point(x, y));
                i++;
            }
            g2.DrawString(text, new Font("Calibri", 13.0f,
                               FontStyle.Italic, GraphicsUnit.Pixel), semiTransBrush, new Point(this.viewPort.Left - 140, (viewPort.Top + viewPort.Height + offset)));

        }



        private Dictionary<int, int> doublejumpD(List<Strade> strades)
        {
            Dictionary<int, int> dbj = new Dictionary<int, int>();
            int i = 0;
            int tmp = 1;
            Boolean trov = false;
            foreach (Strade s in strades)
            {
                i = 0;
                tmp = 1;
                trov = false;
                while (!trov && (i < s.getPath().Count() - 2))
                {
                    if (s.getPath()[i].Y != s.getPath()[i + 1].Y && s.getPath()[i + 1].Y != s.getPath()[i + 2].Y) trov = true;
                    i++;
                }
                i = i + 1;
                if (dbj.TryGetValue(i, out tmp))
                {
                    dbj.Remove(i);
                    dbj.Add(i, tmp + 1);
                }
                else dbj.Add(i, tmp);
            }
            return dbj;
        }

        private Dictionary<int, int> individualJumpFromOriginD(List<Strade> strades)
        {
            Dictionary<int, int> dbj = new Dictionary<int, int>();
            int i = 0;
            int tmp = 1;
            Boolean trov = false;
            foreach (Strade s in strades)
            {
                i = 0;
                tmp = 1;
                trov = false;
                while (!trov && (i < s.getPath().Count() - 2))
                {
                    if (s.getPath()[i].Y != s.getPath()[i + 1].Y) trov = true;
                    i++;
                }
                i++;
                if (dbj.TryGetValue(i, out tmp))
                {
                    dbj.Remove(i);
                    dbj.Add(i, tmp + 1);
                }
                else dbj.Add(i, tmp);
            }
            return dbj;
        }

        private void disgnaIstogrammi(List<PointF> viewPortCDF)
        {

            Pen pen = new Pen(Color.Red);
            Pen pen2 = new Pen(Color.Green);

            //empirical cdf
            for (int j = 1; j < viewPortCDF.Count; j++)
            {
                g2.DrawLine(pen, (float)viewPortCDF[j - 1].X, (float)viewPortCDF[j - 1].Y,
                    (float)viewPortCDF[j].X, (float)viewPortCDF[j].Y);

                g2.DrawEllipse(pen, new Rectangle((int)viewPortCDF[j].X, (int)viewPortCDF[j].Y, 4, 4));
            }

            //theorical cdf

            g2.DrawLine(pen2, (viewPort.Left + viewPort.Width), viewPort.Top,
                  viewPort.Left, viewPort.Top + viewPort.Height);


        }

        public Tuple<Dictionary<double, int>, Dictionary<decimal, int>, Dictionary<decimal, int>> valueToDictionarys(int m, int n)
        {
            Dictionary<double, List<double>> randomvalues = new Dictionary<double, List<double>>();
            int i = 0;
            int j = 0;
            decimal min;
            decimal max;

            //generation of random ditribution
            for (i = 0; i < m; i++)
            {
                List<Double> tmp = new List<double>();
                for (j = 0; j < n; j++)
                    tmp.Add(R.NextDouble());

                randomvalues.Add(i, tmp);
            }

            Dictionary<double, int> randomdistrib = new Dictionary<double, int>();
            Dictionary<decimal, int> minValues = new Dictionary<decimal,int>();
            Dictionary<decimal, int> maxValues = new Dictionary<decimal, int>();

            for (double k = 0.0; k <= 1.0000; k = k + 0.100)
            {
                foreach (var list in randomvalues)
                {
                    foreach (var elem in list.Value)
                    {
                        if (elem >= k && elem <= (k + 0.1))
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
                min = 1;
                max = 0;
                foreach (var elem in list.Value)
                {
                    if ((decimal)elem < min) min =((decimal)( elem-(elem%0.01)));
                    if ((decimal)elem > max) max =  ((decimal)(elem - (elem % 0.01)));
                }
                int tmp2 = 1;
                if (minValues.TryGetValue(min, out tmp2)) { minValues.Remove(min); tmp2++; }
                minValues.Add(min, tmp2);
                tmp2 = 1;
                if (maxValues.TryGetValue(max, out tmp2)) { maxValues.Remove(max); tmp2++; }
                maxValues.Add(max, tmp2);

            }

            return new Tuple<Dictionary<double, int>, Dictionary<decimal, int>, Dictionary<decimal, int>>(randomdistrib, minValues, maxValues);
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
                y = (int)(viewPort.Top + viewPort.Height * ((100 - (intervals[i].UpperBound) * 100) / 100));


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
