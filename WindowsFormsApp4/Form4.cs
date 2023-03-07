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
    public partial class Form4 : Form
    {
        /// <summary>
        /// List of ComponentContainers with dynamically created controls,
        /// </summary>
        List<ComponentContainer> containers = new List<ComponentContainer>();
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        /// <summary>
        /// Converts String into Integer, deletes NonNumerical Characters.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ReadString(string input)
        {
            int ret = 0;
            string str = "";

            foreach(char c in input)
            {
                if(Form1.IsNumeric(c))
                {
                    str += c;
                }
            }

            if(str != "")
            {
                ret = Convert.ToInt32(str);
            }
            else
            {
                ret = 0;
            }

            return ret;
        }

        /// <summary>
        /// On TextBoxTextChangedEvent convert text in TextBox into integer i.
        /// Create i + 1 ComponentContainer. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int count = ReadString(textBox1.Text);

            if(count > 0)
            {
                foreach (ComponentContainer c in containers)
                {
                    RemoveFromControls(c);
                }
                containers.Clear();

                this.Size = new Size(450, (50 + (3) * 110 + 20));

                containers.Add(CreateComponentContainer("XValues", 30, 50));

                int y = 150;
                for(int i = 0; i < count; i++)
                {
                    containers.Add(CreateComponentContainer("YSeries " + (i + 1), 30, y));
                    y += 100;
                }
            }
        }

        /// <summary>
        /// Create ComponentContainer and add controls to controlslist.
        /// </summary>
        /// <param name="cname"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private ComponentContainer CreateComponentContainer(string cname, int x, int y)
        {
            GroupBox gb = new GroupBox();
            gb.Text = cname;
            gb.Location = new Point(x, y);
            gb.Size = new Size(350, 100);

            Label lname = new Label();
            lname.Text = "Name:";
            lname.Location = new Point(x + 15, y + 15);
            lname.Size = new Size(50, 20);
            lname.Visible = true;
            lname.BringToFront();
            TextBox tbname = new TextBox();
            tbname.Text = "";
            tbname.Location = new Point(x + 100, y + 15);
            tbname.Size = new Size(200, 20);
            tbname.BringToFront();

            Label lmin = new Label();
            lmin.Text = "Min Value:";
            lmin.Location = new Point(x + 15, y + 45);
            lmin.BringToFront();
            TextBox tbmin = new TextBox();
            tbmin.Text = "";
            tbmin.Location = new Point(x + 100, y + 45);
            tbmin.Size = new Size(200, 20);
            tbmin.BringToFront();

            Label lmax = new Label();
            lmax.Text = "Max Value:";
            lmax.Location = new Point(x + 15, y + 75);
            lmax.BringToFront();
            TextBox tbmax = new TextBox();
            tbmax.Text = "";
            tbmax.Location = new Point(x + 100, y + 75);
            tbmax.Size = new Size(200, 20);
            tbmax.BringToFront();

            Controls.Add(gb);
            Controls.Add(lname);
            Controls.Add(tbname);
            Controls.Add(tbmin);
            Controls.Add(tbmax);
            Controls.Add(lmin);
            Controls.Add(lmax);

            gb.SendToBack();

            ComponentContainer container = new ComponentContainer(lmax, lmin, lname, tbmax, tbmin, tbname, gb);

            return container;
        }

        /// <summary>
        /// Remove controls in ComponentContainer from controlslist.
        /// </summary>
        /// <param name="container"></param>
        private void RemoveFromControls(ComponentContainer container)
        {
            foreach(Control c in container.controls)
            {
                Controls.Remove(c);
            }
        }

        /// <summary>
        /// On ButtonClickEvent create FileGenerator with input from controls in ComponentContainerList.
        /// Generate file (.txt) and close Form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if(ReadString(textBox1.Text) == 0)
            {
                FileGenerator fg = new FileGenerator();

                string path = fg.GenerateFile(".txt");

                string message = "The file was successfully saved in <" + path + ">.";
                MessageBox.Show(message, "File saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                FileGenerator fg = new FileGenerator(containers.Count - 1);

                for(int i = 0; i < containers.Count; i++)
                {
                    if(i == 0)
                    {
                        fg.XAxis.name = containers[i].controls[5].Text.Replace(' ', '_');
                        fg.XAxis.min = ReadString(containers[i].controls[4].Text);
                        fg.XAxis.max = ReadString(containers[i].controls[3].Text);
                    }
                    else
                    {
                        fg.YAxes[i - 1].name = containers[i].controls[5].Text.Replace(' ', '_');
                        fg.YAxes[i - 1].min = ReadString(containers[i].controls[4].Text);
                        fg.YAxes[i - 1].max = ReadString(containers[i].controls[3].Text);
                    }
                }

                string path = fg.GenerateFile(".txt");

                string message = "The file was successfully saved in <" + path + ">.";
                MessageBox.Show(message, "File saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.Close();
        }

        /// <summary>
        /// On ButtonClickEvent create FileGenerator with input from controls in ComponentContainerList.
        /// Generate file (.xlsx) and close Form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (ReadString(textBox1.Text) == 0)
            {
                FileGenerator fg = new FileGenerator();

                string path = fg.GenerateFile(".xlsx");

                string message = "The file was successfully saved in <" + path + ">.";
                MessageBox.Show(message, "File saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                FileGenerator fg = new FileGenerator(containers.Count - 1);

                for (int i = 0; i < containers.Count; i++)
                {
                    if (i == 0)
                    {
                        fg.XAxis.name = containers[i].controls[5].Text.Replace(' ', '_');
                        fg.XAxis.min = ReadString(containers[i].controls[4].Text);
                        fg.XAxis.max = ReadString(containers[i].controls[3].Text);
                    }
                    else
                    {
                        fg.YAxes[i - 1].name = containers[i].controls[5].Text.Replace(' ', '_');
                        fg.YAxes[i - 1].min = ReadString(containers[i].controls[4].Text);
                        fg.YAxes[i - 1].max = ReadString(containers[i].controls[3].Text);
                    }
                }

                string path = fg.GenerateFile(".xlsx");

                string message = "The file was successfully saved in <" + path + ">.";
                MessageBox.Show(message, "File saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.Close();
        }
    }
}
