using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApp4
{
    /// <summary>
    /// List of Series in FileAxes to generate file with values of FileAxes.
    /// </summary>
    class FileGenerator
    {
        Random rd;
        public FileAxis XAxis;
        public List<FileAxis> YAxes = new List<FileAxis>();
        string filepath;

        /// <summary>
        /// Create FileGenerator with set number of YAxes.
        /// </summary>
        /// <param name="YAxisCount"></param>
        public FileGenerator(int YAxisCount)
        {
            rd = new Random();
            XAxis = new FileAxis();
            for(int i = 0; i < YAxisCount; i++)
            {
                YAxes.Add(new FileAxis());
            }
            filepath = "";
        }

        /// <summary>
        /// Create FileGenerator with default, predefined FileAxes (NM, V, A, Eff, RPM, P).
        /// </summary>
        public FileGenerator()
        {
            rd = new Random();
            XAxis = FileAxis.NM;
            YAxes.Add(FileAxis.AMP);
            YAxes.Add(FileAxis.VOL);
            YAxes.Add(FileAxis.RPM);
            YAxes.Add(FileAxis.EFF);
            YAxes.Add(FileAxis.POW);
            filepath = "";
        }

        /// <summary>
        /// Generate list of values in FileAxes.
        /// For each XValue on YValue in each YAxis.
        /// Generate file with generated values.
        /// </summary>
        /// <returns>filepath</returns>
        public string GenerateFile()
        {
            double startx = (double)(rd.Next(0, 100)) / 1000.0;
            XAxis.last = startx;
            XAxis.values.Add(startx);

            foreach(FileAxis fa in YAxes)
            {
                double starty = rd.Next(fa.min, (fa.max - (fa.max - fa.min) / 2));
                fa.values.Add(starty);
                fa.last = starty;
            }

            bool cont = true;
            while(cont)
            {
                double nextx = (double)(rd.Next(0, 100)) / 1000.0;
                if((nextx + XAxis.last) < XAxis.max)
                {
                    AddValue(XAxis, nextx);
                    foreach(FileAxis fa in YAxes)
                    {
                        double nexty;
                        if(fa.max < 10)
                        {
                            nexty = rd.NextDouble() / 100;
                        }
                        else if((fa.min < fa.max/10 && fa.min > 0) || (fa.min < 0 && fa.min > fa.max/10) || fa.min == 0)
                        {
                            nexty = (double)(rd.Next(fa.max/10, fa.max)) / (double)(fa.max * 10);
                        }
                        else if(fa.min > 0)
                        {
                            nexty = (double)(rd.Next(fa.max - fa.min, fa.max + fa.min)) / (double)((fa.max - fa.min) * 10);
                        }
                        else
                        {
                            nexty = (double)(rd.Next(fa.max + fa.min, fa.max - fa.min)) / (double)((fa.max - fa.min) * 10);
                        }
                        AddValue(fa, nexty);
                    }
                }
                else
                {
                    cont = false;
                }
            }

            FindFileName();
            SaveFile();

            return filepath;
        }

        /// <summary>
        /// Add value to FileAxis.
        /// Change, wether new values are added with + or -, if necessary 
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="value"></param>
        private void AddValue(FileAxis axis, double value)
        {
            double next;
            if(axis.direction)
            {
                next = axis.last + value;
                if(next > axis.max)
                {
                    next = next - value;
                    axis.direction = false;
                }
            }
            else
            {
                next = axis.last - value;
                if(next < axis.min)
                {
                    next = next + value;
                    axis.direction = true;
                }
            }
            axis.values.Add(next);
            axis.last = next;
        }

        /// <summary>
        /// Find next nonexistent file with predefined filenameformat "TestFile#.txt"
        /// </summary>
        private void FindFileName()
        {
            int i = 0;
            bool cont = true;
            while(cont)
            {
                string path = @"C:\Users\dominik.schneider\Documents\YAxisAppFiles\" + "TestFile" + i + ".txt";
                if (!File.Exists(path))
                {
                    filepath = path;
                    cont = false;
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// Save file in filepath.
        /// Read lines from values in FileAxes.
        /// Append lines to file in filepath.
        /// </summary>
        private void SaveFile()
        {
            string firstline = "";
            firstline += XAxis.name;
            foreach(FileAxis fx in YAxes)
            {
                firstline += " " + fx.name;
            }
            File.WriteAllText(filepath, firstline);

            int count = 0;
            foreach(double x in XAxis.values)
            {
                string line = Environment.NewLine;
                line += x.ToString();
                foreach(FileAxis fx in YAxes)
                {
                    line += " " + fx.values[count].ToString();
                }
                File.AppendAllText(filepath, line);
                count++;
            }
        }
    }
}
