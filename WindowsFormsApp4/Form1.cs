using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using cc = System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public List<double> xval = new List<double>();
        public List<YSeries> yval = new List<YSeries>();

        public int x1, x2;

        public bool gridEnabled;

        public string xvalname;

        public string chartTitle, chartDate;

        /// <summary>
        /// path to currently selected file
        /// </summary>
        public string filepath;

        double lastx;

        /// <summary>
        /// List of buttons, one for each YSeries
        /// </summary>
        public List<Button> buttons = new List<Button>();


        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Return double value of input string.
        /// Works regardless of wether ',' or '.' is used as separator.
        /// </summary>
        /// <param name="val">input string</param>
        /// <returns></returns>
        public double ReadString(string val)
        {
            double ret;
            string str = "";
            foreach(char c in val)
            {
                if(IsNumeric(c))
                {
                    str += c;
                }
                else if(c == ',' || c == '.')
                {
                    str += System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                }
            }
            ret = double.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint);
            return ret;
        }

        /// <summary>
        /// Test, if char is numeric.
        /// Returns true, if input char is numeric.
        /// </summary>
        /// <param name="val">input char</param>
        /// <returns></returns>
        private bool IsNumeric(char val)
        {
            bool isNumeric = false;
            if(val == '0' || val == '1' || val == '2' || val == '3' || val == '4' || val == '5' || val == '6' || val == '7' || val == '8' || val == '9')
            {
                isNumeric = true;
            }
            return isNumeric;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Set Variables on FormLoad
            gridEnabled = false;
            chartDate = DateTime.Now.ToString("d");
            filepath = "";
            chartTitle = "TESTTITEL";
        }

        /// <summary>
        /// On ButtonClickEvent execute OpenFileDialog to select file to open and read
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string path = "";

            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Open File Dialog";
            fd.InitialDirectory = @"C:\Users\dominik.schneider\Documents\";
            fd.Filter = "All files (*.txt) | *.txt";
            fd.FilterIndex = 1;
            fd.Multiselect = false;
            fd.RestoreDirectory = true;

            if(fd.ShowDialog() == DialogResult.OK)
            {
                path = fd.FileName;
            }

            filepath = path;
            textBox1.Text = path;
        }

        /// <summary>
        /// read from file, if TextBox content is changed by OpenFileDialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = filepath;

            if(textBox1.Text != "")
            {
                ReadData();
                DrawChart();
            }
        }

        /// <summary>
        /// Create YAxis for YSeries.
        /// Create 2 ChartAreas;
        /// 1. ChartArea on top of default ChartArea -> show only Series;
        /// 2. ChartArea with offset to the left to default ChartArea -> show only YAxis
        /// </summary>
        /// <param name="chart">default Chart</param>
        /// <param name="area">default ChartArea</param>
        /// <param name="yseries">current YSeries</param>
        /// <param name="axisOffset"></param>
        /// <param name="labelsSize"></param>
        private void CreateYAxis(
            cc.Chart chart,                         
            cc.ChartArea area,                      
            YSeries yseries,                        
            float axisOffset, float labelsSize)
        {
            cc.Series series = new cc.Series();
            series.Name = yseries.name;
            for (int i = 0; i < yseries.values.Count(); i++)
            {
                if (xval.ElementAt(i) >= x1 && xval.ElementAt(i) <= x2)
                {
                    series.Points.AddXY(xval.ElementAt(i), yseries.values.ElementAt(i));
                }
            }
            series.Color = yseries.color;
            series.ChartType = cc.SeriesChartType.Line;
            series.BorderWidth = 2;
            chart.Series.Add(series);

            cc.ChartArea areaSeries = chart.ChartAreas.Add("ChartArea_" + series.Name);
            areaSeries.BackColor = Color.Transparent;
            areaSeries.BorderColor = Color.Transparent;
            areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
            areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
            areaSeries.AxisX.MajorGrid.Enabled = false;
            areaSeries.AxisX.MajorTickMark.Enabled = false;
            areaSeries.AxisX.LabelStyle.Enabled = false;
            areaSeries.AxisX.Interval = area.AxisX.Interval;
            areaSeries.AxisX.Minimum = area.AxisX.Minimum;
            areaSeries.AxisX.Maximum = area.AxisX.Maximum;
            areaSeries.AxisY.MajorGrid.Enabled = false;
            areaSeries.AxisY.MajorTickMark.Enabled = false;
            areaSeries.AxisY.LabelStyle.Enabled = false;
            areaSeries.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;

            areaSeries.AxisY.Minimum = yseries.setmin;
            areaSeries.AxisY.Maximum = yseries.setmax;
            areaSeries.AxisY.Interval = yseries.interval;

            series.ChartArea = areaSeries.Name;

            cc.ChartArea areaAxis = chart.ChartAreas.Add("AxisY_" + series.ChartArea);
            areaAxis.BackColor = Color.Transparent;
            areaAxis.BorderColor = Color.Transparent;
            areaAxis.Position.FromRectangleF(chart.ChartAreas.FindByName(series.ChartArea).Position.ToRectangleF());
            areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas.FindByName(series.ChartArea).InnerPlotPosition.ToRectangleF());

            cc.Series seriesCopy = chart.Series.Add(series.Name + "_Copy");
            seriesCopy.ChartType = series.ChartType;
            foreach (cc.DataPoint point in series.Points)
            {
                seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
            }
            seriesCopy.IsVisibleInLegend = false;
            seriesCopy.Color = Color.Transparent;
            seriesCopy.BorderColor = Color.Transparent;
            seriesCopy.ChartArea = areaAxis.Name;

            areaAxis.AxisX.LineWidth = 0;
            areaAxis.AxisX.MajorGrid.Enabled = false;
            areaAxis.AxisX.MajorTickMark.Enabled = false;
            areaAxis.AxisX.LabelStyle.Enabled = false;
            areaAxis.AxisY.MajorGrid.Enabled = false;
            areaAxis.AxisY.LineColor = series.Color;
            areaAxis.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
            areaAxis.AxisY.LabelStyle.Font = area.AxisY.LabelStyle.Font;

            areaAxis.AxisX.Minimum = area.AxisX.Minimum;
            areaAxis.AxisX.Maximum = area.AxisX.Maximum;
            areaAxis.AxisX.Interval = area.AxisX.Interval;

            areaAxis.AxisY.Minimum = yseries.setmin;
            areaAxis.AxisY.Maximum = yseries.setmax;
            areaAxis.AxisY.Interval = yseries.interval;

            areaAxis.Position.X = axisOffset;
            areaAxis.InnerPlotPosition.X = areaAxis.InnerPlotPosition.X + labelsSize;
        }

        /// <summary>
        /// Create data points for each YSeries.
        /// Find closest xvalue in xval to integers in [x1, x2]
        /// </summary>
        /// <param name="series"></param>
        private void CreateDataPoints(
            cc.Series series)
        {
            int index = 0;
            int c = series.Points.Count();

            for(int i = x1; i <= x2; i++)
            {
                bool cont = true;
                double id = i;
                while(cont)
                {
                    double x = series.Points.ElementAt(index).XValue;
                    if(x < id)
                    {
                        index++;
                        if(index == c)
                        {
                            index = c - 1;
                            cont = false;
                        }
                    }
                    else
                    {
                        cont = false;
                        index--;
                        if(index < 0)
                        {
                            index = 0;
                        }
                    }
                }
                series.Points.ElementAt(index).MarkerStyle = cc.MarkerStyle.Circle;
                series.Points.ElementAt(index).MarkerColor = series.Color;
                series.Points.ElementAt(index).MarkerSize = 5;
            }
        }

        /// <summary>
        /// Draw chart with values of currently listed YSeries
        /// </summary>
        public void DrawChart()
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();
            chart1.Legends.Clear();
            foreach(Button b in buttons)
            {
                Controls.Remove(b);
            }
            buttons.Clear();

            cc.ChartArea c1 = new cc.ChartArea();
            c1.Name = "Default";
            chart1.ChartAreas.Add(c1);

            int leftoffset;
            int downoffset = 10;
            leftoffset = yval.Count() * 3 + 2;

            chart1.ChartAreas.FindByName("Default").Position = new cc.ElementPosition(leftoffset, downoffset, 100-leftoffset, 100-downoffset);
            chart1.ChartAreas.FindByName("Default").InnerPlotPosition = new cc.ElementPosition(0, 0, 95, 90);
            chart1.ChartAreas.FindByName("Default").AxisX.MajorGrid.Enabled = gridEnabled;
            chart1.ChartAreas.FindByName("Default").AxisX.MinorGrid.Enabled = gridEnabled;
            chart1.ChartAreas.FindByName("Default").AxisY.MajorGrid.Enabled = gridEnabled;
            chart1.ChartAreas.FindByName("Default").AxisY.MinorGrid.Enabled = gridEnabled;
            chart1.ChartAreas.FindByName("Default").AxisX.Interval = (x2 - x1) / 10;
            chart1.ChartAreas.FindByName("Default").AxisY.Interval = (x2 - x1) / 10;
            chart1.ChartAreas.FindByName("Default").AxisX.Minimum = x1;
            chart1.ChartAreas.FindByName("Default").AxisX.Maximum = x2;
            chart1.ChartAreas.FindByName("Default").AxisY.Minimum = x1;
            chart1.ChartAreas.FindByName("Default").AxisY.Maximum = x2;
            chart1.ChartAreas.FindByName("Default").AxisX.IsStartedFromZero = true;
            chart1.ChartAreas.FindByName("Default").AxisX.Title = xvalname;

            chart1.Titles.Add(new cc.Title(chartTitle));
            chart1.Titles.Add(new cc.Title(chartDate));

            chart1.Titles.ElementAt(1).Position = new cc.ElementPosition(94, 0, 5, 5);
            chart1.Titles.ElementAt(0).Font = new Font("Arial", 20, FontStyle.Bold);

            chart1.Legends.Add(new cc.Legend("Legend1"));
            chart1.Legends.FindByName("Legend1").Position = new cc.ElementPosition(5, 0, 9, 9);

            cc.Series xserie = new cc.Series();
            xserie.ChartType = cc.SeriesChartType.Line;
            xserie.IsVisibleInLegend = false;
            xserie.Color = Color.Transparent;
            for(int i = x1; i <= x2; i++)
            {
                xserie.Points.AddXY(i, i);
            }
            chart1.Series.Add(xserie);

            float offset = 0;
            foreach(YSeries yseries in yval)
            {
                CreateYAxis(chart1, chart1.ChartAreas.FindByName("Default"), yseries, offset, 4);
                offset += 3;
            }

            if(xval.Count > 0)
            {
                foreach(YSeries yseries in yval)
                {
                    CreateDataPoints(chart1.Series.FindByName(yseries.name));
                }
            }

            CreateButtons();
        }

        /// <summary>
        /// Read data from file in filepath.
        /// </summary>
        private void ReadData()
        {
            lastx = double.MinValue;

            if(System.IO.File.Exists(filepath))
            {
                xval.Clear();
                yval.Clear();

                int count = 0;

                foreach (string line in System.IO.File.ReadLines(filepath))
                {
                    if(count == 0)
                    {
                        ReadFirstLine(line);
                    }
                    else
                    {
                        ReadLine(line);
                    }
                    count++;
                }
                //-----
                //TODO 
                //set through TextBoxes 
                x1 = 0;
                x2 = 10;
                //-
            }

        }
        
        /// <summary>
        /// Read 1. line of file. Create YSeries.
        /// lineformat: xAxisName YAxisName1.Series ... YAxisNameN.Series
        /// </summary>
        /// <param name="input"></param>
        private void ReadFirstLine(string input)
        {
            char[] separators = {' '};

            string[] axisnames = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            Color[] colors = {Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Brown, Color.Orange, Color.Black};

            int count = 0;

            foreach(string name in axisnames)
            {
                if(count == 0)
                {
                    xvalname = name;
                }
                else
                {
                    int seriesCount = count - 1;
                    if(seriesCount < colors.Length)
                    {
                        yval.Add(new YSeries(name, colors[seriesCount]));
                    }
                    else
                    {
                        yval.Add(new YSeries(name, Color.Gray));
                    }
                }
                count++;
            }
        }

        /// <summary>
        /// Read n. line of file. Add to each YSeries.
        /// lineformat: xvalue yvalueOf1.Series ... yvalueOfn.Series
        /// </summary>
        /// <param name="input"></param>
        private void ReadLine(string input)
        {
            char[] separators = {' '};

            string[] values = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            int count = 0;

            foreach(string val in values)
            {
                if(count == 0)
                {
                    xval.Add(ReadString(val));
                }
                else
                {
                    int seriesCount = count - 1;
                    yval[seriesCount].Add(ReadString(val));
                }
                count++;
            }
        }

        /// <summary>
        /// Create Button for each YSeries in yval
        /// </summary>
        private void CreateButtons()
        {
            int x = 80; int y = 110;
            for(int i = 0; i < yval.Count(); i++)
            {
                Button nb = new Button();
                nb.Location = new Point(x, y);
                x += 100;
                nb.Height = 40;
                nb.Width = 100;
                nb.BackColor = Color.Gainsboro;
                nb.ForeColor = Color.Black;
                nb.Text = (i+1) + " " + yval[i].name;
                nb.Name = (i+1) + " " + yval[i].name;
                nb.AccessibleName = i.ToString();
                nb.Font = new Font("Arial", 12);
                nb.Click += new EventHandler(NewButtonClick);
                Controls.Add(nb);
                buttons.Add(nb);
            }
        }

        /// <summary>
        /// On ButtonClickEvent -> Open Form2 with according YSeries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewButtonClick(object sender, EventArgs e)
        {
            Button nbc = (Button)sender;
            int index = Convert.ToInt32(nbc.AccessibleName);
            Form2 form2 = new Form2(yval[index], this);
            form2.Show();
        }
    }
}
