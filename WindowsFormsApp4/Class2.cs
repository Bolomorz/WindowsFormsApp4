using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApp4
{
    class FileGenerator
    {
        Random rd;
        public FileAxis XAxis;
        public List<FileAxis> YAxes = new List<FileAxis>();
        string filepath;
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

        public void GenerateFile()
        {

        }

        private void AddValue(FileAxis axis, double value)
        {
            double next;
            if(axis.direction)
            {
                next = axis.last + value;
                if(next > axis.max)
                {
                    next = next - value - value;
                    axis.direction = false;
                }
            }
            else
            {
                next = axis.last - value;
                if(next < axis.min)
                {
                    next = next + value + value;
                    axis.direction = true;
                }
            }
            axis.values.Add(next);
            axis.last = next;
        }

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
                File.WriteAllText(filepath, line);
                count++;
            }
        }
    }
}
