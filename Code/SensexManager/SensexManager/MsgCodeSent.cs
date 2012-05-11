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
    public partial class MsgCodeSent : Form
    {
        Form1 f1;
        public MsgCodeSent(Form1 f1)
        {
            InitializeComponent();
            this.f1 = f1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f1.Show();
            this.Close();
        }
    }
}
