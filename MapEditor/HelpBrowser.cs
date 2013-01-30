using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MapEditor
{
    public partial class HelpBrowser : Form
    {
        public HelpBrowser()
        {
            InitializeComponent();
            webBrowser1.Navigate("http://www.noxhub.net/wiki/tiki-index.php");
        }

        private void HelpBrowser_Load(object sender, EventArgs e)
        {
            
        }
    }
}