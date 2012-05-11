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
using System.Net.Mail;
using Oracle.DataAccess.Client;

namespace SensexManager
{
    public partial class Register : Form
    {
        private Form1 form1;

        String email = "";
        String connstr = "";
        OracleConnection conn = null;
        OracleDataAdapter da = null;
        DataSet ds = null;
        DataTable dt = null;

        String code = "";

        public Register()
        {
            InitializeComponent();
        }

        public Register(Form1 form1)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.form1 = form1;
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
            String date = (string)comboBox1.SelectedItem;
            String month = (string)comboBox2.SelectedItem;
            String year = (string)comboBox3.SelectedItem;
            String gender = "";
            if (radioButton1.Checked)
                gender = "Male";
            else
                gender = "Female";
            String str = "insert into login_details values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','"+date+"-"+month+"-"+year+"','"+gender+"')";
            
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = str;
            cmd.ExecuteNonQuery();
        }

        public void load2()
        {
            connect();
            String str = "insert into user_status values('" + textBox1.Text + "','" + code + "','invalid')";
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = str;
            cmd.ExecuteNonQuery();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            form1.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("D:\\temp\\login\\login.txt",true);
            if(textBox2.Text.Equals(textBox3.Text))
                sw.Write(textBox1.Text+">>>"+textBox2.Text + "\n");
            sw.Close();

            MailMessage mailMsg = new MailMessage();
            System.Net.NetworkCredential cred = new System.Net.NetworkCredential("no.reply.Sensex.Manager@gmail.com", "qwertyuiop[]\\1234567890-=");
            mailMsg.From = new MailAddress("no.reply.Sensex.Manager@gmail.com");
            mailMsg.Subject = "Your Confirmation Code";
            mailMsg.IsBodyHtml = true;
            code = GetPassword();
            mailMsg.Body = "Thank you for registering to Sensex Manager. We intend to provide you the best of services.<br/>Your confirmation code is: <h1>"+code+"</h1><br/>Kindly copy and paste it into the software.<br/><br/>Regards,<br/>Senex Manager Team";
            mailMsg.To.Add(new MailAddress(textBox1.Text));
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = cred;
            smtpClient.Send(mailMsg);

            MsgCodeSent ms = new MsgCodeSent(form1);
            ms.Show();
            this.Close();
        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public string GetPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(8, true));
            builder.Append(RandomNumber(1000, 9999));
            return builder.ToString();
        }
    }
}
