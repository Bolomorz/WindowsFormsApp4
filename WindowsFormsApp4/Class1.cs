using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    public class YSeries
    {
        public List<double> values = new List<double>();
        public double min, max, interval, val;
        public string name;
        public System.Drawing.Color color;

        public YSeries(string sinput, System.Drawing.Color cinput)
        {
            name = sinput;
            color = cinput;
            values.Clear();
            min = double.MaxValue;
            max = double.MinValue;
            val = 1;
        }

        public void Add(double input)
        {
            values.Add(input);
        }

        private void CompareVal(double input)
        {
            if(input > 0)
            {
                while(input > val)
                {
                    val = val * 10;
                }
            }
            else
            {
                while(input * -1 > val)
                {
                    val = val * 10;
                }
            }
        }
        private void CompareMax(double input)
        {
            if(input > max)
            {
                double div = input / (val / 10);
                double idiv = Math.Floor(div);
                max = val * idiv;
            }
        }
        private void CompareMin(double input)
        {
            if(input < min)
            {
                double div = input / (val / 10);
                double idiv = Math.Floor(div);
                min = val * idiv;
            }
        }
    }
}
