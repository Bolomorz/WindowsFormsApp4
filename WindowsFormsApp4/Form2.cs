using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form2 : Form
    {
        YSeries series;
        Form1 parent;
        public Form2(YSeries ser, Form1 par)
        {
            InitializeComponent();
            this.series = ser;
            parent = par;
        }

        /// <summary>
        /// On FormLoadEvent print seriesname and print values in TextBoxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_Load(object sender, EventArgs e)
        {
            label3.Text = "Current Series: " + series.name;
            textBox1.Text = series.setmax.ToString();
            textBox2.Text = series.setmin.ToString();
            if(series.active)
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
        }

        /// <summary>
        /// On ButtonClickEvent reset max value to default value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            series.ResetSetMax();
            textBox1.Text = series.setmax.ToString();
        }

        /// <summary>
        /// On ButtonClickEvent reset min value to default value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            series.ResetSetMin();
            textBox2.Text = series.setmin.ToString();
        }

        /// <summary>
        /// On ButtonClickEvent discard changes and close form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// On ButtonClickEvent accept changes and execute DrawGame with new values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            double max, min;

            if(textBox1.Text.Trim() != "")
            {
                max = parent.ReadString(textBox1.Text);
            }
            else
            {
                max = series.max;
            }

            if(textBox2.Text.Trim() != "")
            {
                min = parent.ReadString(textBox2.Text);
            }
            else
            {
                min = series.min;
            }

            if(max > min)
            {
                series.ChangeSetMax(max);
                series.ChangeSetMin(min);
            }
            else
            {
                min = max - max / 10;
                series.ChangeSetMax(max);
                series.ChangeSetMin(min);
            }

            parent.DrawChart();
            this.Close();
        }

        /// <summary>
        /// On CheckBoxCheckedChangedEvent set series active or inaktive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                series.active = true;
            }
            else
            {
                series.active = false;
            }
        }
    }
}
