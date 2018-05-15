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
    public partial class ViewOfficeTransaction : Form
    {
        private String connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["dbString"].ConnectionString;

        public ViewOfficeTransaction()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            initItems();
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
        private List<_Transaction_Type> LIST_getTransactionType()
        {

            List<_Transaction_Type> dataSource = new List<_Transaction_Type>();
            // List possible Servicing Offices
            SqlConnection con = new SqlConnection(connection_string);
            string retrieve_servicing_offices = "select * from Transaction_Type";
            SqlDataReader _rdr;
            SqlCommand __cmd = new SqlCommand(retrieve_servicing_offices, con);

            try
            {
                con.Open();
                _rdr = __cmd.ExecuteReader();
                while (_rdr.Read())
                {
                    dataSource.Add(new _Transaction_Type()
                    {
                        Transaction_Name = (string)_rdr["Transaction_Name"],
                        id = (int)_rdr["ID"]
                    });
                }
                con.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Can't connect to local DB!");
            }
            Console.WriteLine("returned datasource");
            return dataSource;
        }
        public void initItems()
        {
            comboBox1.DataSource = LIST_getServicingOffices();
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "id";
            comboBox2.DataSource = LIST_getTransactionType();
            comboBox2.DisplayMember = "Transaction_Name";
            comboBox2.ValueMember = "id";
        }
        private List<_Transaction_List> LIST_RetrieveTransactionOffices(int id)
        {
            List<_Transaction_List> a = new List<_Transaction_List>();
            SqlConnection con = new SqlConnection(connection_string);
            string query = "select * from Transaction_List where Transaction_ID = @param1";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@param1", id);
            SqlDataReader rdr;
            con.Open();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                a.Add(new _Transaction_List
                {
                    showPattern = "#"+rdr["Pattern_No"].ToString() + " "+(string)rdr["Servicing_Office_Name"],
                    Pattern_No = (int)rdr["Pattern_No"]
                });
            }
            con.Close();
            return a;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(connection_string);
            string query = "select * from Servicing_Office where id = @param1";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader rdr;
            cmd.Parameters.AddWithValue("@param1", (int)comboBox1.SelectedValue);
            con.Open();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                label10.Text = rdr["id"].ToString();
                label11.Text = (string)rdr["Name"];
                label12.Text = (string)rdr["Address"];
            }
            con.Close();
            initItems();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue != null)
            {
                SqlConnection con = new SqlConnection(connection_string);
                string query = "select * from Transaction_Type where id = @param1";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader rdr;
                cmd.Parameters.AddWithValue("@param1", (int)comboBox2.SelectedValue);
                con.Open();
                rdr = cmd.ExecuteReader();
                int id = 0;
                while (rdr.Read())
                {
                    label16.Text = rdr["id"].ToString();
                    label15.Text = (string)rdr["Transaction_Name"];
                    label14.Text = (string)rdr["Description"];
                    id = (int)rdr["id"];
                }
                listBox1.DataSource = LIST_RetrieveTransactionOffices(id);
                listBox1.DisplayMember = "showPattern";
                listBox1.ValueMember = "Pattern_No";
                con.Close();
                initItems();
            }
        }
    }
}
