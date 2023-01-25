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
        public string name;
        public System.Drawing.Color color;

        public YSeries(string sinput, System.Drawing.Color cinput)
        {
            name = sinput;
            color = cinput;
            values.Clear();
        }

        public void Add(double input)
        {
            values.Add(input);
        }
    }
}
