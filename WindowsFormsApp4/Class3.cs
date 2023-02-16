using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    class FileAxis
    {
        public List<double> values = new List<double>();
        public double min, max, last;
        public string name;
        public bool direction;
        public FileAxis()
        {
            values.Clear();
            name = "";
            min = 0;
            max = 0;
            last = 0;
            direction = true;
        }
    }
}
