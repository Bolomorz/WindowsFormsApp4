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
        //-
        //List of Values of y-Series
        public List<double> values = new List<double>();
        //-

        //-
        //default max and min value for YAxis.Minimum YAxis.Maximum
        public int min, max; 
        //-

        //-
        //setmin and setmax can be changed by user
        //interval is (setmax-setmin)/10
        public double setmin, setmax, interval;
        //-

        //-
        //name for Legend; Color of Series
        public string name;
        public System.Drawing.Color color;
        //-

        public YSeries(string sinput, System.Drawing.Color cinput)
        {
            name = sinput;
            color = cinput;
            values.Clear();

            //-
            //set min and max, s.t. firstInput < min AND firstInput > max
            min = int.MaxValue;
            max = int.MinValue;
            //-
        }

        public void Add(double input)
        {
            //Add value <input> to List; change min/max values if necessary
            values.Add(input);
            CompareMax(input);
            CompareMin(input);
            setmin = min;
            setmax = max;
            interval = (max - min) / 10;
        }

        public void ChangeSetMax(double input)
        {
            //User can change maximum of YAxis through TextBox
            if(input > setmin)
            {
                setmax = input;
                interval = (setmax - setmin) / 10;       
            }
        }

        public void ChangeSetMin(double input)
        {
            //User can change minimum of YAxis through TextBox
            if(input < setmax)
            { 
                setmin = input;
                interval = (setmax - setmin) / 10;
            }
        }

        public void ResetSetMax()
        {
            //reset maximum of YAxis to default value
            
            setmax = max;
            interval = (setmax - setmin) / 10;
        }

        public void ResetSetMin()
        {
            //reset minimum of YAxis to default value

            setmin = min;
            interval = (setmax - setmin) / 10;
        }

        private void CompareMax(double input)
        {
            //
            //string output = Environment.NewLine + "CompareMax(" + input + ")" + max + Environment.NewLine;
            //Compare <input> with current maxValue, change maxValue if necessary
            if(input > max)
            {
                //output += "input > max = true" + Environment.NewLine;
                int val = 1;

                //-
                //Find position of <value>, f.e. input = 345 => val = 100;
                while(input > val)
                {
                    //output += "input: " + input + " val: " + val + Environment.NewLine;
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
                //output += val + Environment.NewLine;
                //-

                //-
                //Find new maxvalue from <value>, f.e. input = 345 => max = 400;
                max = 0;
                while(input > max)
                {
                    //output += "max: " + max + " val: " + val + Environment.NewLine;
                    max = max + val;
                }
                //-
            }
            else
            {
                //output += "input > max = false" + Environment.NewLine;
            }
            //File.AppendAllText(@"C:\Users\dominik.schneider\Documents\test.txt", output);
        }

        private void CompareMin(double input)
        {
            //string output = Environment.NewLine + "CompareMin(" + input + ")" + min + Environment.NewLine;
            //Compare <input> with current minValue, change minValue if necessary
            if (input < min)
            {
                //output += "input < min = true" + Environment.NewLine; 
                int val;
                if(input < 0)
                {
                    //output += "input < 0 = true" + Environment.NewLine;
                    val = -1;
                    while (input < val)
                    {
                        //output += "input: " + input + " val: " + val + Environment.NewLine;
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
                    //output += "val: " + val + Environment.NewLine;
                    min = 0;
                    while(input < min)
                    {
                        //output += "input: " + input + " min: " + min + Environment.NewLine;
                        min = min + val;
                    }
                    //output += "min: " + min + Environment.NewLine;
                }
                else
                {
                    //output += "input < 0 = false" + Environment.NewLine;
                    val = 1;
                    while (input > val)
                    {
                        //output += "input: " + input + " val: " + val + Environment.NewLine;
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
                    //output += " val: " + val + Environment.NewLine;
                    min = 0;
                    while(input > min + val)
                    {
                        //output += "input: " + input + " min: " + min + Environment.NewLine;
                        min = min + val;
                    }
                    //output += "min: " + min + Environment.NewLine;
                }
            }
            else
            {
                
                //output += "input < min = false" + Environment.NewLine;
            }
            //File.AppendAllText(@"C:\Users\dominik.schneider\Documents\test.txt", output);

            //Compare minValue with maxValue
            //if minvalue < maxvalue/10 then minvalue = 0
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
