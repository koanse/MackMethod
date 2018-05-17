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
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            webBrowser1.DocumentText = Properties.Resources.Help;
        }
    }
}
