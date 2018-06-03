using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a3
{
    public partial class Queue_Information : Form
    {
        List<_Servicing_Terminal> LIST_ServicingTerminals = new List<_Servicing_Terminal>();
        private String connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["dbString"].ConnectionString;
        private Timer timer1;
        public Queue_Information()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            Queue_Information_onLoad();
            refreshTerminalList();

            
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            timer1.Tick -= new EventHandler(timer1_Tick);
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
            timer1.Dispose();
            timer1 = null;
        }

        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 5000; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            refreshTerminalList();
        }
        private void Queue_Information_onLoad()
        {
            //comboBox1.Items.Clear();
            //comboBox1.DataSource = LIST_getServicingOffices();
            //comboBox1.DisplayMember = "Name";
            //comboBox1.ValueMember = "id";
        }
        private List<_Servicing_Office> LIST_getServicingOffices()
        {

            List<_Servicing_Office> dataSource = new List<_Servicing_Office>();
            // List possible Servicing Offices
            SqlConnection con = new SqlConnection(connection_string);
            string retrieve_servicing_offices = "select * from Servicing_Office";
            SqlDataReader _rdr;
            SqlCommand __cmd = new SqlCommand(retrieve_servicing_offices, con);

            try
            {
                con.Open();
                _rdr = __cmd.ExecuteReader();
                while (_rdr.Read())
                {
                    dataSource.Add(new _Servicing_Office()
                    {
                        Name = (string)_rdr["Name"],
                        Address = (string)_rdr["Address"],
                        id = (int)_rdr["ID"]
                    });
                }
                con.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Can't connect to local DB!");
                Environment.Exit(0);
            }
            return dataSource;
        }
        private void refreshTerminalList()
        {
            ListView1.Items.Clear();
            ListView1.View = View.Details;

            string Customer_Queue_Number = " ";
            string Office_Name = " ";
            string Avg_Serving_Time = " ";
            string Window = " ";
            SqlConnection con = new SqlConnection(connection_string);
            string query = "select * from Servicing_Terminal";
            SqlCommand cmd1 = new SqlCommand(query, con);
            SqlDataReader rdr1;
            con.Open();
            rdr1 = cmd1.ExecuteReader();
            while (rdr1.Read())
            {
                Customer_Queue_Number = (rdr1["Customer_Queue_Number"] != null && rdr1["Customer_Queue_Number"] != DBNull.Value) ? (string)rdr1["Customer_Queue_Number"] : "N/A";
                Office_Name = (string)rdr1["Name"];
                Window = (rdr1["Window"] != null && rdr1["Window"] != DBNull.Value) ? ((int)rdr1["Window"]).ToString(): "No one serving";
                
                string[] row = { Office_Name, Window, Customer_Queue_Number, Avg_Serving_Time };
                
                var lvi = new ListViewItem(row);
                ListView1.Items.Add(lvi);
                Console.WriteLine("add {0}", Customer_Queue_Number);
            }

            con.Close();
            
        }
        private void Queue_Information_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

    }
}
