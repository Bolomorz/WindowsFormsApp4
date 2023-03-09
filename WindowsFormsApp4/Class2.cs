using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

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
        public string GenerateFile(string type)
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
                double nextdouble = (double)rd.Next(int.MaxValue / 100, int.MaxValue) / (double)int.MaxValue;
                double nextx = nextdouble / 10;
                if((nextx + XAxis.last) < XAxis.max)
                {
                    AddValue(XAxis, nextx);
                    foreach(FileAxis fa in YAxes)
                    {
                        double interval = fa.max - fa.min;
                        nextdouble = (double)rd.Next(int.MaxValue / 100, int.MaxValue) / (double)int.MaxValue;
                        double nexty = nextdouble * interval / 100;
                        AddValue(fa, nexty);
                    }
                }
                else
                {
                    cont = false;
                }
            }

            switch(type)
            {
                case ".txt":
                    filepath = FindFileName();
                    SaveFileTxt();
                    break;
                case ".xlsx":
                    filepath = ExcelReaderWriter.FindNextFileName();
                    SaveFileXlsx();
                    break;
                default:
                    MessageBox.Show("FileFormat <" + type + "> not supported!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
            }

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
            next = Math.Round(next, 5);
            axis.values.Add(next);
            axis.last = next;
        }

        /// <summary>
        /// Find next nonexistent file with predefined filenameformat "TestFile#.txt"
        /// </summary>
        private string FindFileName()
        {
            string ret = string.Empty;

            int i = 0;
            bool cont = true;
            while(cont)
            {
                string path = @"C:\Users\dominik.schneider\Documents\YAxisAppFiles\" + "TestFile" + i + ".txt";
                if (!File.Exists(path))
                {
                    ret = path;
                    cont = false;
                }
                else
                {
                    i++;
                }
            }

            return ret;
        }

        /// <summary>
        /// Save file in filepath.
        /// Read lines from values in FileAxes.
        /// Append lines to file in filepath.
        /// </summary>
        private void SaveFileTxt()
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

        /// <summary>
        /// Save file in filepath to .xlsx format.
        /// </summary>
        private void SaveFileXlsx()
        {
            ExcelReaderWriter writer = new ExcelReaderWriter(filepath);

            Color[] colors = { Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Turquoise, Color.Purple, Color.Yellow, Color.Black };

            writer.WriteCell(1, 2, "XValues");
            writer.WriteCell(2, 2, XAxis.name);
            int col = 3;
            foreach (FileAxis fx in YAxes)
            {
                writer.WriteCell(1, col, "YValues");
                writer.WriteCell(2, col, fx.name);
                writer.SetColor(2, col, colors[col - 3]);
                col++;
            }

            int row = 3;
            int count = 0;
            foreach(double x in XAxis.values)
            {
                writer.WriteCell(row, 1, count);
                writer.WriteCell(row, 2, x);

                int column = 3;
                foreach(FileAxis fx in YAxes)
                {
                    writer.WriteCell(row, column, fx.values[count]);
                    column++;
                }

                row++;
                count++;
            }

            writer.SaveChanges();
            writer.Quit();
        }
       
    }
}
