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
        //-
        //List of Values for each Series
        //For each xval (x-coordinate) one yval (y-coordinate) in each Series
        //for each xval[i] one yval[j].values[i] for each yval[j]
        public List<double> xval = new List<double>();
        public List<YSeries> yval = new List<YSeries>();
        //-

        //-
        //Values x1 and x2 as Intervall which will be shown in chart [x1, x2]
        //Can later be Set through 2 TextBoxes
        public int x1, x2;
        //-

        //-
        //Wether a grid will be shown in chart or not
        //Can later be Set by a CheckBox
        public bool gridEnabled;
        //-

        //-
        //Title of the x-Axis
        //Will be read out of file
        public string xvalname;
        //-

        //-
        //Title of chart; current Date
        //Title can be set through TextBox; Date will be calculated dynamically
        public string chartTitle, chartDate;
        //-

        //-
        //path of file to read
        //can be set with OpenFileDialog through ButtonClick
        public string filepath;
        //-

        double lastx;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Set Variables on FormLoad
            gridEnabled = false;
            chartDate = DateTime.Now.ToString("d");
            filepath = "";
            chartTitle = "TESTTITEL";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //OpenFileDialog on ButtonClickEvent to select file to open
            
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

            //-
            //Show opened file in TextBox
            filepath = path;
            textBox1.Text = path;
            //-
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //-
            //Text in textbox can only be overwritten by OpenFileDialog on ButtonClickEvent, cannot be manually overwritten
            textBox1.Text = filepath;
            //-

            //-
            //if Text changed from OpenFileDialog -> read data from file, draw chart
            if(textBox1.Text != "")
            {
                ReadData();
                DrawChart();
            }
            //-
        }

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

            areaSeries.AxisY.Minimum = yseries.min;
            areaSeries.AxisY.Maximum = yseries.max;
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

            areaAxis.AxisY.Minimum = yseries.min;
            areaAxis.AxisY.Maximum = yseries.max;
            areaAxis.AxisY.Interval = yseries.interval;

            areaAxis.Position.X = axisOffset;
            areaAxis.InnerPlotPosition.X = areaAxis.InnerPlotPosition.X + labelsSize;
        }

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
        private void DrawChart()
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();
            chart1.Legends.Clear();

            cc.ChartArea c1 = new cc.ChartArea();
            c1.Name = "Default";
            chart1.ChartAreas.Add(c1);

            int leftoffset;
            int downoffset = 10;
            leftoffset = yval.Count() * 3 + 2;

            chart1.ChartAreas.FindByName("Default").Position = new cc.ElementPosition(leftoffset, downoffset, 100-leftoffset, 100-downoffset);
            chart1.ChartAreas.FindByName("Default").InnerPlotPosition = new cc.ElementPosition(0, 0, 90, 90);
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

            chart1.Titles.ElementAt(1).Position = new cc.ElementPosition(90, 0, 5, 5);
            chart1.Titles.ElementAt(0).Font = new Font("Arial", 20, FontStyle.Bold);

            chart1.Legends.Add(new cc.Legend("Legend1"));
            chart1.Legends.FindByName("Legend1").Position = new cc.ElementPosition(5, 0, 9, 9);

            cc.Series xserie = new cc.Series();
            xserie.ChartType = cc.SeriesChartType.Line;
            xserie.IsVisibleInLegend = false;
            xserie.Color = Color.Transparent;
            for(int i = 0; i < 11; i++)
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
        }

        private void ReadData()
        {
            //read data from file

            //-
            //value of last x added
            //if value of new x <= lastx dont add to Series, so it always creates a steady function, not a relation
            //=> x is steadily increasing in displayed function (not decreasing or staying constant)
            lastx = double.MinValue;
            //-

            if(System.IO.File.Exists(filepath))
            {
                //if file in filepath exists, read each line of file for input as data
                //1. line of file:<xAxisName> <1.SeriesName> <2.SeriesName> ... <n.SeriesName>
                //m. line of file:<m.XValue> <m.YValOf1.Series> ... <m.YValOfn.Series>
                
                //-
                //Clear Lists
                xval.Clear();
                yval.Clear();
                //-

                //-
                //count counting lines in file
                int count = 0;
                //-

                foreach (string line in System.IO.File.ReadLines(filepath))
                {
                    if(count == 0)
                    {
                        //1. line of file
                        ReadFirstLine(line);
                    }
                    else
                    {
                        ReadLine(line);
                    }
                    count++;
                }

                //-
                //for now for testing x values in interval [0, 10]
                //-----
                //TODO 
                //set through TextBoxes 
                x1 = 0;
                x2 = 10;
                //-
            }

        }

        private void ReadFirstLine(string input)
        {
            //read first line from file
            //1. line: XAxisName 1.YAxisName ... n.YAxisName

            char[] separators = {' '};

            string[] axisnames = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            Color[] colors = {Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Brown, Color.Orange, Color.Black};

            int count = 0;
            foreach(string name in axisnames)
            {
                if(count == 0)
                {
                    //first entry => XAxisName
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

        private void ReadLine(string input)
        {
            //read line from file
            //lineformat: xvalue yvalueOf1.Series ... yvalueOfN.Series

            char[] separators = {' '};

            string[] values = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            int count = 0;
            foreach(string val in values)
            {
                if(count == 0)
                {
                    xval.Add(Convert.ToDouble(val));
                }
                else
                {
                    int seriesCount = count - 1;
                    yval[seriesCount].Add(Convert.ToDouble(val));
                }
                count++;
            }
        }
    }
}
