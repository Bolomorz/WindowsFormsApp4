using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    /// <summary>
    /// List of values of a Series in interval [min, max] with Series name.
    /// For generating file with values.
    /// </summary>
    class FileAxis
    {
        public List<double> values = new List<double>();
        public int min, max;
        public double last;
        public string name;
        public bool direction;

        /// <summary>
        /// FileAxis with interval [0, 10] and name "Ampere".
        /// </summary>
        public static FileAxis AMP = new FileAxis(0, 10, "Ampere");

        /// <summary>
        /// FileAxis with interval [10, 15] and name "Volt".
        /// </summary>
        public static FileAxis VOL = new FileAxis(10, 15, "Volt");

        /// <summary>
        /// FileAxis with interval [0, 1] and name "Efficiency".
        /// </summary>
        public static FileAxis EFF = new FileAxis(0, 1, "Efficiency");

        /// <summary>
        /// FileAxis with interval [1000, 10000] and name "RotationsPerMinute".
        /// </summary>
        public static FileAxis RPM = new FileAxis(1000, 10000, "RotationsPerMinute");

        /// <summary>
        /// FileAxis with interval [0, 10] and name "NewtonMeter".
        /// </summary>
        public static FileAxis NM = new FileAxis(0, 10, "NewtonMeter");

        /// <summary>
        /// FileAxis with interval [0, 150] and name "Power".
        /// </summary>
        public static FileAxis POW = new FileAxis(0, 150, "Power");

        /// <summary>
        /// Create FileAxis with default values.
        /// </summary>
        public FileAxis()
        {
            values.Clear();
            name = "";
            min = 0;
            max = 0;
            last = 0;
            direction = true;
        }

        /// <summary>
        /// Create FileAxis with set interval and name
        /// </summary>
        /// <param name="imin">min value</param>
        /// <param name="imax">max value</param>
        /// <param name="sname">series name</param>
        public FileAxis(int imin, int imax, string sname)
        {
            values.Clear();
            name = sname;
            min = imin;
            max = imax;
            last = 0;
            direction = true;
        }
    }
}
