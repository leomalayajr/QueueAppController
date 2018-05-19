using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a3
{
    public partial class Logs : Form
    {
        private String connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["dbString"].ConnectionString;
        DataTable _b = new DataTable();
        List<__Log> lists = new List<__Log>();
        public Logs()
        {
            InitializeComponent();
            generateItems();
            refreshTerminalList();
            StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
        }
        private void refreshTerminalList()
        {
            listView1.Items.Clear();
            listView1.View = View.Details;

            string Servicing_Office_Name = " ";
            string ServeDate = " ";
            string Transaction_Name = " ";
            string _Type = " ";
            SqlConnection con = new SqlConnection(connection_string);
            string query = "select * from Log order by ServeDate desc";
            SqlCommand cmd1 = new SqlCommand(query, con);
            SqlDataReader rdr1;
            con.Open();
            rdr1 = cmd1.ExecuteReader();
            while (rdr1.Read())
            {
                Servicing_Office_Name = rdr1["Servicing_Office_Name"].ToString();
                ServeDate = rdr1["ServeDate"].ToString();
                Transaction_Name = rdr1["Transaction_Name"].ToString();
                _Type = rdr1["Type"].ToString();
                string[] row = { Servicing_Office_Name, ServeDate, Transaction_Name, _Type };

                var lvi = new ListViewItem(row);
                listView1.Items.Add(lvi);
                lists.Add(new __Log
                {
                    Servicing_Office_Name = Servicing_Office_Name,
                    ServeDate = ServeDate,
                    Transaction_Name = Transaction_Name,
                    Type = _Type
                });
            }

            con.Close();

        }
        private List<_Servicing_Office> LIST_getServicingOffices()
        {

            List<_Servicing_Office> dataSource = new List<_Servicing_Office>();
            // List possible Servicing Offices
            SqlConnection con = new SqlConnection(connection_string);
            string retrieve_servicing_offices = "select * from Servicing_Office";
            SqlDataReader _rdr;
            SqlCommand __cmd = new SqlCommand(retrieve_servicing_offices, con);
            dataSource.Add(new _Servicing_Office()
            {
                Name = "ALL",
                Address = "Everywhere",
                id = -1
            });
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

            dataSource.Add(new _Transaction_Type()
            {
                Transaction_Name = "ALL",
                id = -1
            });
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
        public void generateItems()
        {
            comboBox1.DataSource = LIST_getServicingOffices();
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "id";
            comboBox2.DataSource = LIST_getTransactionType();
            comboBox2.DisplayMember = "Transaction_Name";
            comboBox2.ValueMember = "id";
        }
        private void Apply()
        {
            listView1.Items.Clear();
            listView1.View = View.Details;
            lists.Clear();
            SqlConnection con = new SqlConnection(connection_string);
            string query = "select * from Log where ServeDate BETWEEN @param1 and @param2 ";
                
                if ((int)comboBox1.SelectedValue > 0)
                    query += " and Servicing_Office = @param3"; // servicing_office_id
                if ((int)comboBox2.SelectedValue > 0)
                    query += " and Transaction_Name = @param4";
                query += " order by ServeDate desc";
                string Servicing_Office_Name = " ";
                string ServeDate = " ";
                string Transaction_Name = " ";
                string _Type = " ";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@param1", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@param2", dateTimePicker2.Value);
                if ((int)comboBox1.SelectedValue > 0)
                    cmd.Parameters.AddWithValue("@param3",comboBox1.SelectedValue);
                if ((int)comboBox2.SelectedValue > 0)
                    cmd.Parameters.AddWithValue("@param4", comboBox2.SelectedValue);
                Console.WriteLine("Query + " + query);
                SqlDataReader rdr;
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Servicing_Office_Name = rdr["Servicing_Office_Name"].ToString();
                    ServeDate = rdr["ServeDate"].ToString();
                    Transaction_Name = rdr["Transaction_Name"].ToString();
                    _Type = rdr["Type"].ToString();
                    string[] row = { Servicing_Office_Name, ServeDate, Transaction_Name, _Type };

                    var lvi = new ListViewItem(row);
                    listView1.Items.Add(lvi);
                    lists.Add(new __Log
                    {
                    Servicing_Office_Name = Servicing_Office_Name,
                    ServeDate = ServeDate,
                    Transaction_Name = Transaction_Name,
                    Type = _Type
                    });
                }
                con.Close();

        }
        private DataTable getShownItems()
        {
            DataTable a = new DataTable();
            a.Columns.Add("Servicing_Office", typeof(string));
            a.Columns.Add("Date", typeof(string));
            a.Columns.Add("Transaction_Name", typeof(string));
            a.Columns.Add("Type", typeof(string));
            foreach(__Log q in lists)
            {
                a.Rows.Add(
                    q.Servicing_Office_Name,
                    q.ServeDate,
                    q.Transaction_Name,
                    q.Type);
            }
            Console.Write(" \n returning getTransasctionType... \n ");
            a.TableName = "Log " + DateTime.Today.ToString("D");
            return a;
        }
        private void export()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files|*.xlsx",
                Title = "Save an Excel File"
            };

            saveFileDialog.ShowDialog();
            var wb = new XLWorkbook();
            var dataTable = getShownItems();
            // Add a DataTable as a worksheet
            wb.Worksheets.Add(dataTable);
            if (!String.IsNullOrWhiteSpace(saveFileDialog.FileName))
                wb.SaveAs(saveFileDialog.FileName);

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Apply();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // export
            export();
        }
    }
}
