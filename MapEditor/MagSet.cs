using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MapEditor
{
    public partial class MagSet : Form
    {
        public int MagMax=1;
        public int MagSize=1;
        public MagSet(int Max, int Size)
        {
            InitializeComponent();
            MagMax = Max;
            MagSize = Size;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            MagMax = hScrollBar1.Value;
            label3.Text = hScrollBar1.Value.ToString();
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            MagSize = hScrollBar2.Value;
            label4.Text = hScrollBar2.Value.ToString();
        }

        private void MagSet_Shown(object sender, EventArgs e)
        {
            if (MagMax > 0 && MagSize > 0)
            {
                hScrollBar1.Value = MagMax;
                label3.Text = MagMax.ToString();
                hScrollBar2.Value = MagSize;
                label4.Text = MagSize.ToString();
            }
        }
    }

}