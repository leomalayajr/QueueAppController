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
    public partial class EditWindows : Form
    {
        private static String connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["dbString"].ConnectionString;
        private BindingSource bindingSource1 = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        // submitButton
        SqlConnection con = new SqlConnection(connection_string);
        SqlCommand cmd;
        SqlDataAdapter adapt;
        //ID variable used in Updating and Deleting Record  
        int ID = 0;
        List<_Servicing_Office> LIST_ServicingOffice = new List<_Servicing_Office>();
        List<int> LIST_Windows = new List<int>();
        public EditWindows()
        {
            InitializeComponent();
            DisplayData();
            AddWindows();
            RefreshServicingOfficeList();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            dataGridView1.Columns["id"].ReadOnly = true;
            dataGridView1.Columns["Name"].ReadOnly = true;
            dataGridView1.Columns["Servicing_Office_ID"].ReadOnly = true;
            dataGridView1.Columns["Window"].ReadOnly = true;
            dataGridView1.Columns["MAC_Address"].ReadOnly = true;
            
            dataGridView1.AutoResizeColumns();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }
        private void _SelectedIndexChanged(object sender, EventArgs e)
        {
            
        } 
        private void AddWindows()
        {
            for (int x = 1; x <= 5; x++)
                LIST_Windows.Add(x);

            info_Windows.DataSource = LIST_Windows;
        }
        private void RefreshServicingOfficeList()
        {
            LIST_ServicingOffice.Clear();
            _Servicing_Office b;
            con.Open();
            SqlCommand b_cmd = con.CreateCommand();
            SqlDataReader b_rdr;

            String b_q = "select * from Servicing_Office";
            b_cmd = new SqlCommand(b_q, con);

            b_rdr = b_cmd.ExecuteReader();
            while (b_rdr.Read())
            {
                b = new _Servicing_Office
                {
                    id = (int)b_rdr["id"],
                    Name = (string)b_rdr["Name"]
                };
                LIST_ServicingOffice.Add(b);
            }
            con.Close();
            info_ServicingOffice.DataSource = LIST_ServicingOffice;
            info_ServicingOffice.ValueMember = "id";
            info_ServicingOffice.DisplayMember = "Name";
        }
        private bool checkIfSame(int id, int Servicing_Office_ID, int Window)
        {
            SqlConnection con = new SqlConnection(connection_string);
            string query = "select id from Set_Windows where Servicing_Office_ID = @param1 AND Window = @param2";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@param1", Servicing_Office_ID);
            cmd.Parameters.AddWithValue("@param2", Window);
            Console.WriteLine("office id = " + Servicing_Office_ID + "  WINDOW" + Window);
            
            try
            {
                con.Open();
                var a = (int)cmd.ExecuteScalar();
                con.Close();
                if (a >= 0)
                    if (a != id)
                    {
                        MessageBox.Show("Office and window should be unique to one terminal.", "Request failed");
                        return true;
                    }
                    else
                        return false;
                else
                    return true;
            }
            catch (NullReferenceException) { return false; }
            catch (SqlException) { return true; }
        }
        private bool checkMACIfExists(int id,string _MAC)
        {
            SqlConnection con = new SqlConnection(connection_string);
            string query = "select id from Set_Windows where MAC_Address = @param1";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@param1", _MAC);
            
            try
            {
                con.Open();
                var a = (int)cmd.ExecuteScalar();
                con.Close();
                if (a >= 0)
                    if (a != id)
                    {
                        MessageBox.Show("You entered a MAC address that is already used.", "Request failed");
                        return true;
                    }
                    else
                        return false;
                else
                    return true;
            }
            catch (NullReferenceException) { return false; }
            catch (SqlException) { return true; }
        }
        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (info_MAC.Text != "")
            {
                if (!checkIfSame(ID,(int)info_ServicingOffice.SelectedValue, (int)info_Windows.SelectedValue))
                {
                    if (!checkMACIfExists(ID,info_MAC.Text))
                    {
                        try
                        {
                            string __name = info_ServicingOffice.Text;
                            string query = "insert into Set_Windows(Name,Servicing_Office_ID,Window,MAC_Address) values(@office_name,@office_id,@window,@MAC)";
                            cmd = new SqlCommand(query, con);
                            con.Open();
                            cmd.Parameters.AddWithValue("@office_name", __name);
                            cmd.Parameters.AddWithValue("@office_id", info_ServicingOffice.SelectedValue);
                            cmd.Parameters.AddWithValue("@window", info_Windows.SelectedValue);
                            cmd.Parameters.AddWithValue("@MAC", info_MAC.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Record Inserted Successfully");
                            DisplayData();
                            ClearData();
                        }
                        catch (NullReferenceException)
                        {
                            MessageBox.Show("One of the office does not exist.", "Error!");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Provide Details!");
            }
            RefreshServicingOfficeList();
            ClearData();
        }
        private void DisplayData()
        {
            try
            {
                con.Open();
                DataTable dt = new DataTable();
                adapt = new SqlDataAdapter("select * from Set_Windows", con);
                adapt.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();
            }
            catch (ArgumentOutOfRangeException) { }
        }
        //Clear Data  
        private void ClearData()
        {
            try
            {
                info_MAC.Text = "";
                info_ServicingOffice.SelectedIndex = 0;
                info_Windows.SelectedIndex = 0;
            }
            catch (ArgumentOutOfRangeException) { }
        }
        //dataGridView1 RowHeaderMouseClick Event  
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try { ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()); }
            catch (FormatException) { }
            info_MAC.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            info_ServicingOffice.SelectedValue = (dataGridView1.Rows[e.RowIndex].Cells[2].Value);
            info_Windows.SelectedIndex = info_Windows.Items.IndexOf(dataGridView1.Rows[e.RowIndex].Cells[3].Value);
            Console.WriteLine("**" + (dataGridView1.Rows[e.RowIndex].Cells[2].Value));
        }
        //Update Record  
        private void btn_Update_Click(object sender, EventArgs e)
        {
            Console.WriteLine("ID is " + ID);
            if (info_MAC.Text != "")
            {
                if (!checkIfSame(ID,(int)info_ServicingOffice.SelectedValue, (int)info_Windows.SelectedValue))
                {
                    if (!checkMACIfExists(ID,info_MAC.Text))
                    {
                        try
                        {
                            cmd = new SqlCommand("update Set_Windows set Name=@param3,Servicing_Office_ID=@param1,Window=@param2,MAC_Address=@param6 where ID=@param0", con);
                            con.Open();
                            cmd.Parameters.AddWithValue("@param0", ID);
                            cmd.Parameters.AddWithValue("@param1", (int)info_ServicingOffice.SelectedValue);
                            cmd.Parameters.AddWithValue("@param2", (int)info_Windows.SelectedValue);
                            cmd.Parameters.AddWithValue("@param3", info_ServicingOffice.Text);
                            cmd.Parameters.AddWithValue("@param6", info_MAC.Text);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Record Updated Successfully");
                            con.Close();
                            DisplayData();
                            ClearData();
                        }
                        catch (SqlException eb)
                        {
                            MessageBox.Show(eb.Message + "Error!");
                        }
                        catch (NullReferenceException)
                        {
                            MessageBox.Show("One of the office does not exist.", "Error!");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Select Record to Update");
            }
        }
        //Delete Record  
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (ID != 0)
            {
                cmd = new SqlCommand("delete Set_Windows where id=@id", con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", ID);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Deleted Successfully!");
                DisplayData();
                ClearData();
            }
            else
            {
                MessageBox.Show("Please Select Record to Delete");
            }
        }
    }
}
