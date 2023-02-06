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
        public double interval;
        public string name;
        public System.Drawing.Color color;

        public YSeries(string sinput, System.Drawing.Color cinput)
        {
            name = sinput;
            color = cinput;
            values.Clear();
            min = int.MaxValue;
            max = int.MinValue;
        }

        public void Add(double input)
        {
            //Add value <input> to List
            values.Add(input);
            CompareMax(input);
            CompareMin(input);
            interval = (max - min) / 10;
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
