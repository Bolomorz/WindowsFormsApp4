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
    public partial class Form3 : Form
    {
        Form1 parent;
        public Form3(Form1 par)
        {
            InitializeComponent();
            parent = par;
        }

        /// <summary>
        /// On FormLoadEvent print values to TextBoxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form3_Load(object sender, EventArgs e)
        {
            textBox1.Text = parent.x2.ToString();
            textBox2.Text = parent.x1.ToString();
        }

        /// <summary>
        /// On ButtonClickEvent reset x2 value to default value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            parent.x2 = parent.defx2;
            textBox1.Text = parent.x2.ToString();
        }

        /// <summary>
        /// On ButtonClickEvent reset x1 value to default value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            parent.x1 = parent.defx1;
            textBox2.Text = parent.x1.ToString();
        }

        /// <summary>
        /// On ButtonClickEvent accept changes and set values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            int min, max;

            if(textBox1.Text.Trim() != "")
            {
                max = Form4.ReadString(textBox1.Text);
            }
            else
            {
                max = parent.defx2;
            }

            if(textBox2.Text.Trim() != "")
            {
                min = Form4.ReadString(textBox2.Text);
            }
            else
            {
                min = parent.defx1;
            }

            if(min > max)
            {
                min = max - 1;
            }

            parent.x1 = min;
            parent.x2 = max;
            parent.DrawChart();

            this.Close();
        }

        /// <summary>
        /// On ButtonClickEvent discard changes and close Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
