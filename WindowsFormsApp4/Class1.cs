using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApp4
{
    public class YSeries
    {
        public List<double> values = new List<double>();

        public int min, max; 

        public double setmin, setmax, interval;

        public string name;
        public System.Drawing.Color color;

        /// <summary>
        /// Create YSeries with input name and input color.
        /// </summary>
        /// <param name="sinput">input name</param>
        /// <param name="cinput">input color</param>
        public YSeries(string sinput, System.Drawing.Color cinput)
        {
            name = sinput;
            color = cinput;
            values.Clear();

            min = int.MaxValue;
            max = int.MinValue;
        }

        /// <summary>
        /// Add input value to valuesList.
        /// Change max and min value if necessary.
        /// </summary>
        /// <param name="input"></param>
        public void Add(double input)
        {
            values.Add(input);
            CompareMax(input);
            CompareMin(input);
            setmin = min;
            setmax = max;
            interval = (max - min) / 10;
        }

        /// <summary>
        /// Change max value to input value.
        /// </summary>
        /// <param name="input"></param>
        public void ChangeSetMax(double input)
        {
            if(input > setmin)
            {
                setmax = input;
                interval = (setmax - setmin) / 10;       
            }
        }

        /// <summary>
        /// Change min value to input value.
        /// </summary>
        /// <param name="input"></param>
        public void ChangeSetMin(double input)
        {
            if(input < setmax)
            { 
                setmin = input;
                interval = (setmax - setmin) / 10;
            }
        }

        /// <summary>
        /// Reset max value to default value.
        /// </summary>
        public void ResetSetMax()
        {
            setmax = max;
            interval = (setmax - setmin) / 10;
        }

        /// <summary>
        /// Reset min value to default value.
        /// </summary>
        public void ResetSetMin()
        {
            setmin = min;
            interval = (setmax - setmin) / 10;
        }

        /// <summary>
        /// Compare input value with max value.
        /// Change max value if necessary.
        /// s.t. input = 324 => max = 400
        /// </summary>
        /// <param name="input"></param>
        private void CompareMax(double input)
        {
            if(input > max)
            {
                int val = 1;

                while(input > val)
                {
                    val = val * 10;
                }
                if(val == 1)
                {
                    val = 1;
                }
                else
                {
                    val = val / 10;
                }
                max = 0;
                while(input > max)
                {
                    max = max + val;
                }
            }
        }

        /// <summary>
        /// Compare input value with min value.
        /// Change min value if necessary.
        /// s.t. input = 324 => min = 300
        /// </summary>
        /// <param name="input"></param>
        private void CompareMin(double input)
        {
            if (input < min)
            {
                int val;
                if(input < 0)
                {
                    val = -1;
                    while (input < val)
                    {
                        val = val * 10;
                    }
                    if(val == -1)
                    {
                        val = -1;
                    }
                    else
                    {
                        val = val / 10;
                    }
                    min = 0;
                    while(input < min)
                    { 
                        min = min + val;
                    }
                }
                else
                {
                    val = 1;
                    while (input > val)
                    {
                        val = val * 10;
                    }
                    if(val == 1)
                    {
                        val = 1;
                    }
                    else
                    {
                        val = val / 10;
                    }
                    min = 0;
                    while(input > min + val)
                    {
                        min = min + val;
                    }
                }
            }

            if(min < 0)
            {
                if((-1) * min < max / 10)
                {
                    min = (-1) * (max / 10);
                }
            }
            else
            {
                if(min < max / 10)
                {
                    min = 0;
                }
            }
        }
    }
}
