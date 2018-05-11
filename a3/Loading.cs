using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a3
{
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
        }
        public void Increment(int value)
        {
            progressBar1.Increment(value);
        }
        public void setMax(int max)
        {
            progressBar1.Maximum = max;
        }
        public void reset()
        {
            //progressBar1.Value = 0;
            //label1.Text = "Init done";
        }
        public void setText(string text)
        {
            label1.Text = text;
        }
        
    
    }
}
