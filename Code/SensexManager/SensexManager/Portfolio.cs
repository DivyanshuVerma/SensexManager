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
using Oracle.DataAccess.Client;

namespace SensexManager
{
    public partial class Portfolio : Form
    {
        Form1 f1;
        String email;
        String name;
        String gender;
        String dob;

        String connstr = "";
        OracleConnection conn = null;
        OracleDataAdapter da = null;
        DataSet ds = null;
        DataTable dt = null;

        public Portfolio(Form1 f1, String name, String email, String gender, String dob)
        {
            InitializeComponent();

            this.f1 = f1;
            this.name = name;
            this.email = email;
            this.gender = gender;
            this.dob = dob;

            textBox1.Hide();
            textBox2.Hide();
            textBox3.Hide();
            button3.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            f1.Show();
            this.Close();
        }

        private void Portfolio_Load(object sender, EventArgs e)
        {
            label1.Text = name;
            label2.Text = dob;
            label3.Text = gender;
            label4.Text = email;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "Name";
            label2.Text = "Date Of Birth";
            label3.Text = "Gender";

            textBox1.Show();
            textBox2.Show();
            textBox3.Show();

            button3.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            name = textBox1.Text;
            dob = textBox2.Text;
            gender = textBox3.Text;

            label1.Text = name;
            label2.Text = dob;
            label3.Text = gender;

            connect();
            load();
            conn.Close();

            textBox1.Hide();
            textBox2.Hide();
            textBox3.Hide();
            button3.Hide();
        }

        private void load()
        {
            String str = "update login_details set name='"+name+"' where email_id='" + email + "'";
            String str2 = "update login_details set gender='" + gender + "' where email_id='" + email + "'";
            String str3 = "update login_details set dob='" + dob + "' where email_id='" + email + "'";
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = str;
            cmd.ExecuteNonQuery();
            cmd.CommandText = str2;
            cmd.ExecuteNonQuery();
            cmd.CommandText = str3;
            cmd.ExecuteNonQuery();
        }

        public void connect()
        {
            connstr = "Data Source=Divyanshu;user id=system;password=1234red";
            conn = new OracleConnection(connstr);
            conn.Open();
        }
    }
}
