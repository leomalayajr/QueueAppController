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
using Firebase.Database;
using Firebase.Auth;
namespace a3
{
    public partial class settingsForm : Form
    {

        private String connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["dbString"].ConnectionString;
        main mainForm = (main)Application.OpenForms["main"];
        addTransactionType form_tt = (addTransactionType)Application.OpenForms["addTransactionType"];
        public settingsForm()
        {
            InitializeComponent();
            generateDeleteItems();
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
        private List<_Users> LIST_getUsers()
        {
            List<_Users> dataSource = new List<_Users>();
            // List possible Servicing Offices
            SqlConnection con = new SqlConnection(connection_string);
            string retrieve_servicing_offices = "select * from Users";
            SqlDataReader _rdr;
            SqlCommand __cmd = new SqlCommand(retrieve_servicing_offices, con);
            
                    try
                    {
                        con.Open();
                        _rdr = __cmd.ExecuteReader();
                        while (_rdr.Read())
                        {
                            dataSource.Add(new _Users()
                            {
                                FullName = (string)_rdr["FullName"],
                                id = (int)_rdr["ID"]
                            });
                        }
                        con.Close();
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("Can't connect to local DB!");
                    }

            return dataSource;
        }
        public void generateDeleteItems()
        {

            comboBox1.DataSource = LIST_getServicingOffices();
            comboBox2.DataSource = LIST_getTransactionType();
            comboBox3.DataSource = LIST_getUsers();
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "id";
            comboBox2.DisplayMember = "Transaction_Name";
            comboBox2.ValueMember = "id";
            comboBox3.DisplayMember = "FullName";
            comboBox3.ValueMember = "id";

        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            // Save changes
            // Check textBoxes length if OK
            if (OkFieldLength())
            {

                if (mainForm != null)
                {
                    SqlConnection con = new SqlConnection(connection_string);

                    // Check if password is correct

                    SqlCommand _check_cmd = new SqlCommand("SELECT Password FROM users WHERE status = @param_st", con);
                    _check_cmd.Parameters.AddWithValue("@param_st", 0);
                    SqlDataReader reader;
                    string Password = "";

                    try
                    {
                        con.Open();
                        reader = _check_cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Password = (string)reader["Password"];
                        }
                        if (Cryptography.Decrypt(Password).Equals(textBox6.Text))
                        {
                            string query = "update Users set ";
                            SqlCommand _cmd = new SqlCommand();

                            _cmd.Connection = con;
                            
                            if (!(string.IsNullOrEmpty(textBox4.Text)))
                            {
                                query += " Password = @param2";
                                _cmd.Parameters.AddWithValue("@param2", Cryptography.Encrypt(textBox4.Text.ToString()));
                            }
                            

                            query += " where status = @param_st";
                            _cmd.Parameters.AddWithValue("@param_st", 0);
                            _cmd.CommandText = query;
                            _cmd.ExecuteNonQuery();
                            
                            textBox4.Clear();
                            textBox6.Clear();

                            MessageBox.Show("Change Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else
                        {
                            MessageBox.Show("Please enter the valid credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        con.Close();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("DB error! {0}", ex.Message);
                    }
                }

            }

        }
        private bool OkFieldLength()
        {
                if (textBox4.TextLength < 7 && textBox4.TextLength > 50)
                { MessageBox.Show("New password should be 8 - 50 characters."); return false; }
                if (textBox6.TextLength < 7 && textBox6.TextLength > 50)
                { MessageBox.Show("Old password should be 8 - 50 characters."); return false; }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
                    if (textBox1.TextLength < 100 && textBox1.TextLength >= 2 && textBox2.TextLength < 100)
                    {
                        var confirmResult = MessageBox.Show("Are you sure to add "+textBox1.Text+"?",
                                             "Confirm Delete",
                                             MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    // Add to database
                    SqlConnection con = new SqlConnection(connection_string);
                    string _query = "insert into Servicing_Office (Name,Address) " +
                        "values (@param1,@param2)";
                    SqlCommand _cmd = new SqlCommand(_query, con);
                    try
                    {
                        con.Open();
                        _cmd.Parameters.AddWithValue("@param1", textBox1.Text);
                        _cmd.Parameters.AddWithValue("@param2", textBox2.Text);
                        _cmd.ExecuteNonQuery();
                        _cmd.Parameters.Clear();
                        con.Close();
                        generateDeleteItems();
                        MessageBox.Show(textBox1.Text+" added on Servicing Offices!", "Success!");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Database error. {0}" + ex.Message);
                    }
                }
                    }
                    else
                    {
                        MessageBox.Show("Name or address length exceeds the limit!");
                    }
                

        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (form_tt == null)
            {
                form_tt = new addTransactionType();
            }

            form_tt.ShowDialog();


        }

        private void button4_Click(object sender, EventArgs e)
        {
                
                    SqlConnection con = new SqlConnection(connection_string);
                    string SERVICINGOFFICE_Delete = "delete from Servicing_Office where id = @param1";
                    SqlCommand _cmd = new SqlCommand(SERVICINGOFFICE_Delete, con);

                    var confirmResult = MessageBox.Show("Are you sure to delete this?",
                                             "Confirm Delete",
                                             MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        // If 'Yes', do something here.
                        try
                        {
                            con.Open();
                            _cmd.Parameters.AddWithValue("@param1", comboBox1.SelectedValue);
                            _cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Error ->" + ex.Message);
                        }
                    }
                    else
                    {
                        // If 'No', do something here.
                    }
                    generateDeleteItems();
                

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
                    SqlConnection con = new SqlConnection(connection_string);
                    string TRANSACTION_Delete = "delete from Transaction_Type where id = @param1";
                    SqlCommand _cmd = new SqlCommand(TRANSACTION_Delete, con);

                    var confirmResult = MessageBox.Show("Are you sure to delete this?",
                                             "Confirm Delete",
                                             MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        // If 'Yes', do something here.
                        try
                        {
                            con.Open();
                            _cmd.Parameters.AddWithValue("@param1", comboBox2.SelectedValue);
                            _cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Error ->" + ex.Message);
                        }
                    }
                    else
                    {
                        // If 'No', do something here.
                    }
                    generateDeleteItems();

                

        }

        private void button6_Click(object sender, EventArgs e)
        {

            SqlConnection con = new SqlConnection(connection_string);
            string USERS_Delete = "delete from Users where id = @param1";
            SqlCommand _cmd = new SqlCommand(USERS_Delete, con);

            var confirmResult = MessageBox.Show("Are you sure to delete this?",
                                     "Confirm Delete",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                // If 'Yes', do something here.
                try
                {
                    con.Open();
                    _cmd.Parameters.AddWithValue("@param1", comboBox3.SelectedValue);
                    _cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error ->" + ex.Message);
                }
            }
            else
            {
                // If 'No', do something here.
            }
            generateDeleteItems();

        }
        private void settingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private async void btn_Import_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    firebase_Connection fcon = new firebase_Connection();
                    MessageBox.Show("Prepare...");
                    string path = dialog.FileName;
                    Excel excel = new Excel(path, 1);
                    try
                    {
                        List<_App_User> results = excel.ReadCell();

                        //cleanup
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        excel.cleanCOMobjects();

                        progressBar1.Maximum = excel._userCount;
                        foreach (_App_User b in results)
                        {
                            //upload to the Firebase DB
                            await fcon.Controller_ImportUsers(b);
                            await fcon.Controller_RegisterThisUser(b);
                            progressBar1.Increment(1);
                        }
                        MessageBox.Show("Import finished. You may check the online database now.", "Success!");
                        progressBar1.Value = 0;
                    }
                    catch (FormatException)
                    {
                        progressBar1.Value = 0;
                        MessageBox.Show("Make sure all the ID contains numbers only.", "Format error");
                    }
                    catch (FirebaseAuthException exd)
                    {
                        progressBar1.Value = 0;
                        MessageBox.Show("Make sure all of the Student_No contains no spaces and special characters. Error Code: "+exd.Reason,"Online Database error!");
                    }
                    catch (FirebaseException)
                    {
                        progressBar1.Value = 0;
                        MessageBox.Show("Please check your internet connection. Error Code:", "Connection error");
                    }
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
        }
    }
}
