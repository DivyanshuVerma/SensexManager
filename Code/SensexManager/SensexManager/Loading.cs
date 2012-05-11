/*
//	Written by Divyanshu Verma
//	dev.verma1010@gmail.com
//	www.github.com/DivyanshuVerma
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SensexManager
{
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
            Timer timer = new Timer();
            timer.Interval = 500;
            timer.Enabled = false;

            timer.Start();

            timer.Tick += new EventHandler( timer_Tick );


        }
        void timer_Tick( object sender, EventArgs e )
        {
            /*String dash = ".....";
            i = (i+1)%5;
            dash = dash.Substring(0,i);
            label1.Text = ext + dash;*/
        }
    }
}
