using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PhisycalParticals
{

    public partial class MainWindow : Window
    {
        private List<ParticalViewModel> p = new List<ParticalViewModel>();
        
        //private double sc = 0.01; //speed коэфициент, БЕСИТ ПИСАТЬ ЭТО СЛОВО ТЕМ БОЛЕЕЕ НА АНГЛИЙСКОМ
        private double G = 6.67384/100000000;//6.67384 × 10^(-11) Гравитационная постоянная 

        private DispatcherTimer timer = new DispatcherTimer();
        private Random _rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 0; i++)
            {
                ParticalViewModel part = new ParticalViewModel();
                part.Density = 100000;
                part.Mass = 1000000;
                part.Inertia = 0;
                part.Rad = _rnd.NextDouble() * 2 * Math.PI;
                part.X = _rnd.Next(0, 517);
                part.Y = _rnd.Next(0, 320);
                p.Add(part);

                Border b = new Border();
                b.Background = Brushes.White;
                b.Height = Math.Pow((4 * Math.PI * part.Mass) / (3 * part.Density), 1 / 3f) * 2;//3.47293138579
                b.Width = b.Height;
                b.CornerRadius = new CornerRadius(b.Width);
                b.Opacity = 0.7;

                map.Children.Add(b);
            }

            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Action();
        }

        private void Action()
        {
            for(int i = 0; i< p.Count(); i++)
                for(int j = 0; j< p.Count(); j++)
                    if (i != j)
                    {
                        double sx = p[j].X - p[i].X;
                        double sy = p[j].Y - p[i].Y;
                        double s = Math.Sqrt(sx * sx + sy * sy);
                        double t = newAngle(sx, sy);

                        

                        double f = (G * p[i].Mass * p[j].Mass) / (s * s);

                        if (s < (Math.Pow((4 * Math.PI * p[i].Mass) / (3 * p[i].Density), 1 / 3f) + Math.Pow((4 * Math.PI * p[j].Mass) / (3 * p[j].Density), 1 / 3f)))
                        {
                            f = (G * p[i].Mass * p[j].Mass) / (Math.Pow((4 * Math.PI * p[i].Mass) / (3 * p[i].Density), 1 / 3f) + Math.Pow((4 * Math.PI * p[j].Mass) / (3 * p[j].Density), 2 / 3f));
                        }

                        double tx = p[i].X;
                        double ty = p[i].Y;
                        tx += Math.Cos(t) * f;
                        ty += Math.Sin(t) * f;
                        tx += Math.Cos(p[i].Rad) * p[i].Inertia;
                        ty += Math.Sin(p[i].Rad) * p[i].Inertia;

                        sx = tx - p[i].X;
                        sy = ty - p[i].Y;

                        s = Math.Sqrt(sx * sx + sy * sy);
                        t = newAngle(sx, sy);

                        p[i].Rad = t;
                        p[i].Inertia = s;

                        
                    }
            for (int i = 0; i < p.Count(); i++)
            {
                p[i].X = p[i].X + Math.Cos(p[i].Rad) * p[i].Inertia / p[i].Mass;
                p[i].Y = p[i].Y + Math.Sin(p[i].Rad) * p[i].Inertia / p[i].Mass;
            }
        }

        private void Draw()
        {
            for (int i = 0; i < p.Count(); i++)
            {
                #region отражение от правой и левой границы
                if (p[i].X < 0)
                {
                    p[i].X = 0;

                    if (p[i].Rad > Math.PI)
                        p[i].Rad = Math.PI/2 - p[i].Rad % (Math.PI/2) + Math.PI*3.0/2.0;
                    else
                        p[i].Rad = Math.PI/2 - p[i].Rad % (Math.PI/2);
                }
                else if (p[i].X > map.ActualWidth)
                {
                    p[i].X = map.ActualWidth;

                    if (p[i].Rad < Math.PI)
                        p[i].Rad = Math.PI - p[i].Rad;
                    else
                        p[i].Rad = Math.PI*3.0/2.0 - (p[i].Rad % (Math.PI / 2));
                }
                #endregion

                if (p[i].Y < 0)
                {
                    p[i].Y = 0;
                    if (p[i].Rad > Math.PI / 2)
                        p[i].Rad = Math.PI * 2 - p[i].Rad;
                    else
                        p[i].Rad = p[i].Rad % (Math.PI / 2) + Math.PI;
                }
                else if (p[i].Y > map.ActualHeight)
                {
                    p[i].Y = map.ActualHeight;
                    if (p[i].Rad > Math.PI/2.0)
                        p[i].Rad =Math.PI*3.0/2.0 - p[i].Rad % (Math.PI/2.0);
                    else
                        p[i].Rad = Math.PI*2 - p[i].Rad  ;
                }

                Canvas.SetLeft(map.Children[i], p[i].X - Math.Pow((4 * Math.PI * p[i].Mass) / (3 * p[i].Density), 1 / 3f));
                Canvas.SetBottom(map.Children[i], p[i].Y - Math.Pow((4 * Math.PI * p[i].Mass) / (3 * p[i].Density), 1 / 3f));
            }
        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {


            ParticalViewModel part = new ParticalViewModel();
            part.Density = 50000;
            part.Mass = _rnd.Next(100000, 300000);
            part.X = _rnd.Next(0, (int)map.ActualWidth);
            part.Y = _rnd.Next(0, (int)map.ActualHeight);
            part.Inertia = _rnd.Next(0, 500000);
            part.Rad = _rnd.NextDouble() * Math.PI * 2.0;

            if (e.Key == Key.Space)
            {
                part.Density = 100000;
                part.Mass = _rnd.Next(100000000, 200000000);
                part.Inertia = 0;
                part.X = map.ActualWidth/2;
                part.Y = map.ActualHeight/2;
            }

            p.Add(part);
            Border b = new Border();
            b.Background = Brushes.White;
            b.Height = Math.Pow((4 * Math.PI * part.Mass) / (3 * part.Density), 1 / 3f) * 2;//3.47293138579
            b.Width = b.Height;
            b.CornerRadius = new CornerRadius(b.Width);
            b.Opacity = 0.7;

            map.Children.Add(b);
        }

        private double newAngle(double x, double y)
        {
            double t = 0;

            if ((x > 0) & (y >= 0))
                t = Math.Atan(y / x);
            else if ((x > 0) & (y < 0))
                t = Math.Atan(y / x) + Math.PI * 2.0;
            else if (x < 0)
                t = Math.Atan(y / x) + Math.PI;
            else if ((x == 0) & (y > 0))
                t = Math.PI / 2.0;
            else if ((x == 0) & (y < 0))
                t = Math.PI * 3.0 / 2.0;

            return t;
        }//thats work

        private ParticalViewModel newVector(int iNumber, int itNumber, double additionalAng)
        {
            //double alpha = _particals[iNumber].Rad % Math.PI + additionalAng % (Math.PI/2); // Уголмежду векторами
            double lx = p[iNumber].X - p[itNumber].X;
            double ly = p[iNumber].Y - p[itNumber].Y;
            double length = Math.Sqrt(lx*lx + ly*ly);//Посколько в следующей формуле необходим квадрат, просто не будем вычислять корень из этой формулы
            double F = (G * p[iNumber].Mass * p[itNumber].Mass) / (length*length);
            if(length < 1)
                F = (G * p[iNumber].Mass * p[itNumber].Mass);

            double x = p[iNumber].X
                + Math.Cos(p[iNumber].Rad) * p[iNumber].Inertia
                + Math.Cos(additionalAng) * F;

            double y = p[iNumber].Y
                + Math.Sin(p[iNumber].Rad) * p[iNumber].Inertia
                + Math.Sin(additionalAng) * F;

            lx = p[iNumber].X - x;
            ly = p[iNumber].Y - y;
            length = Math.Sqrt(lx*lx + ly*ly);//Длинна вектора новой инерции
            double rad = newAngle(x - p[iNumber].X, y - p[iNumber].Y);

            return new ParticalViewModel() { 
                X = p[iNumber].X,
                Y = p[iNumber].Y,
                Mass = p[iNumber].Mass,
                Density = p[iNumber].Density,
                Inertia = length,
                Rad = rad
            };
        }
    }
}
