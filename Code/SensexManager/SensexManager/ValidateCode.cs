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
    public partial class ValidateCode : Form
    {
        SignIn si;

        String connstr = "";
        OracleConnection conn = null;
        OracleDataAdapter da = null;
        DataSet ds = null;
        DataTable dt = null;

        Boolean chk = false;
        public ValidateCode(SignIn si)
        {
            this.si = si;
            InitializeComponent();
        }

        public void connect()
        {
            connstr = "Data Source=Divyanshu;user id=system;password=1234red";
            conn = new OracleConnection(connstr);
            conn.Open();
        }

        public void load2()
        {
            connect();
            String str = "select code from user_status where email_id='" + textBox1.Text + "'";

            da = new OracleDataAdapter(str, conn);
            ds = new DataSet();
            da.Fill(ds, "user_status");
            dt = ds.Tables[0];

            String code = (dt.Rows[0]["code"]).ToString();

            if (code.Equals(textBox2.Text))
                chk = true;
            else
                chk = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            si.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            load2();
            String str = "update user_status set status='valid' where email_id='" + textBox1.Text + "'";
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = str;
            cmd.ExecuteNonQuery();

            this.Hide();
            si.Show();
            si.updatelabel("Code Confirmed.");
            this.Close();
        }
    }
}
