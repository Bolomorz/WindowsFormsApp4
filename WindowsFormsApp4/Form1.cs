using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using cc = System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public List<double> xval = new List<double>();
        public List<YSeries> yval = new List<YSeries>();

        public int minx, maxx;
        public int x1, x2;

        public bool gridEnabled;

        public string xvalname;
        public string chartTitle, chartDate;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gridEnabled = false;
            chartDate = DateTime.Now.ToString("d");
        }

        private void CreateYAxis(
            cc.Chart chart, 
            cc.ChartArea area,
            cc.Series series,
            float axisOffset, float labelsSize)
        {
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

            //TODO AXISY.MAX/MIN

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
                seriesCopy.Points.AddXY(point.XValue, point.YValues);
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

            //TODO AXISX.MAX/MIN, AXISY.MAX/MIN

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

            chart1.ChartAreas.FindByName("Default").Position = new cc.ElementPosition(20, 10, 80, 90);
            chart1.ChartAreas.FindByName("Default").InnerPlotPosition = new cc.ElementPosition(0, 0, 90, 90);
            chart1.ChartAreas.FindByName("Default").AxisX.MajorGrid.Enabled = gridEnabled;
            chart1.ChartAreas.FindByName("Default").AxisX.MinorGrid.Enabled = gridEnabled;
            chart1.ChartAreas.FindByName("Default").AxisY.MajorGrid.Enabled = gridEnabled;
            chart1.ChartAreas.FindByName("Default").AxisY.MinorGrid.Enabled = gridEnabled;
            chart1.ChartAreas.FindByName("Default").AxisX.Interval = (x2 - x1) / 10;
            chart1.ChartAreas.FindByName("Default").AxisY.Interval = (x2 - x1) / 10;
            chart1.ChartAreas.FindByName("Default").AxisX.Minimum = x1;
            chart1.ChartAreas.FindByName("Default").AxisX.Maximum = x2;
            chart1.ChartAreas.FindByName("Default").AxisX.IsStartedFromZero = true;
            chart1.ChartAreas.FindByName("Default").AxisX.Title = xvalname;

            chart1.Titles.Add(chartTitle);
            chart1.Titles.Add(chartDate);

            chart1.Titles.FindByName(chartDate).Position = new cc.ElementPosition(95, 0, 5, 5);
            chart1.Titles.FindByName(chartTitle).Font = new Font("Arial", 20, FontStyle.Bold);

            chart1.Legends.Add("Legend1");
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

            foreach(YSeries y in yval)
            {
                cc.Series si = new cc.Series();
                si.Name = y.name;
                for(int i = 0; i < y.values.Count(); i++)
                {
                    if(xval.ElementAt(i) >= x1 && xval.ElementAt(i) <= x2)
                    {
                        si.Points.AddXY(xval.ElementAt(i), y.values.ElementAt(i));
                    }
                }
                si.Color = y.color;
                si.ChartType = cc.SeriesChartType.Line;
                si.BorderWidth = 2;
                chart1.Series.Add(si);
            }

            float offset = 0;
            for(int i = 1; i <= chart1.Series.Count(); i++)
            {
                CreateYAxis(chart1, chart1.ChartAreas.FindByName("Default"), chart1.Series.ElementAt(i), offset, 4);
                offset += 4;
            }

            if(xval.Count > 0)
            {
                for(int i = 1; i < chart1.Series.Count(); i++)
                {
                    CreateDataPoints(chart1.Series.ElementAt(i));
                }
            }
        }
    }
}
