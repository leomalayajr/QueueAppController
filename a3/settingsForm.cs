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
        DataTable table_Servicing_Office;
        public settingsForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            generateDeleteItems();
            table_Servicing_Office = getServicingOffice();
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
                    Console.WriteLine((string)_rdr["Name"]);
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
            string retrieve_servicing_offices = "select * from Users where Status = 1";
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
                                id = (int)_rdr["ID"],
                                usernameAndFullName = (string)_rdr["Username"] + " / " + (string)_rdr["FullName"]
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
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "id";
            comboBox2.DataSource = LIST_getTransactionType();
            comboBox2.DisplayMember = "Transaction_Name";
            comboBox2.ValueMember = "id";
            //comboBox3.Items.AddRange(LIST_getUsers().OrderBy(c => c.usernameAndFullName).ToArray());
            comboBox3.DataSource = LIST_getUsers().OrderBy(c => c.FullName).ToArray();
            comboBox3.DisplayMember = "usernameAndFullName";
            comboBox3.ValueMember = "id";

        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            // Save changes
            // Check textBoxes length if OK

            Console.WriteLine("wat items");
            if (OkFieldLength())
            {
                    SqlConnection con = new SqlConnection(connection_string);

                    // Check if password is correct

                    SqlCommand _check_cmd = new SqlCommand("SELECT Password FROM users WHERE status = @param_st", con);
                    _check_cmd.Parameters.AddWithValue("@param_st", 0);
                    SqlDataReader reader;
                    string Password = "";
                    Console.WriteLine("Running items");
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
        private bool OkFieldLength()
        {
                if (textBox4.TextLength > 7 && textBox4.TextLength < 50)
                { MessageBox.Show("New password should be 8 - 50 characters."); return false; }
                if (textBox6.TextLength > 7 && textBox6.TextLength < 50)
                { MessageBox.Show("Old password should be 8 - 50 characters."); return false; }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 100 && textBox1.TextLength >= 2 && textBox2.TextLength < 100)
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                    MessageBox.Show("Name must not be empty!", "Error");
                else
                {
                    var confirmResult = MessageBox.Show("Are you sure to add " + textBox1.Text + "?",
                                         "Confirm Add",
                                         MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        SqlConnection con = new SqlConnection(connection_string);
                        con.Open();
                        string __query = "select * from Servicing_Office where Name = @param1";
                        SqlCommand __cmd = new SqlCommand(__query, con);
                        __cmd.Parameters.AddWithValue("@param1", textBox1.Text);
                        var __a = __cmd.ExecuteScalar();

                        if (__a != null)
                            MessageBox.Show(textBox1.Text + " already exists!", "Error!");
                        else
                        {
                            // Add to database
                            string _query = "insert into Servicing_Office (Name,Address) OUTPUT Inserted.id values (@param1,@param2)";
                            SqlCommand _cmd = new SqlCommand(_query, con);
                            string _b = "insert into Queue_Info (Current_Number,Current_Queue,Servicing_Office,Counter," +
                                "Mode,Status,Avg_Serving_Time,Office_Name) values " +
                                "(0,1,@param_newID,0,1,@param_online,0,@param_newName)";
                            SqlCommand _cmd2 = new SqlCommand(_b, con);
                            try
                            {

                                _cmd.Parameters.AddWithValue("@param1", textBox1.Text);
                                _cmd.Parameters.AddWithValue("@param2", textBox2.Text);
                                int a = (int)_cmd.ExecuteScalar();
                                _cmd.Parameters.Clear();
                                _cmd2.Parameters.AddWithValue("@param_newID", a);
                                _cmd2.Parameters.AddWithValue("@param_newName", textBox1.Text);
                                _cmd2.Parameters.AddWithValue("@param_online", "Online");
                                _cmd2.ExecuteNonQuery();
                                generateDeleteItems();
                                textBox1.Clear();
                                textBox2.Clear();
                                MessageBox.Show(textBox1.Text + " added on Servicing Offices!", "Success!");
                            }
                            catch (SqlException ea)
                            {
                                MessageBox.Show("Database error. -> " + ea.Message);
                            }
                        }
                        con.Close();
                    }
                }
            }
            else
            {
                //textBox1.TextLength < 100 && textBox1.TextLength >= 2 && textBox2.TextLength < 100
                //MessageBox.Show("Name or address length exceeds the limit!");
                MessageBox.Show("Name length should 2 to 100, address is less than 100.", "Length error!");
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
        private DataTable getServicingOffice()
        {
            DataTable b = new DataTable();
            b.Columns.Add("id", typeof(int));
            b.Columns.Add("Name", typeof(string));
            b.Columns.Add("Address", typeof(string));
            SqlConnection con = new SqlConnection(connection_string);
            using (con)
            {
                con.Open();
                SqlCommand b_cmd = con.CreateCommand();
                SqlDataReader b_rdr;

                String b_q = "select * from Servicing_Office";
                b_cmd = new SqlCommand(b_q, con);

                b_rdr = b_cmd.ExecuteReader();
                while (b_rdr.Read())
                {
                    b.Rows.Add(
                       (int)b_rdr["id"],
                       (string)b_rdr["Name"],
                       (string)b_rdr["Address"]);
                }
                con.Close();
            }
            return b;
        }
        private string getServicingOfficeName(int _so)
        {
            string servicing_office_Name = "";
            foreach (DataRow row in table_Servicing_Office.Rows)
            {
                int temp_id = (int)row["id"];
                if (_so == temp_id)
                {
                    servicing_office_Name = (string)row["Name"];
                    break;
                }
            }
            return servicing_office_Name;
        }
        private void Queue_Info_Update()
        {
            //Checks whether Queue_Info is available.
            //Writes default data.
            //Always executed when a Kiosk have been opened.
            Console.WriteLine("Info update have been called.");
            SqlConnection con = new SqlConnection(connection_string);
            using (con)
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                SqlCommand cmd2 = con.CreateCommand();
                SqlCommand cmd3 = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd2.CommandType = CommandType.Text;

                String query = "select * from Queue_Info";
                String query2 = "";

                SqlDataReader rdr;
                cmd = new SqlCommand(query, con);

                rdr = cmd.ExecuteReader();
                int rowCount = 0;
                while (rdr.Read())
                { rowCount++; { if (rowCount > 0) { break; } } }
                if (rowCount > 0)
                {
                    //MessageBox.Show(Status+" already");
                }
                else
                {
                    foreach (DataRow a in table_Servicing_Office.Rows)
                    {
                        int _so = (int)a["id"];
                        query2 = "insert into Queue_Info (Current_Number,Current_Queue,Servicing_Office,Mode,Status,Counter,Office_Name) values (@cn,@cq,@so,@m,@sn,@c,@o_n)";
                        cmd2 = new SqlCommand(query2, con);
                        cmd2.Parameters.AddWithValue("@cn", 0);
                        cmd2.Parameters.AddWithValue("@cq", 1);
                        cmd2.Parameters.AddWithValue("@so", _so);
                        cmd2.Parameters.AddWithValue("@m", 1);
                        cmd2.Parameters.AddWithValue("@sn", "Online");
                        cmd2.Parameters.AddWithValue("@c", "0");
                        cmd2.Parameters.AddWithValue("@o_n", getServicingOfficeName(_so));
                        int result = cmd2.ExecuteNonQuery();
                        Console.WriteLine("Queue info inserting something...");
                        // Inserting data to firebase
                        // FirebaseFunction: Kiosk_Insert_QueueInfo(_so);
                    }
                }

            }
            con.Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int _id = (int)comboBox1.SelectedValue;
                SqlConnection con = new SqlConnection(connection_string);
                string SERVICINGOFFICE_Delete = "delete from Servicing_Office where id = @param1";
                string QUEUEINFO_Delete = "delete from Queue_Info where Servicing_Office = @param1";
                string WINDOW_Delete = "delete from Set_Windows where Servicing_Office_ID = @param1";
                string TERMINAL_Delete = "delete from Servicing_Terminal where Servicing_Office = @param1";
                string search_office_on_other_transactions = "select Transaction_ID from Transaction_List where Servicing_Office = @param_so ";
                string TRANSACTIONTYPE_DELETE = "delete from Transaction_Type where id = @param_id";
                string TRANSACTIONLIST_DELETE = "delete from Transaction_List where Transaction_ID = @param_id";
                SqlCommand _cmd = new SqlCommand(SERVICINGOFFICE_Delete, con);
                SqlCommand __cmd = new SqlCommand(QUEUEINFO_Delete, con);
                SqlCommand ___cmd = new SqlCommand(WINDOW_Delete, con);
                SqlCommand ____cmd = new SqlCommand(TERMINAL_Delete, con);
                SqlCommand _____cmd = new SqlCommand(TRANSACTIONTYPE_DELETE, con);
                SqlCommand ______cmd = new SqlCommand(TRANSACTIONLIST_DELETE, con);
                SqlCommand cmd_ = new SqlCommand(search_office_on_other_transactions, con);
                SqlDataReader rdr;
                var confirmResult = MessageBox.Show("Are you sure to delete this?",
                                         "Confirm Delete",
                                         MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    con.Open();
                    // If 'Yes', do something here.
                    // Check if Servicing_Office exists on other Transaction Type
                    String transaction_list = string.Empty;
                    List<int> transaction_id_where_offices_exists = new List<int>();
                    cmd_.Parameters.AddWithValue("@param_so", _id);
                    rdr = cmd_.ExecuteReader();
                    while (rdr.Read())
                    {
                        transaction_id_where_offices_exists.Add((int)rdr["Transaction_ID"]);
                    }
                    foreach (int a in transaction_id_where_offices_exists)
                    {
                        _____cmd.Parameters.AddWithValue("@param_id", a);
                        _____cmd.ExecuteNonQuery();
                        _____cmd.Parameters.Clear();
                        ______cmd.Parameters.AddWithValue("@param_id", a);
                        ______cmd.ExecuteNonQuery();
                        ______cmd.Parameters.Clear();
                    }
                    //{
                    //   transaction_list += Environment.NewLine + (string)rdr[""]
                    //}
                    if (transaction_id_where_offices_exists.Count > 0)
                    {
                        var confirmresult2 = MessageBox.Show("You are about to delete an office that is used on transaction type/s."+Environment.NewLine+" Deleting this will also delete transaction type/s where it is used.",
                                             "Confirm Delete",
                                             MessageBoxButtons.YesNo);
                        if (confirmresult2 == DialogResult.Yes)
                        {
                            try
                            {
                                _cmd.Parameters.AddWithValue("@param1", _id);
                                _cmd.ExecuteNonQuery();
                                __cmd.Parameters.AddWithValue("@param1", _id);
                                __cmd.ExecuteNonQuery();
                                ___cmd.Parameters.AddWithValue("@param1", _id);
                                ___cmd.ExecuteNonQuery();
                                ____cmd.Parameters.AddWithValue("@param1", _id);
                                ____cmd.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show("Error ->" + ex.Message);
                            }
                            catch (NullReferenceException)
                            {
                                Console.WriteLine("Selecting nothing...");
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            
                            _cmd.Parameters.AddWithValue("@param1", _id);
                            _cmd.ExecuteNonQuery();
                            __cmd.Parameters.AddWithValue("@param1", _id);
                            __cmd.ExecuteNonQuery();
                            ___cmd.Parameters.AddWithValue("@param1", _id);
                            ___cmd.ExecuteNonQuery();
                            ____cmd.Parameters.AddWithValue("@param1", _id);
                            ____cmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Error ->" + ex.Message);
                        }
                        catch (NullReferenceException)
                        {
                            Console.WriteLine("Selecting nothing...");
                        }
                    }
                    con.Close();
                }
                else
                {
                    // If 'No', do something here.
                }
            }
            catch (NullReferenceException) { }
            generateDeleteItems();


        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(connection_string);
            string TRANSACTION_Delete = "delete from Transaction_Type where id = @param1";
            string TRANSACTION_Delete_List = "delete from Transaction_List where Transaction_ID = @param1";
            SqlCommand _cmd = new SqlCommand(TRANSACTION_Delete, con);

            var confirmResult = MessageBox.Show("Are you sure to delete this transaction?",
                                     "Confirm Delete",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                // If 'Yes', do something here.
                try
                {
                    int a = (int)comboBox2.SelectedValue;
                    con.Open();
                    _cmd.Parameters.AddWithValue("@param1", a);
                    _cmd.ExecuteNonQuery();
                    _cmd.CommandText = TRANSACTION_Delete_List;
                    _cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error ->" + ex.Message);
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("Selecting nothing...");
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
                catch (NullReferenceException)
                {
                    Console.WriteLine("Selecting nothing...");
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
                    Enabled = false;
                    firebase_Connection fcon = new firebase_Connection();
                    SqlConnection con = new SqlConnection(connection_string);
                    string query = "insert into vw_es_students (Full_Name,Student_No,College,Course,Status,Year,Password) " +
                        " values (@param_fn,@param_sn,@param_cl,@param_cs,@param_s,@param_y,@param_p)";
                    string truncate_query = "TRUNCATE TABLE vw_es_students";
                    SqlCommand cmd = new SqlCommand(query,con);
                    SqlCommand truncate_cmd = new SqlCommand(truncate_query, con);
                    string path = dialog.FileName;
                    Excel excel = new Excel(path, 1);
                    try
                    {
                        List<_App_User> results = excel.ReadCell();

                        //cleanup
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        excel.cleanCOMobjects();

                        con.Open();
                        truncate_cmd.ExecuteNonQuery();
                        int max = excel._userCount * 4;
                        progressBar1.Maximum = max;
                        //drop queue_status and accounts first
                        await fcon.Controller_TruncateQueueStatus();
                        await fcon.Controller_DeleteAllAccounts();
                        foreach (_App_User b in results)
                        {
                            string password = Cryptography.Encrypt(b.password.ToString());

                            progressBar1.Increment(1);
                            //upload to the Firebase DB
                            try
                            {
                                await fcon.Controller_ImportUsers(b);
                                progressBar1.Increment(1);
                                await fcon.Controller_RegisterThisUser(b);
                                progressBar1.Increment(1);
                                await fcon.Controller_InsertQueueStatus(b.accountNumber);
                                progressBar1.Increment(1);

                                //upload to local db
                                cmd.Parameters.AddWithValue("@param_fn", b.lastName.ToUpper() + "," + b.firstName + " " + b.middleName);
                                cmd.Parameters.AddWithValue("@param_sn", b.accountNumber);
                                cmd.Parameters.AddWithValue("@param_cl", b.college);
                                cmd.Parameters.AddWithValue("@param_cs", b.course);
                                cmd.Parameters.AddWithValue("@param_s", b.status);
                                cmd.Parameters.AddWithValue("@param_y", b.year);
                                cmd.Parameters.AddWithValue("@param_p", password);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            catch (FirebaseAuthException exd) {
                                Console.WriteLine(exd.Message);
                            }
                            
                        }
                        con.Close();
                        progressBar1.Value = max;
                        MessageBox.Show("Import finished!", "Success!");
                        progressBar1.Value = 0;
                    }
                    catch (FormatException)
                    {
                        progressBar1.Value = 0;
                        MessageBox.Show("Make sure all the ID contains numbers only.", "Format error");
                    }
                    catch (FirebaseAuthException exd)
                    {
                        //progressBar1.Value = 0;
                        //MessageBox.Show("Make sure all of the Student_No contains no spaces and special characters. Error Code: "+exd.Reason,"Online Database error!");
                    }
                    catch (FirebaseException)
                    {
                        progressBar1.Value = 0;
                        MessageBox.Show("Please check your internet connection. Error Code:", "Connection error");
                    }
                    Enabled = true;
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (textBox6.Enabled == false)
            { textBox6.Enabled = true; textBox4.Enabled = true; textBox6.Clear(); textBox4.Clear(); }
            else
            { textBox6.Enabled = false; textBox4.Enabled = false; textBox6.Clear(); textBox4.Clear(); }
        }
        

        private async void button7_Click(object sender, EventArgs e)
        {
            Enabled = false;
            var confirmResult = MessageBox.Show("Are you sure to restart the queuing system? ",
                                     "Clean queue confirmation",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                SqlConnection con = new SqlConnection(connection_string);
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    String getRating = "select * from Rating_Office";
                    String saveRating = "insert into Feedbacks (Servicing_Office,Score,Transaction_ID,Date_Of_Feedback,isStudent) values " +
                        " (@param_so,@param_score,@param_tt_ID,@param_date,@param_isStudent)";
                    SqlCommand cmd_getRating = new SqlCommand(getRating, con);
                    SqlCommand cmd_saveRating = new SqlCommand(saveRating, con);
                    cmd_getRating.Transaction = tran;
                    cmd_saveRating.Transaction = tran;
                    SqlDataReader rdr_rating;
                    List<_Rating_Office> evaluationForToday = new List<_Rating_Office>();
                    String query0 = "TRUNCATE TABLE Main_Queue; TRUNCATE TABLE Queue_Info; TRUNCATE TABLE Serving_Time; TRUNCATE TABLE Servicing_Terminal; TRUNCATE TABLE Rating_Office; TRUNCATE TABLE Serving_Info;";
                    // retrieve the evaluations first 
                    rdr_rating = cmd_getRating.ExecuteReader();
                    while (rdr_rating.Read())
                    {
                        _Rating_Office a = new _Rating_Office
                        {
                            Customer_Queue_Number = (string)rdr_rating["Customer_Queue_Number"],
                            isStudent = (bool)rdr_rating["isStudent"],
                            score = (int)rdr_rating["Score"],
                            Servicing_OFfice = (int)rdr_rating["Servicing_Office"],
                            Transaction_ID = (int)rdr_rating["Transaction_ID"]
                        };
                        evaluationForToday.Add(a);
                    }
                    // save the evaluations
                    DateTime today = DateTime.Today;
                    progressBar1.Maximum = 2 + evaluationForToday.Count;
                    foreach (_Rating_Office b in evaluationForToday)
                    {
                        cmd_saveRating.Parameters.AddWithValue("@param_so", b.Servicing_OFfice);
                        cmd_saveRating.Parameters.AddWithValue("@param_score", b.score);
                        cmd_saveRating.Parameters.AddWithValue("@param_tt_ID", b.Transaction_ID);
                        cmd_saveRating.Parameters.AddWithValue("@param_date", today);
                        cmd_saveRating.Parameters.AddWithValue("@param_isStudent", b.isStudent);
                        cmd_saveRating.ExecuteNonQuery();
                        cmd_saveRating.Parameters.Clear();
                        progressBar1.Increment(1);
                    }
                    // clean all
                    SqlCommand cmd0 = new SqlCommand(query0, con);
                    cmd0.Transaction = tran;
                    cmd0.ExecuteNonQuery();
                    progressBar1.Increment(1);
                    // Doing the work on firebase too
                    firebase_Connection fcon = new firebase_Connection();
                    await fcon.Truncate_Firebase();
                    fcon.Controller_SetAllToInactive();
                    fcon.App_Delete_PreQueueAsyncNoCTS();
                    progressBar1.Increment(1);
                    tran.Commit();
                    Queue_Info_Update(); 
                    MessageBox.Show("All queue at the system and information about it have been cleared.","Clean Success!");
                    progressBar1.Value = 0;
                }
                catch (FirebaseException exd)
                {
                    try { tran.Rollback(); } catch (Exception exRollback) { MessageBox.Show("Error at -> " + exRollback.Message); }
                    Console.WriteLine("An error occured while connecting to firebase DB. Error ->" + exd.Message);
                    MessageBox.Show("Please check your internet connection.", "Could not connect online");
                    progressBar1.Value = 0;
                }
                catch (SqlException eb)
                {
                    try { tran.Rollback(); } catch (Exception exRollback) { MessageBox.Show("Error at -> " + exRollback.Message); }
                    MessageBox.Show("An error occured while connecting to local DB. Error -> " + eb.Message, "Databse error");
                    progressBar1.Value = 0;
                }
                con.Close();
            }
                
            Enabled = true;
        }
    }
}
