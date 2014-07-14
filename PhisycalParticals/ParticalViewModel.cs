using Saikt.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhisycalParticals
{
    public class ParticalViewModel : ViewModelBase
    {
        private double _x;
        private double _y;
        private double _inertia; //Ньютоны
        private double _rad;//угол в радианах
        private double _mass;//масса в килограммах
        private double _density;//Плотность килограмм/м^3

        public double X
        {
            get { return _x; }
            set
            {
                if (value == _x)
                    return;
                _x = value;
                OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get { return _y; }
            set
            {
                if (value == _y)
                    return;
                _y = value;
                OnPropertyChanged("Y");
            }
        }

        public double Inertia
        {
            get { return _inertia; }
            set
            {
                if (value == _inertia)
                    return;
                _inertia = value;
                OnPropertyChanged("Force");
            }
        }

        public double Rad
        {
            get { return _rad; }
            set
            {
                if (value == _rad)
                    return;
                _rad = value;
                OnPropertyChanged("Rad");
            }
        }

        public double Mass
        {
            get { return _mass; }
            set
            {
                if (value == _mass)
                    return;
                _mass = value;
                OnPropertyChanged("Mass");
            }
        }

        public double Density
        {
            get { return _density; }
            set
            {
                if (value == _density)
                    return;
                _density = value;
                OnPropertyChanged("Density");
            }
        }
    }
}
