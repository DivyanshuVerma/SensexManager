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
using System.IO;
using System.Threading;
using Oracle.DataAccess.Client;

namespace SensexManager
{
    public partial class SignIn : Form
    {
        private Form1 form1 = null;

        String connstr = "";
        OracleConnection conn = null;
        OracleDataAdapter da = null;
        DataSet ds = null;
        DataTable dt = null;

        Boolean code = false;
        Boolean password = false;
        String uname = "USER";
        String gender = "";
        String dob = "";

        public SignIn()
        {
            InitializeComponent();
        }

        public SignIn(Form1 form1)
        {
            // TODO: Complete member initialization
            this.form1 = form1;
            InitializeComponent();
            form1.Hide();
        }

        public void connect()
        {
            connstr = "Data Source=Divyanshu;user id=system;password=1234red";
            conn = new OracleConnection(connstr);
            conn.Open();
        }

        public void load()
        {
            connect();
            String str = "select password,fname,gender,dob from login_details where email_id='"+textBox1.Text+"'";

            da = new OracleDataAdapter(str, conn);
            ds = new DataSet();
            da.Fill(ds, "login_details");

            dt = ds.Tables[0];
            String pass = (dt.Rows[0]["password"]).ToString();
            uname = (dt.Rows[0]["fname"]).ToString();
            gender = (dt.Rows[0]["gender"]).ToString();
            dob = (dt.Rows[0]["dob"]).ToString();
            if (pass.Equals(textBox2.Text))
                password = true;
            else
                password = false;
        }

        public void load2()
        {
            connect();
            String str = "select status from user_status where email_id='" + textBox1.Text + "'";

            da = new OracleDataAdapter(str, conn);
            ds = new DataSet();
            da.Fill(ds, "user_status");
            dt = ds.Tables[0];

            String status = (dt.Rows[0]["status"]).ToString();
            if (status.Equals("valid"))
                code = true;
            else
                code = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            form1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            load();
            conn.Close();
            load2();
            conn.Close();

            if (password)
            {
                if (code)
                {
                    form1.username = uname;
                    form1.updateUser(textBox1.Text,gender, dob);
                    form1.Show();
                    this.Close();
                }
                else
                    label4.Text = "Click on \"Enter Code\" for verifying your code.";
            }
            else
                label4.Text = "Sorry, the details don't match";
        }

        public void updatelabel(String txt)
        {
            label4.Text = txt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ValidateCode vc = new ValidateCode(this);
            this.Hide();
            vc.Show();
        }
    }
}
