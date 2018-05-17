using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MackMethod
{
    public partial class ProtocolForm : Form
    {
        public ProtocolForm(string protocol)
        {
            InitializeComponent();
            textBox1.Text = protocol;
            /*float[,] x = new float[5, 5] {
                {10, 5, 9, 18, 11},
                {13, 19, 6, 12, 14},
                {3, 2, 4, 4, 5},
                {18, 9, 12, 17, 15},
                {11, 6, 14, 19, 10}
            };
            Mack m = new Mack(x);
            int i = 0;
            string protocol = "";
            while (m.DoIteration())
            {
                protocol += m.iterationResult;
                i++;
            }
            textBox1.Text = protocol;*/
        }
    }
}
