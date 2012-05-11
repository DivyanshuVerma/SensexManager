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
using System.Net;
using System.IO;
using System.Threading;
using Oracle.DataAccess.Client;

namespace SensexManager
{
    public partial class Form1 : Form
    {
        String[] companyNames = { "Bajaj Auto", "Bharti Airtel", "BHEL", "Cipla", "Coal India", "DLF", "Gail India", "HDFC", "HDFC Bank", "Hero MotoCorp", "Hindalco Inds", "Hindustan Unilever", "ICICI Bank", "Infosys", "ITC", "Jindal Steel", "L&T", "Mahindra & Mahindra", "Maruti Suzuki", "NTPC", "ONGC", "RIL", "SBI", "Sterlite Inds", "Sun Pharma", "Tata Motors", "Tata Power", "Tata Steel", "TCS", "Wipro" };
        public String username = "GUEST!";
        System.Windows.Forms.Timer timer1;
        String[][] cv;
        String[] sv;
        int tickTim = 0;
        String tickText = "";
        String email = "";
        String gender="";
        String dob = "";
        Boolean work = false;

        String connstr = "";
        OracleConnection conn = null;
        OracleDataAdapter da = null;
        DataSet ds = null;
        DataTable dt = null;

        public Form1()
        {
            //Loading ll = new Loading();
            //ll.Show();
            //this.Hide();

            InitializeComponent();

            reload_data();

            label14.Text = username;
            button1.Show();
            button4.Hide();

            for (int i = 0; i < 30; i++)
                    tickText += companyNames[i] + "   ";

            textBox2.Text = tickText;

            //ll.Close();
            //this.Show();

            //Thread for reloading data
            BackgroundWorker bw = new BackgroundWorker();

            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args)
            {
                BackgroundWorker b = o as BackgroundWorker;
                while (true)
                {
                    reload_data();
                    b.ReportProgress(40);
                    Thread.Sleep(15000);
                }
            });

            bw.ProgressChanged += new ProgressChangedEventHandler(
        delegate(object o, ProgressChangedEventArgs args)
        {
            if (work)
            {
                DateTime dt = DateTime.Now;
                label15.Text = String.Format("{0:dddd, MMMM d, yyyy}", dt);

                label1.Text = sv[0];
                label2.Text = sv[1];
                label3.Text = sv[2];
                label4.Text = sv[3];
                label5.Text = sv[4];
                label6.Text = sv[5];
                label7.Text = sv[6];

                //connect();
                //load2();
                //conn.Close();

                if (!label2.Text.Equals(""))
                {
                    if (double.Parse(label2.Text) <= 0)
                    {
                        pictureBox1.Show();
                        pictureBox2.Hide();
                    }
                    else
                    {
                        pictureBox2.Show();
                        pictureBox1.Hide();
                    }
                }
                else
                    label2.Text = "0.00";

                System.Windows.Forms.ListBox.ObjectCollection obc = listBox1.Items;
                obc.Clear();

                tickText = "";
                for (int i = 0; i < 30; i++)
                {
                    String spac = " ";
                    String cn = companyNames[i];
                    for (; cn.Length < 30; cn += spac) ;
                    obc.Add(cn + cv[i][1]);//cv[i]);
                    tickText += companyNames[i] + " " + cv[i][1] + "   ";
                    //connect();
                    //load(i);
                    //conn.Close();
                }

                textBox2.Text = tickText;
            }
            });

            bw.RunWorkerAsync();

            //Timer for ticker
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 150;
            timer1.Enabled = false;

            timer1.Start();

