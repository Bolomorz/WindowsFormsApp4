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
using excel = Microsoft.Office.Interop.Excel;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public List<double> xval = new List<double>();
        public List<YSeries> yval = new List<YSeries>();

        public int x1, x2, defx1, defx2;

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
        public Button SaveFileButton;
        public Label ChartTitleLabel;
        public TextBox ChartTitleTextBox;
        public CheckBox EnableGridCheckBox;


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
        public static double ReadString(string val)
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

            if(str != "")
            {
                ret = double.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint);
            }
            else
            {
                ret = 0;
            }

            return ret;
        }

        /// <summary>
        /// Test, if char is numeric.
        /// Returns true, if input char is numeric.
        /// </summary>
        /// <param name="val">input char</param>
        /// <returns></returns>
        public static bool IsNumeric(char val)
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
            chartTitle = "<insert title here>";
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
            fd.Title = "Open File";
            fd.InitialDirectory = @"C:\Users\dominik.schneider\Documents\YAxisAppFiles\";
            fd.Filter = "TXT|*.txt|XLSX|*.xlsx";
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
        /// Create data points for YSeries.
        /// Find closest xvalue in xval to integers in [x1, x2]
        /// </summary>
        /// <param name="series"></param>
        private void CreateDataPoints(
            cc.Series series)
        {
            int index = 0;
            int c = series.Points.Count();

            for(double i = x1; i <= x2; i+=0.5)
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
                series.Points.ElementAt(index).ToolTip = "#SERIESNAME : X=#VALX, Y=#VALY";
            }
        }

        /// <summary>
        /// Draw chart with values of currently active YSeries
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
            Controls.Remove(SaveFileButton);
            Controls.Remove(ChartTitleLabel);
            Controls.Remove(ChartTitleTextBox);
            Controls.Remove(EnableGridCheckBox);

            cc.ChartArea c1 = new cc.ChartArea();
            c1.Name = "Default";
            chart1.ChartAreas.Add(c1);

            int leftoffset;
            int downoffset = 10;
            int count = 0;
            foreach(YSeries yseries in yval)
            {
                if(yseries.active)
                {
                    count++;
                }
            }
            leftoffset = count * 3 + 2;

            chart1.ChartAreas.FindByName("Default").Position = new cc.ElementPosition(leftoffset, downoffset, 100-leftoffset, 100-downoffset);
            chart1.ChartAreas.FindByName("Default").InnerPlotPosition = new cc.ElementPosition(0, 0, 95, 90);
            chart1.ChartAreas.FindByName("Default").AxisX.MajorGrid.Enabled = gridEnabled;
            chart1.ChartAreas.FindByName("Default").AxisX.MinorGrid.Enabled = gridEnabled;
            chart1.ChartAreas.FindByName("Default").AxisY.MajorGrid.Enabled = gridEnabled;
            chart1.ChartAreas.FindByName("Default").AxisY.MinorGrid.Enabled = gridEnabled;
            chart1.ChartAreas.FindByName("Default").AxisX.Interval = (double)(x2 - x1) / 20.0;
            chart1.ChartAreas.FindByName("Default").AxisY.Interval = (double)(x2 - x1) / 20.0;
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
                if(yseries.active)
                {
                    CreateYAxis(chart1, chart1.ChartAreas.FindByName("Default"), yseries, offset, 4);
                    offset += 3;
                }
            }

            if(xval.Count > 0)
            {
                foreach(YSeries yseries in yval)
                {
                    if(yseries.active)
                    {
                        CreateDataPoints(chart1.Series.FindByName(yseries.name));
                    }
                }
            }

            CreateControls();
        }

        /// <summary>
        /// Read data from file in filepath.
        /// </summary>
        private void ReadData()
        {
            lastx = double.MinValue;

            bool exists = File.Exists(filepath);
            string extension = Path.GetExtension(filepath);

            if(exists && extension == ".txt")
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

                defx1 = (int)Math.Floor(xval[0]);
                defx2 = (int)Math.Ceiling(xval[xval.Count-1]);
                x1 = defx1;
                x2 = defx2;
            }
            else if(exists && extension == ".xlsx")
            {
                xval.Clear();
                yval.Clear();

                ReadXlsx();

                defx1 = (int)Math.Floor(xval[0]);
                defx2 = (int)Math.Ceiling(xval[xval.Count - 1]);
                x1 = defx1;
                x2 = defx2;
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

            Color[] colors = {Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Yellow, Color.Black};

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
                    if(seriesCount < colors.Length-1)
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
        /// On ButtonClickEvent -> Open Form4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Form4 child = new Form4();
            child.Show();
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

            double xvalue = Form1.ReadString(values[0]);
            if(xvalue > lastx)
            {
                xval.Add(xvalue);
                for(int i = 1; i < values.Length; i++)
                {
                    yval[i - 1].Add(ReadString(values[i]));
                }
                lastx = xvalue;
            }
        }

        /// <summary>
        /// Create Button for each YSeries in yval.
        /// Create Label and TextBox for ChartTitle input.
        /// Create CheckBox to enable/disable grid.
        /// </summary>
        private void CreateControls()
        {
            int x = 80; int y = 110;

            Button xb = new Button();
            xb.Location = new Point(x, y);
            x += 100;
            xb.Height = 40;
            xb.Width = 100;
            xb.BackColor = Color.Gainsboro;
            xb.ForeColor = Color.Black;
            xb.Text = "X: " + '\n' + xvalname;
            xb.Name = "X: " + '\n' + xvalname;
            xb.Font = new Font("Arial", 9);
            xb.Click += new EventHandler(XButtonClick);
            Controls.Add(xb);
            buttons.Add(xb);

            for(int i = 0; i < yval.Count(); i++)
            {
                Button nb = new Button();
                nb.Location = new Point(x, y);
                x += 100;
                nb.Height = 40;
                nb.Width = 100;
                nb.BackColor = Color.Gainsboro;
                nb.ForeColor = Color.Black;
                nb.Text = "Y" + (i+1) + ": " + '\n' + yval[i].name;
                nb.Name = "Y" + (i+1) + ": " + '\n' + yval[i].name;
                nb.Tag = i;
                nb.Font = new Font("Arial", 9);
                nb.Click += new EventHandler(YButtonClick);
                Controls.Add(nb);
                buttons.Add(nb);
            }

            ChartTitleLabel = new Label();
            ChartTitleLabel.Location = new Point(80, 83);
            ChartTitleLabel.Text = "Set ChartTitle: ";
            ChartTitleLabel.Size = new Size(90, 20);
            ChartTitleLabel.Font = new Font("Arial", 9);
            Controls.Add(ChartTitleLabel);

            ChartTitleTextBox = new TextBox();
            ChartTitleTextBox.Location = new Point(170, 80);
            ChartTitleTextBox.Size = new Size(500, 20);
            ChartTitleTextBox.Text = chartTitle;
            ChartTitleTextBox.Font = new Font("Arial", 9);
            ChartTitleTextBox.TextChanged += new EventHandler(ChartTitleTextBoxTextChanged);
            Controls.Add(ChartTitleTextBox);

            EnableGridCheckBox = new CheckBox();
            EnableGridCheckBox.Location = new Point(700, 83);
            EnableGridCheckBox.Size = new Size(200, 20);
            EnableGridCheckBox.Checked = gridEnabled;
            EnableGridCheckBox.Text = "enable/disable grid";
            EnableGridCheckBox.Font = new Font("Arial", 9);
            EnableGridCheckBox.CheckedChanged += new EventHandler(EnableGridCheckBoxCheckedChanged);
            Controls.Add(EnableGridCheckBox);

            SaveFileButton = new Button();
            SaveFileButton.Location = new Point(5, 80);
            SaveFileButton.Text = "Save File";
            SaveFileButton.Size = new Size(70, 70);
            SaveFileButton.Font = new Font("Arial", 9);
            SaveFileButton.Click += new EventHandler(SaveFileButtonClick);
            Controls.Add(SaveFileButton);
        }

        /// <summary>
        /// On ButtonClickEvent -> Open Form2 with according YSeries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YButtonClick(object sender, EventArgs e)
        {
            Button nbc = (Button)sender;
            int index = (int)nbc.Tag;
            Form2 child = new Form2(yval[index], this);
            child.Show();
        }

        /// <summary>
        /// On ButtonClickEvent -> Open Form3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XButtonClick(object sender, EventArgs e)
        {
            Form3 child = new Form3(this);
            child.Show();
        }

        /// <summary>
        /// On TextBoxTextChangedEvent print text to chart.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartTitleTextBoxTextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            chartTitle = tb.Text;
            chart1.Titles.ElementAt(0).Text = chartTitle;
        }

        /// <summary>
        /// On CheckBoxCheckedChangedEvent set gridEnabled to CheckBox.Checked and draw chart.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableGridCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if(cb.Checked)
            {
                gridEnabled = true;
            }
            else
            {
                gridEnabled = false;
            }
            DrawChart();
        }

        /// <summary>
        /// On ButtonClickEvent -> Open SaveFileDialog to save file as .png
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveFileButtonClick(object sender, EventArgs e)
        {
            string path = "";

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save File";
            sfd.InitialDirectory = @"C:\Users\dominik.schneider\Documents\YAxisAppFiles\";
            sfd.Filter = "All files (*.png) | *.png";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            sfd.DefaultExt = ".png";
            sfd.FileName = chartTitle;

            if(sfd.ShowDialog() == DialogResult.OK)
            {
                path = sfd.FileName;
                chart1.SaveImage(path, cc.ChartImageFormat.Png);
                chart1.Printing.Print(true);
                MessageBox.Show("File was successfully saved in <" + path + ">", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ReadXlsx()
        {
            ExcelReaderWriter reader = new ExcelReaderWriter(filepath);

            xvalname = reader.ReadCell(2, 2).Item1;

            bool cont = true;

            int col = 3;

            while (cont)
            {
                Tuple<string, Color> tuple = reader.ReadCell(2, col);
                if(tuple.Item1 != "")
                {
                    yval.Add(new YSeries(tuple.Item1, tuple.Item2));
                }
                else
                {
                    cont = false;
                }
            }

            cont = true;

            int row = 3;

            while(cont)
            {
                if(reader.ReadCell(row, 1).Item1 != "")
                {
                    double xvalue = Form1.ReadString(reader.ReadCell(row, 2).Item1);
                    if(xvalue > lastx)
                    {
                        xval.Add(xvalue);
                        lastx = xvalue;

                        int column = 3;
                        int i = 0;
                        bool cont2 = true;
                        while(cont2)
                        {
                            Tuple<string, Color> tuple = reader.ReadCell(row, column);
                            if(tuple.Item1 != "")
                            {
                                yval[i].values.Add(Form1.ReadString(tuple.Item1));
                            }
                            else
                            {
                                cont2 = false;
                            }

                            column++;
                            i++;
                        }

                    }
                    
                }
                else
                {
                    cont = false;
                }

                row++;
            }

            reader.SaveChanges();
            reader.Quit();
        }
    }
}
