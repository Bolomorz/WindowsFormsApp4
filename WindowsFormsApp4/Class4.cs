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
    /// <summary>
    /// Array of dynamically created controls.
    /// </summary>
    class ComponentContainer
    {
        /// <summary>
        /// 0 = lmax;
        /// 1 = lmin;
        /// 2 = lname;
        /// 3 = tbmax;
        /// 4 = tbmin;
        /// 5 = tbname;
        /// 6 = gbcomponent;
        /// </summary>
        public Control[] controls = new Control[7];

        public ComponentContainer(Label lmax, Label lmin, Label lname, TextBox tbmax, TextBox tbmin, TextBox tbname, GroupBox gbcomponent)
        {
            controls[0] = lmax;
            controls[1] = lmin;
            controls[2] = lname;
            controls[3] = tbmax;
            controls[4] = tbmin;
            controls[5] = tbname;
            controls[6] = gbcomponent;
        }
    }
}