            timer1.Tick += new EventHandler(timer_Tick1);

        }

        void timer_Tick1(object sender, EventArgs e)
        {
            int ind = 125;
            if(textBox2.Text.Length < ind)
               ind = textBox2.Text.Length;
            if (ind > 0 && tickTim < ind)
                textBox2.Text = tickText.Substring(tickTim, ind);
            else
                tickTim = 0;
            tickTim = (tickTim+1)%ind;
        }

        public void reload_data()
        {
            try
            {
                work = true;
                //String[] sv = { "0", "0", "0", "0", "0", "0", "0" };
                sv = getSensexValues();

                //String[] cv = { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", };
                cv = getCompanyValues();
                label13.Text = "Welcome";
            }
            catch (Exception ex)
            {
                //textBox2.Text = "Not connected to the internet!";
                work = false;
               // label13.Text = ex.ToString();
                //label13.Font = new Font("Arial", 10);
                //label14.Hide();
            }
        }

        public void updateUser(String email, String gender, String dob)
        {
            label14.Text = username;
            this.email = email;
            this.gender = gender;
            this.dob = dob;
            button2.Text = "Log Out";
            button1.Hide();
            button4.Show();
        }

        public void connect()
        {
            connstr = "Data Source=Divyanshu;user id=system;password=1234red";
            conn = new OracleConnection(connstr);
            conn.Open();
        }

        public void load(int i)
        {
            connect();

            DateTime dt = DateTime.Now;

            String str = "insert into stock_info values('" + cv[i][0] + "','" + String.Format("{dd-MM-yyyy HH:mm:ss}", dt) + "'," + int.Parse(cv[i][3]) + "," + int.Parse(cv[i][2]) + "," + int.Parse(cv[i][3]) + ")";

            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = str;
            cmd.ExecuteNonQuery();
        }

        public void load2()
        {
            connect();

            DateTime dt = DateTime.Now;

            String str = "insert into sensex_val values('" + String.Format("{dd-MM-yyyy HH:mm:ss}", dt) + "'," + int.Parse(sv[0]) + "," + int.Parse(sv[1]) + "," + int.Parse(sv[2]) + ")";

            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = str;
            cmd.ExecuteNonQuery();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (label14.Text.Equals("GUEST!"))
            {
                SignIn si = new SignIn(this);
                si.Show();
            }
            else
            {
                button2.Text = "SignIn";
                label14.Text = "GUEST!";
                button1.Show();
                button4.Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Register r = new Register(this);
            r.Show();
        }

        public static String[][] getCompanyValues()
        {
            downloadPage("http://www.bseindia.com/mktlive/indiceswatch_scrip.asp?iname=BSE30&sensid=30&type=sens", "D:\\temp\\temp_web.html");
            String s = returnStringFromFile("D:\\temp\\temp_web.html");

            String skey = "<td class=\"tbmain\" align=\"center\" valign=\"top\">";
            String ekey = "</td>";

            String[] v1 = extractValue(s, skey, ekey, 30);

            skey = "<td class=\"tbmain\" align=\"right\" valign=\"top\">";

            String[] v2 = extractValue(s, skey, ekey, 90);

            String[][] temp = new String[30][];

            for (int i = 0; i < int.Parse(v1[0]); i++)
            {
                temp[i] = new String[4];
                temp[i][0] = v1[i + 1];
                temp[i][1] = v2[i * 3 + 1];
                temp[i][2] = v2[i * 3 + 2];
                temp[i][3] = v2[i * 3 + 3];
            }

            return temp;
        }
        public static String[] getSensexValues()
        {
            String[] temp = new String[7];

            downloadPage("http://www.moneycontrol.com/sensex/bse/sensex-live", "D:\\temp\\temp_web.html");
            String s = returnStringFromFile("D:\\temp\\temp_web.html");

            String skey = "<div class='FL r_35'><strong>";
            String ekey = "</strong></div>";

            temp[0] = extractValue(s, skey, ekey, 1)[1];

            if (temp[0]==null || temp[0].Length < 3)
            {
                skey = "<div class='FL gr_35'><strong>";
                ekey = "</strong></div>";

                temp[0] = extractValue(s, skey, ekey, 1)[1];
            }

            skey = "<div class='FL r_20 PT10 MT3'><strong>";
            ekey = "</strong></div>";

            temp[1] = extractValue(s, skey, ekey, 1)[1];

            if (temp[1] == null || temp[1].Length < 3)
            {
                skey = "<div class='FL gr_20 PT10 MT3'><strong>";
                ekey = "</strong></div>";

                temp[1] = extractValue(s, skey, ekey, 1)[1];
            }

            skey = "<div class='FL r_15 PT10 MT3 PL5'>";
            ekey = "</div>";

            temp[2] = extractValue(s, skey, ekey, 1)[1];

            if (temp[2] == null || temp[2].Length < 3)
            {
                skey = "<div class='FL gr_15 PT10 MT3 PL5'>";
                ekey = "</div>";

                temp[2] = extractValue(s, skey, ekey, 1)[1];
            }

            skey = "<td style=\"text-align:left\" align=\"center\" class=\"br01\">";
            ekey = "</td>";

            temp[3] = extractValue(s, skey, ekey, 1)[1];

            skey = "<td align=\"center\" class=\"br01\">";
            ekey = "</td>";

            temp[4] = extractValue(s, skey, ekey, 2)[1];
            temp[5] = extractValue(s, skey, ekey, 2)[2];

            skey = "<td align=\"center\">";
            ekey = "</td>";

            temp[6] = extractValue(s, skey, ekey, 1)[1];

            return temp;
        }
        public static String[] extractValue(String source, String stkey, String endkey, int limit)
        {
            String[] temp = new String[limit + 1];
            int top = 0;
            for (int i = 0, j = 0; i < source.Length && j < limit; j++)
            {
                int stind = source.IndexOf(stkey, i);
                if (stind == -1)
                    break;
                int diff = source.IndexOf(endkey, stind + 2) - stind - stkey.Length;
                String extract = source.Substring(stind + stkey.Length, diff);
                temp[++top] = extract;
                i = stind + stkey.Length + 3;
            }
            temp[0] = top.ToString();
            return temp;
        }
        public static Boolean downloadPage(String url, String path)
        {
            try
            {
                WebClient client = new WebClient();
                client.DownloadFile(url, path);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public static String returnStringFromFile(String path)
        {
            try
            {
                StreamReader sr = new StreamReader(path);
                String temp = sr.ReadToEnd();
                sr.Close();
                return temp;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Portfolio p = new Portfolio(this, label14.Text, email, gender, dob);
            this.Hide();
            p.Show();
        }

        /*private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.MouseDoubleClick += new MouseEventHandler(listBox1_MouseDoubleClick);
        }*/

        /*void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            StockInfo sin = new StockInfo();
            for (int i = 0; i < 30; i++)
                if (listBox1.GetSelected(i))
                    sin.Show();

        }*/

    }
}
