using Firebase.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading.Tasks.Dataflow;
using System.Net;
using System.Net.Http;
namespace a3
{
    public partial class main : Form
    {
        #region PROGRAM VARIABLES
        private String connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["dbString"].ConnectionString;
        private static int VARIABLE_Priority_Sync_Time = 0;
        private static bool VARIABLE_FIREBASE_IsOnline = false;
        private static bool VARIABLE_SQL_IsOnline = false;
        private static bool VARIABLE_Allowed_To_Sync = false;
        private static bool STATE_toggleSync = true;
        private static System.Windows.Forms.Timer timer1;
        private static System.Windows.Forms.Timer timer2;
        private static int counter = 0;
        private static string _wholeTextLog = string.Empty;
        private static _Main_Queue _mq;
        private static _Main_Queue _mq_t;
        private static _Main_Queue _pq_mq;
        private static _Queue_Info _qi;
        private static _Transfer_Queue _t_q;
        private static _Servicing_Terminal _st;
        private static _Transaction_Type _tt_t;
        private static _Log _log;
        private static bool DEBUG_deleteEveryRun = false;
        public int user_id = 7;
        Stopwatch stopp = new Stopwatch();
        Stopwatch temp_clock = new Stopwatch();
        DataTable table_Servicing_Office;
        DataTable table_Transactions;
        DataTable table_Transaction_Table;
        CancellationTokenSource wtoken;
        ITargetBlock<DateTimeOffset> wtask;
        settingsForm frmSettings = new settingsForm();
        Queue_Information frmQueueInfo = new Queue_Information();
        EditWindows frmWindows = new EditWindows();
        Evaluation evalForm = new Evaluation();
        #endregion
        public main()
        {
            #region MAIN CONSTRUCTOR
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            // Variable Init
            VARIABLE_Allowed_To_Sync = true;
            VARIABLE_Priority_Sync_Time = 60000;
            stopp.Start();
            txtLog.ScrollBars = ScrollBars.Both; // use scroll bars; no text wrapping
            toggleSync.Enabled = false;

            // Function Init
            // Functions that will be run once
            table_Servicing_Office = getServicingOffice();
            table_Transactions = getTransactionList();
            table_Transaction_Table = getTransactionType();
            PROGRAM_Sync_Once();
            Queue_Info_Update();

            // Functions that will be always updated
            //    PROGRAM_Online_Sync(); //This can be transferred on a button (switch) to manually start. Currently on automatic.
            //    InitSyncTimer();
            #endregion

        }
        #region BASIC METHODS
        private void btnSettings_Click(object sender, EventArgs e)
        {
            frmSettings.generateDeleteItems();
            if (frmSettings != null)
            {
                // Define the border style of the form to a dialog box.
                frmSettings.FormBorderStyle = FormBorderStyle.FixedDialog;

                // Set the MaximizeBox to false to remove the maximize box.
                frmSettings.MaximizeBox = false;

                // Set the MinimizeBox to false to remove the minimize box.
                frmSettings.MinimizeBox = false;

                // Set the start position of the form to the center of the screen.
                frmSettings.StartPosition = FormStartPosition.CenterScreen;
                frmSettings.Show();
            }

        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (frmQueueInfo.IsDisposed)
            {
                frmQueueInfo = new Queue_Information();
            }
            if (frmQueueInfo != null)
            {
                frmQueueInfo.InitTimer();
                frmQueueInfo.Show();
            }
        }

        private void btnWindows_Click(object sender, EventArgs e)
        {
            if (frmWindows.IsDisposed)
                frmWindows = new EditWindows();

            frmWindows.Show();
        }
        public Object CheckIfNull(SqlDataReader reader, int colIndex)
        {
            if (reader.GetFieldType(colIndex) == typeof(int))
                if (!reader.IsDBNull(colIndex))
                    return reader.GetInt32(colIndex);
                else
                    return 0;
            else
                if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            else
                return string.Empty;
        }
        private void RefreshUI()
        {
            // Set default values for UI
            if (VARIABLE_FIREBASE_IsOnline)
                txtOnlineStatus.Invoke(new Action(() => txtOnlineStatus.Text = "Online"));
            else
                txtOnlineStatus.Invoke(new Action(() => txtOnlineStatus.Text = "Offline"));

            if (VARIABLE_SQL_IsOnline)
                txtLocalStatus.Invoke(new Action(() => txtLocalStatus.Text = "Online"));
            else
                txtLocalStatus.Invoke(new Action(() => txtLocalStatus.Text = "Offline"));
        }
        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                    e.Cancel = MessageBox.Show(@"Do you really want to close the Controller?",
                                               "Controller",
                                               MessageBoxButtons.YesNo) == DialogResult.No;
                if (!e.Cancel) { Application.Exit(); }
                Console.WriteLine("!!! e.Cancel ="+e.Cancel);
            }
        }
        private DataTable getTransactionType()
        {
            DataTable a = new DataTable();
            a.Columns.Add("id", typeof(int));
            a.Columns.Add("Pattern_Max", typeof(int));
            a.Columns.Add("Transaction_Name", typeof(string));
            a.Columns.Add("Description", typeof(string));
            a.Columns.Add("Short_Name", typeof(string));
            SqlConnection con = new SqlConnection(connection_string);
            using (con)
            {
                con.Open();
                SqlCommand a_cmd = con.CreateCommand();
                SqlDataReader a_rdr;

                String a_q = "select * from Transaction_Type";
                a_cmd = new SqlCommand(a_q, con);

                a_rdr = a_cmd.ExecuteReader();
                while (a_rdr.Read())
                {
                    a.Rows.Add(
                       (int)a_rdr["id"],
                       (int)a_rdr["Pattern_Max"],
                       (string)a_rdr["Transaction_Name"],
                       (string)a_rdr["Description"],
                       (string)a_rdr["Short_Name"]);
                }
                con.Close();
            }
            Console.Write(" \n returning getTransasctionType... \n ");
            return a;
        }
        public void logWrite(string name, string text)
        {
            _log = new _Log
            {
                Name = name,
                Text = text,
                Date = DateTime.Now
            };
            _wholeTextLog = _log.Name + ": " + _log.Text + Environment.NewLine + _wholeTextLog;
            txtLog.Invoke(new Action(() => txtLog.Text = _wholeTextLog));
        }
        private DataTable getTransactionList()
        {
            DataTable transactionList = new DataTable();
            transactionList.Columns.Add("Transaction_ID", typeof(int));
            transactionList.Columns.Add("Servicing_Office", typeof(int));
            transactionList.Columns.Add("Pattern_No", typeof(int));

            SqlConnection con = new SqlConnection(connection_string);
            using (con)
            {
                con.Open();
                SqlCommand t_cmd = con.CreateCommand();
                SqlDataReader t_rdr;

                String t_q = "select * from Transaction_List";
                t_cmd = new SqlCommand(t_q, con);

                t_rdr = t_cmd.ExecuteReader();
                while (t_rdr.Read())
                {
                    transactionList.Rows.Add(
                       (int)t_rdr["Transaction_ID"],
                       (int)t_rdr["Servicing_Office"],
                       (int)t_rdr["Pattern_No"]);
                    Console.Write(" getTransactions -> Added a row! ");
                }
                con.Close();
            }
            Console.Write(" \n returning transactionList... \n ");
            return transactionList;
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
        #endregion
        #region SYNC ONCE METHOD
        public async void PROGRAM_Sync_Once()
        {
            // Sync items that are default for queuing 
            // Or items need to be inserted and not always updated
            await Task.Run(async () =>
            {
                if (VARIABLE_Allowed_To_Sync)
                {

                    SqlConnection con = new SqlConnection(connection_string);
                    firebase_Connection fcon = new firebase_Connection();
                    string QUERY_InsertTransactions = "select * from Transaction_Type";

                    SqlCommand COMMAND_syncOnce = new SqlCommand(QUERY_InsertTransactions, con);

                    using (con)
                    {
                        try { con.Open(); }
                        catch (SqlException e)
                        {
                            MessageBox.Show("Local SQL Database could not be found. This app will now close.");
                            Environment.Exit(0);
                        }
                        try
                        {
                            // Clean the online DB first before any sync happens
                            Task k1 = Task.Run(async () => { await fcon.App_Delete_QueueInfoAsync(); });
                            Task k2 = Task.Run(async () => { await fcon.App_Delete_MainQueueAsync(); });
                            Task k3 = Task.Run(async () => { await fcon.App_Delete_TransferQueueAsync(); });
                            Task k4 = Task.Run(async () => { await fcon.App_Delete_ServicingTerminalAsync(); });

                            if (DEBUG_deleteEveryRun)
                            {
                                await Task.WhenAll(k1, k2, k3, k4);
                                logWrite("Online", "Online DB cleared.");
                            }
                            SqlDataReader RDR_Transaction;

                            RDR_Transaction = COMMAND_syncOnce.ExecuteReader();

                            List<_Transaction_Type> LIST_Transaction_Type = new List<_Transaction_Type>();
                            while (RDR_Transaction.Read())
                            {
                                _tt_t = new _Transaction_Type
                                {
                                    id = (int)RDR_Transaction["ID"],
                                    Transaction_Name = (string)RDR_Transaction["Transaction_Name"],
                                    Description = (string)RDR_Transaction["Description"]
                                };
                                LIST_Transaction_Type.Add(_tt_t);
                            }

                            await Task.Run(
                            async () =>
                            {
                                 fcon.App_Delete_TransactionTypeAsync();
                                foreach (_Transaction_Type a in LIST_Transaction_Type)
                                     fcon.App_Insert_TransactionTypeAsync(a);
                            }
                            );

                            logWrite("Program", "Items to work on first sync done.");
                        }
                        catch (FirebaseException)
                        {
                            logWrite("Error -> Online", "Can't connect to the database.");
                            MessageBox.Show("Can't connect online.");
                            Environment.Exit(0);
                        }

                    }

                }
                toggleSync.Invoke(new Action(() => toggleSync.Enabled = true));
            });

            Console.WriteLine("SyncOnce() finished.");
        }
        #endregion

        #region MAIN SYNC FUNCTION
        private async Task PROGRAM_Online_Sync(CancellationToken cancelToken)
        {
            VARIABLE_FIREBASE_IsOnline = true;
            VARIABLE_SQL_IsOnline = true;
            // Check if the app can connect to online DB
            WebRequest request = WebRequest.Create("http://firebase.google.com");
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response == null || response.StatusCode != HttpStatusCode.OK)
                {
                    logWrite("Online", "Internet is OK but can't reach Google Firebase.");
                    MessageBox.Show("Internet is OK but can't reach Google Firebase.");
                }
                response.Close();
            }
            catch (WebException)
            {
                VARIABLE_FIREBASE_IsOnline = false;
                logWrite("Error -> Online", "Can't connect!");
            }
            // Check if the app can connect to local DB
            try
            {
                SqlConnection localSQLcheck = new SqlConnection(connection_string);
                localSQLcheck.Open();
                SqlCommand TEST_CMD = new SqlCommand("select top 1 id from Users", localSQLcheck);
                int q = (int)TEST_CMD.ExecuteScalar();
                localSQLcheck.Close();
            }
            catch (SqlException)
            {
                VARIABLE_SQL_IsOnline = false;
                // output the error to see what's going on
                logWrite("Error -> Local", "Can't connect on Local DB.");
            }

            // Refresh values on the UI
            RefreshUI();
            //This function fetches data from the local database, then uploads it online.
            await Task.Run(async () =>
            {
                if (VARIABLE_Allowed_To_Sync)
                {

                    if (VARIABLE_FIREBASE_IsOnline)
                    {
                        SqlConnection con = new SqlConnection(connection_string);

                        String QUERY_Select_QueueInfo = "select Current_Number, Current_Queue, Servicing_Office, Status, Customer_Queue_Number, Window, Avg_Serving_Time, id, Office_Name from Queue_Info";
                        String QUERY_Select_MainQueue = "select * from Main_Queue";
                        //String QUERY_Select_TransferQueue = "select * from Transfer_Queue";
                        String QUERY_Select_ServicingTerminal = "select * from Servicing_Terminal";

                        List<_Main_Queue> LIST_MainQueue = new List<_Main_Queue>();
                        List<_Queue_Info> LIST_QueueInfo = new List<_Queue_Info>();
                        List<_Queue_Request> LIST_QueueRequest = new List<_Queue_Request>();
                        List<_Servicing_Terminal> LIST_ServicingTerminal = new List<_Servicing_Terminal>();
                        List<_Transaction_Type> LIST_TransactionTypes = new List<_Transaction_Type>();

                        List<_Main_Queue> LIST_MainQueue_fromFirebase = new List<_Main_Queue>();
                        //List<_Transfer_Queue> LIST_TransferQueue_fromFirebase = new List<_Transfer_Queue>();

                        List<string> fromOnline_MainQueue = new List<string>();
                        //List<string> fromOnline_TransferQueue = new List<string>();
                        List<string> fromLocal_MainQueue = new List<string>();
                        //List<string> fromLocal_TransferQueue = new List<string>();

                        List<_Main_Queue> LOCAL_MainQueueList = new List<_Main_Queue>();
                       //List<_Transfer_Queue> LOCAL_TransferQueueList = new List<_Transfer_Queue>();

                        List<_Main_Queue> ONLINE_MainQueueList = new List<_Main_Queue>();
                       //List<_Transfer_Queue> ONLINE_TransferQueueList = new List<_Transfer_Queue>();

                        List<_Queue_Info> temp_QueueInfoList = new List<_Queue_Info>();
                        List<_Servicing_Terminal> temp_ServicingTerminalList = new List<_Servicing_Terminal>();

                        List<_Main_Queue> COMBINE_MainQueueList = new List<_Main_Queue>();
                        List<_Transfer_Queue> COMBINE_TransferQueueList = new List<_Transfer_Queue>();

                        List<_Main_Queue> PREQUEUE_TO_MAINQUEUE = new List<_Main_Queue>();
                        List<_Pre_Queue> PREQUEUE_LIST = new List<_Pre_Queue>();
                        using (con)
                        {
                            Console.WriteLine("Checking if local db is online.");
                            try { con.Open(); }
                            catch (SqlException)
                            {
                                logWrite("Error -> Program", "Local SQL Databse could not be found. Closing app.");
                                MessageBox.Show("Local SQL Database could not be found. This app will now close.");
                                Environment.Exit(0);
                            }



                            SqlCommand CMD_select_MainQueue = new SqlCommand(QUERY_Select_MainQueue, con);
                            SqlCommand CMD_select_QueueInfo = new SqlCommand(QUERY_Select_QueueInfo, con);
                            //SqlCommand CMD_select_TransferQueue = new SqlCommand(QUERY_Select_TransferQueue, con);
                            // SqlCommand CMD_select_TransactionQueue = new SqlCommand(QUERY_Select_TransactionQueue, con);
                            SqlCommand CMD_select_ServicingTerminal = new SqlCommand(QUERY_Select_ServicingTerminal, con);

                            SqlDataReader RDR_mq_select;
                            SqlDataReader RDR_qi_select;
                            // SqlDataReader RDR_q_transaction_select;
                            // SqlDataReader RDR_transfer_q_select;
                            SqlDataReader RDR_st_select;


                            firebase_Connection fcon = new firebase_Connection();

                            var t1 = Task.Run(() =>
                                {
                                    RDR_mq_select = CMD_select_MainQueue.ExecuteReader();

                                    while (RDR_mq_select.Read())
                                    {
                                        // set the class
                                        _mq = new _Main_Queue
                                        {
                                            Queue_Number = (int)RDR_mq_select["Queue_Number"],
                                            Full_Name = (string)RDR_mq_select["Full_Name"],
                                            Servicing_Office = (int)RDR_mq_select["Servicing_Office"],
                                            Transaction_Type = (int)RDR_mq_select["Transaction_Type"],
                                            Type = ((Boolean)RDR_mq_select["Type"] == false) ? "Student" : "Guest",
                                            Customer_From = ((Boolean)RDR_mq_select["Customer_From"] == false) ? "Local" : "Mobile",
                                            Customer_Queue_Number = (string)RDR_mq_select["Customer_Queue_Number"],
                                            ID = (int)RDR_mq_select["id"],
                                            Pattern_Current = (int)RDR_mq_select["Pattern_Current"],
                                            Time = (DateTime)RDR_mq_select["Time"],
                                            Student_No = (string)RDR_mq_select["Student_No"],
                                            Pattern_Max = (int)RDR_mq_select["Pattern_Max"]
                                        };
                                        // insert it to temporary list of _Main_Queue
                                        LIST_MainQueue.Add(_mq);
                                        // insert same list but only Customer_Queue_Number as string
                                        fromLocal_MainQueue.Add((string)RDR_mq_select["Customer_Queue_Number"]);
                                    }
                                    CMD_select_MainQueue.Dispose();
                                });
                            var t2 = Task.Run(() =>
                                {
                                    RDR_qi_select = CMD_select_QueueInfo.ExecuteReader();

                                    while (RDR_qi_select.Read())
                                    {
                                        // set the class
                                        _qi = new _Queue_Info
                                        {
                                            ID = (int)RDR_qi_select["id"],
                                            Current_Number = (int)RDR_qi_select["Current_Number"],
                                            Current_Queue = (int)RDR_qi_select["Current_Queue"],
                                            Servicing_Office = (int)RDR_qi_select["Servicing_Office"],
                                            Status = (string)RDR_qi_select["Status"],
                                            Customer_Queue_Number = (string)CheckIfNull(RDR_qi_select, 4),
                                            Window = (int)CheckIfNull(RDR_qi_select, 5),
                                            Avg_Serving_Time = (int)RDR_qi_select["Avg_Serving_Time"],
                                            Office_Name = (string)RDR_qi_select["Office_Name"]
                                        };
                                        // insert it to temporary list
                                        LIST_QueueInfo.Add(_qi);
                                    }
                                    CMD_select_QueueInfo.Dispose();
                                });
                            var t3 = Task.Run(() =>
                                {
                                    string QUERY_RetrieveTransactions = "select * from Transaction_Type";
                                    SqlDataReader RDR_transactions;
                                    SqlCommand COMMAND_RetrieveTransactions = new SqlCommand(QUERY_RetrieveTransactions, con);
                                    RDR_transactions = COMMAND_RetrieveTransactions.ExecuteReader();
                                    while (RDR_transactions.Read())
                                    {
                                        // set the class
                                        _tt_t = new _Transaction_Type
                                        {
                                            id = (int)RDR_transactions["id"],
                                            Transaction_Name = (string)RDR_transactions["Transaction_Name"],
                                            Description = (string)RDR_transactions["Description"]
                                        };
                                        LIST_TransactionTypes.Add(_tt_t);
                                    }
                                });
                            var t4 = Task.Run(() =>
                                {
                                    RDR_st_select = CMD_select_ServicingTerminal.ExecuteReader();

                                    while (RDR_st_select.Read())
                                    {
                                        // set the class
                                        _st = new _Servicing_Terminal
                                        {
                                            Customer_Queue_Number = (string)RDR_st_select["Customer_Queue_Number"],
                                            Servicing_Office = (int)RDR_st_select["Servicing_Office"],
                                            Window = (int)RDR_st_select["Window"],
                                            Name = (string)RDR_st_select["Name"]
                                        };
                                        LIST_ServicingTerminal.Add(_st);
                                    }
                                    CMD_select_ServicingTerminal.Dispose();
                                }

                                );

                            await Task.WhenAll(t1, t2, t3, t4);
                            logWrite("Local", "Preparing lists for sync done.");

                            Console.WriteLine("Initializing lists finished.");
                            try
                            {
                                var i1 = Task.Run(async () =>
                                        {
                                            foreach (_Queue_Info a in LIST_QueueInfo)
                                                try { await fcon.App_Insert_QueueInfoAsync(a, cancelToken); }
                                                catch (FirebaseException) { throw; }
                                        });
                                var i2 = Task.Run(async () =>
                                    {
                                        // await fcon.App_Delete_MainQueueAsync(); // April 02, 2018
                                        fromOnline_MainQueue = await fcon.CQN_Retrieve_MainQueueAsync(cancelToken);
                                        LIST_MainQueue_fromFirebase = await fcon.App_Retrieve_MainQueueAsync(cancelToken);

                                        // remove everything that is on Local but not online
                                        LOCAL_MainQueueList = LIST_MainQueue
                                                    .Where(w => !fromOnline_MainQueue
                                                    .Contains(w.Customer_Queue_Number))
                                                    .ToList();

                                        // remove everything that is at Online but not Local
                                        // these are lists of Customer_Queue_Number
                                        ONLINE_MainQueueList = LIST_MainQueue_fromFirebase
                                                    .Where(w => !fromLocal_MainQueue
                                                    .Contains(w.Customer_Queue_Number))
                                                    .ToList();

                                        // update everything that is at Online and Local
                                        HashSet<string> diff_cqn = new HashSet<string>(LIST_MainQueue.Select(s => s.Customer_Queue_Number));
                                        COMBINE_MainQueueList = LIST_MainQueue_fromFirebase.Where(m => diff_cqn.Contains(m.Customer_Queue_Number)).ToList();

                                        var __result = (from mq_fire in LIST_MainQueue
                                                        join mq_allLocal in COMBINE_MainQueueList on mq_fire.ID equals mq_allLocal.ID
                                                        into mq_update
                                                        select new _Main_Queue
                                                        {
                                                            Full_Name = mq_fire.Full_Name,
                                                            Queue_Number = mq_fire.Queue_Number,
                                                            Queue_Status = mq_fire.Queue_Status,
                                                            Servicing_Office = mq_fire.Servicing_Office,
                                                            Student_No = mq_fire.Student_No,
                                                            Transaction_Type = mq_fire.Transaction_Type,
                                                            Type = mq_fire.Type,
                                                            Customer_From = mq_fire.Customer_From,
                                                            Customer_Queue_Number = mq_fire.Customer_Queue_Number,
                                                            ID = mq_fire.ID,
                                                            Pattern_Current = mq_fire.Pattern_Current,
                                                            Time = mq_fire.Time,
                                                            Key = mq_update.Any() ? mq_update.First().Key : "Unknown Key",
                                                            Pattern_Max = mq_fire.Pattern_Max
                                                        });

                                        List<_Main_Queue> LIST_MainQueue_withKeys = __result.ToList();

                                        Console.WriteLine("List of new customers LOCAL");
                                        foreach (_Main_Queue a in LOCAL_MainQueueList)
                                        {
                                            cancelToken.ThrowIfCancellationRequested();
                                            Console.WriteLine(a.Customer_Queue_Number);
                                            // If not on Firebase but exists on Local (new updates) => Insert
                                            if (a.Type == "Guest")
                                                fcon.App_Insert_MainQueueAsync(a, true, cancelToken);
                                            else
                                                fcon.App_Insert_MainQueueAsync(a, false, cancelToken);
                                        }
                                        Console.WriteLine("List of new customers ONLINE");
                                        foreach (_Main_Queue b in ONLINE_MainQueueList)
                                        {
                                            cancelToken.ThrowIfCancellationRequested();
                                            Console.WriteLine(b.Customer_Queue_Number);
                                            // If not on Local but exists on Firebase (outdated) => Delete
                                            if (b.Type == "Student")
                                                fcon.Specific_Delete_MainQueueAsync(b.Student_No, cancelToken);
                                            else
                                                fcon.Specific_Delete_MainQueueAsync(b.Key, cancelToken);
                                        }
                                        Console.WriteLine("List of customers to be updated");
                                        foreach (_Main_Queue c in LIST_MainQueue_withKeys)
                                        {
                                            cancelToken.ThrowIfCancellationRequested();
                                            //Console.WriteLine(c.Customer_Queue_Number);
                                            //    Console.WriteLine(c.Full_Name);
                                            //Console.WriteLine(c.Key);
                                            if (c.Key != "Unknown Key")
                                                fcon.App_Update_MainQueue(c);
                                            else
                                                Console.WriteLine(c.Customer_Queue_Number + " -> key not known, ignored.");

                                        }

                                    });
                                var i3 = Task.Run(async () =>
                                    {
                                        LIST_QueueRequest = await fcon.App_Retrieve_QueueRequest(cancelToken);
                                        foreach (_Queue_Request c in LIST_QueueRequest)
                                        {
                                            SqlConnection _temp_connection = new SqlConnection(connection_string);
                                            _temp_connection.Open();
                                            //Check what type of Request
                                            switch (c.Action)
                                            {
                                                case "Drop":
                                                    Console.WriteLine();
                                                    Console.WriteLine("DROP REQUEST ID OF " + c.ID);
                                                    string _query = "DELETE FROM Main_Queue where id = @param1";
                                                    SqlCommand _cmd = new SqlCommand(_query, _temp_connection);
                                                    _cmd.Parameters.AddWithValue("@param1", c.ID);
                                                    _cmd.ExecuteNonQuery();
                                                    Console.WriteLine(c.ID + " is now deleted");
                                                    break;
                                                case "Move":
                                                    // Check if it can allow move
                                                    string _check_turns = "SELECT COUNT(id) from Main_Queue where Queue_Number between @param1 and @param2 and Servicing_Office = @param3";
                                                    SqlCommand _check_cmd = new SqlCommand(_check_turns, _temp_connection);
                                                    int _new_QueueNumber = c.Queue_ID + c.Value;
                                                    _check_cmd.Parameters.AddWithValue("@param1", (c.Queue_ID + 1));
                                                    _check_cmd.Parameters.AddWithValue("@param2", _new_QueueNumber);
                                                    _check_cmd.Parameters.AddWithValue("@param3", c.Servicing_Office);
                                                    int _customers = (int)_check_cmd.ExecuteScalar();
                                                    Console.WriteLine();
                                                    Console.WriteLine("Customer with id of {0} wanted to move {1} turns.", c.Queue_ID, c.Value);
                                                    Console.WriteLine(" BETWEEN {0} and {1} at {2}", (c.Queue_ID + 1), _new_QueueNumber, c.Servicing_Office);
                                                    Console.WriteLine("There are {0} customers.", _customers);
                                                    Console.WriteLine("Checking if {0} >= 5 and {0} <= 20", _customers);
                                                    if (_customers >= 5 && _customers <= 20)
                                                    {
                                                        // Increment all values on between
                                                        Console.WriteLine("SET QUEUE_NUMBER - 1 BETWEEN {0} and {1}", (c.Queue_ID + 1), _new_QueueNumber);
                                                        string _update_query = "";
                                                        SqlCommand _update_cmd = new SqlCommand(_update_query, _temp_connection);
                                                        _update_query = "UPDATE Main_Queue set Queue_Number = Queue_Number - 1 where Queue_Number between @param1 and @param2";
                                                        _update_cmd.CommandText = _update_query;
                                                        _update_cmd.Parameters.AddWithValue("@param1", (c.Queue_ID + 1));
                                                        _update_cmd.Parameters.AddWithValue("@param2", _new_QueueNumber);
                                                        _update_cmd.ExecuteNonQuery();
                                                        Console.WriteLine("SET QUEUE_NUMBER = {0} where id = {1}", _new_QueueNumber, c.ID);
                                                        _update_query = "UPDATE Main_Queue set Queue_Number = @param1 where id = @param2";
                                                        _update_cmd.CommandText = _update_query;
                                                        _update_cmd.Parameters.AddWithValue("@param1", _new_QueueNumber);
                                                        _update_cmd.Parameters.AddWithValue("@param2", c.ID);

                                                        _update_cmd.ExecuteNonQuery();

                                                        // Call a function to send a successful change request

                                                        /**/
                                                    }
                                                    else
                                                    {
                                                        // Call a function to send a notification denying request
                                                        /**/
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                            _temp_connection.Close();
                                        }
                                        await fcon.App_Delete_QueueRequest(cancelToken);
                                    });
                                var i4 = Task.Run(async () =>
                                    {
                                        foreach (_Servicing_Terminal d in LIST_ServicingTerminal)
                                            await fcon.App_Insert_ServicingTerminalAsync(d, cancelToken);
                                    });
                                var i6 = Task.Run(async () => 
                                {
                                    foreach (_Transaction_Type g in LIST_TransactionTypes)
                                        await fcon.App_Insert_TransactionTypeAsync(g);
                                });
                                var i5 = Task.Run(async () =>
                                    {
                                        PREQUEUE_LIST = await fcon.App_Retrieve_PreQueue(cancelToken);
                                        foreach (_Pre_Queue e in PREQUEUE_LIST)
                                        {
                                            Console.WriteLine("WE -> " + e.Transaction_Type);
                                            int firstServicingOffice = getFirstServicingOffice(e.Transaction_Type);
                                            int newQueueNumber = getQueueNumber(firstServicingOffice);
                                            _pq_mq = new _Main_Queue
                                            {
                                                Queue_Number = newQueueNumber,
                                                Servicing_Office = firstServicingOffice,
                                                Pattern_Max = retrievePatternMax(e.Transaction_Type),
                                                Customer_Queue_Number = generateQueueShortName(e.Transaction_Type, newQueueNumber),
                                                Full_Name = e.Full_Name,
                                                Student_No = e.Student_No,
                                                Transaction_Type = e.Transaction_Type,
                                                Time = UnixTimeStampToDateTime(e.timestamp_date),
                                                Pattern_Current = 1,
                                                Queue_Status = "Waiting",
                                                Type = "Student",
                                                Customer_From = "Mobile"
                                            };
                                            PREQUEUE_TO_MAINQUEUE.Add(_pq_mq);
                                        }

                                        string QUERY_Add_PreQueue_To_MainQueue = "insert into Main_Queue " +
                                        "(Queue_Number,Full_Name,Servicing_Office,Student_No," +
                                        "Transaction_Type,Type,Time,Pattern_Current,Pattern_Max," +
                                        "Customer_Queue_Number,Queue_Status,Customer_From) " +
                                        " values (@q_qn,@q_fn,@q_so,@q_sn,@q_tt,@q_type,GETDATE(),@q_pc,@q_pm,@q_cqn,@q_qs,@q_cf)";
                                        SqlConnection temp_con = new SqlConnection(connection_string);
                                        SqlCommand cmdPreQueue = new SqlCommand(QUERY_Add_PreQueue_To_MainQueue, temp_con);
                                        using (temp_con)
                                        {
                                            temp_con.Open();
                                            foreach (_Main_Queue f in PREQUEUE_TO_MAINQUEUE)
                                            {

                                                Console.WriteLine("PREQUEUE {0} ==", f.Customer_Queue_Number);
                                                // Add to local DB -> MainQueue
                                                cmdPreQueue.Parameters.AddWithValue("@q_qn", f.Queue_Number);
                                                cmdPreQueue.Parameters.AddWithValue("@q_fn", f.Full_Name);
                                                cmdPreQueue.Parameters.AddWithValue("@q_so", f.Servicing_Office);
                                                cmdPreQueue.Parameters.AddWithValue("@q_sn", f.Student_No);
                                                cmdPreQueue.Parameters.AddWithValue("@q_tt", f.Transaction_Type);
                                                cmdPreQueue.Parameters.AddWithValue("@q_type", 0); // 0 = student
                                                cmdPreQueue.Parameters.AddWithValue("@q_pc", f.Pattern_Current);
                                                cmdPreQueue.Parameters.AddWithValue("@q_pm", f.Pattern_Max);
                                                cmdPreQueue.Parameters.AddWithValue("@q_cqn", f.Customer_Queue_Number);
                                                cmdPreQueue.Parameters.AddWithValue("@q_qs", f.Queue_Status);
                                                cmdPreQueue.Parameters.AddWithValue("@q_cf", 1); // 1 = mobile
                                                cmdPreQueue.ExecuteNonQuery();
                                                cmdPreQueue.Parameters.Clear();

                                            }
                                            temp_con.Close();
                                        }
                                        // Delete after retrieving all PreQueue and inserting them to MainQueue

                                        await fcon.App_Delete_PreQueueAsync(cancelToken);
                                    }
                                    );


                                logWrite("Online", "-----");
                                await Task.WhenAll(i6);
                                await Task.WhenAll(i1);
                                await Task.WhenAll(i3, i5);
                                //await Task.WhenAll(i1, i2, i3, i4, i5);
                                await Task.WhenAll(i2, i4);
                                logWrite("Online", "Sync successful at " + DateTime.Now + " !");
                            }
                            catch (OperationCanceledException)
                            {
                                logWrite("Program", "Cancelled!");
                                return;
                            }
                            catch (FirebaseException)
                            {
                                logWrite("Online", "Can't connect!");
                            }
                            catch (NullReferenceException b)
                            {
                                Console.WriteLine(b);
                            }

                            Console.WriteLine("Last work for synching finished. /n /n /n ");
                            Console.WriteLine("------------------------------------------");


                            con.Close();
                        }
                        counter++;
                        Console.WriteLine("How many times update run? -> " + counter + " at TIME -> " + stopp.Elapsed);
                        Console.WriteLine("Hours at " + stopp.Elapsed.Hours);
                        Console.WriteLine("Minutes at " + stopp.Elapsed.Minutes);
                        Console.WriteLine("Seconds at " + stopp.Elapsed.Seconds);
                        Console.WriteLine("TotalSeconds at " + stopp.Elapsed.TotalSeconds);
                    }
                }
                else
                {
                    Console.WriteLine("Sync is not allowed.");
                }

            });
            GC.Collect();
        }
        #endregion
        #region FUNCTIONS COPIED FROM KIOSK
        /**** LIST OF FUNCTIONS FROM KIOSK as of April 04, 2018 ****/
        private void incrementQueueNumber(int q_so)
        {
            SqlConnection con = new SqlConnection(connection_string);
            using (con)
            {
                con.Open();
                int b = 0;
                // increment queue number 
                SqlCommand cmd4;
                String query2 = "update Queue_Info set Current_Queue = Current_Queue+1 OUTPUT Inserted.Current_Queue where Servicing_Office = @Servicing_Office";
                cmd4 = new SqlCommand(query2, con);
                cmd4.Parameters.AddWithValue("@Servicing_Office", q_so);
                b = (int)cmd4.ExecuteScalar();
                con.Close();

            }

        }
        private int getQueueNumber(int q_so)
        {
            SqlConnection con = new SqlConnection(connection_string);
            // retrieves queue number
            int res = 0;
            using (con)
            {
                con.Open();
                SqlCommand cmd3;
                String query = "select Current_Queue from Queue_Info where Servicing_Office = @Servicing_Office";
                cmd3 = new SqlCommand(query, con);
                cmd3.Parameters.AddWithValue("@Servicing_Office", q_so);
                SqlDataReader rdr2;
                rdr2 = cmd3.ExecuteReader();
                while (rdr2.Read()) { res = (int)rdr2["Current_Queue"]; }
                con.Close();
            }
            incrementQueueNumber(q_so);

            return res;
        }
        private int getFirstServicingOffice(int q_tt)
        {
            int a = 0;
            int temp_pattern_no = 0;
            int temp_transaction_id = 0;
            foreach (DataRow row in table_Transactions.Rows)
            {
                temp_pattern_no = (int)row["Pattern_No"];
                temp_transaction_id = (int)row["Transaction_ID"];
                if (q_tt == temp_transaction_id && temp_pattern_no == 1)
                {
                    a = (int)row["Servicing_Office"];
                    break;
                }
            }
            return a;
        }
        private int retrievePatternMax(int Transaction_Type)
        {
            int a = 0, id = 0;
            foreach (DataRow row in table_Transaction_Table.Rows)
            {
                id = (int)row["id"];
                Console.Write(" RetrievePatternMax -> searching for the respective pattern number ");
                if (id == Transaction_Type)
                {
                    a = (int)row["Pattern_Max"];
                    break;
                }
            }
            return a;
        }
        private string generateQueueShortName(int Transaction_Type, int queueNumber)
        {
            string short_name = "";
            int id = 0;
            foreach (DataRow row in table_Transaction_Table.Rows)
            {
                id = (int)row["id"];
                Console.WriteLine(" generateQueueShortName - > searching for short name");
                if (id == Transaction_Type)
                {
                    short_name = (string)row["Short_Name"];

                    break;
                }

            }
            short_name += queueNumber;
            return short_name;
        }
        /**** LIST OF FUNCTIONS END                             ****/

        #endregion
        #region IMPORTANT METHODS FOR SYNC
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public void InitSyncTimer()
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(run_EveryTick);
            timer1.Interval = VARIABLE_Priority_Sync_Time; // in miliseconds
            timer1.Start();
        }
        private ITargetBlock<DateTimeOffset> CreateNeverEndingTask(
    Func<DateTimeOffset, CancellationToken, Task> action,
    CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (action == null) throw new ArgumentNullException("action");

            // Declare the block variable, it needs to be captured.
            ActionBlock<DateTimeOffset> block = null;

            // Create the block, it will call itself, so
            // you need to separate the declaration and
            // the assignment.
            // Async so you can wait easily when the
            // delay comes.
            block = new ActionBlock<DateTimeOffset>(async now =>
            {
                    // Perform the action.  Wait on the result.
                    try
                {
                    await action(now, cancellationToken).
                    // Doing this here because synchronization context more than
                    // likely *doesn't* need to be captured for the continuation
                    // here.  As a matter of fact, that would be downright
                    // dangerous.
                    ConfigureAwait(false);

                        // Wait.
                        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken).
                    // Same as above.
                    ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    logWrite("Online", "Online sync is turned off.");
                }
                catch (OperationCanceledException)
                {
                    logWrite("Online", "Online sync is turned off.");
                }
                    // Post the action back to the block.
                    block.Post(DateTimeOffset.Now);
            }, new ExecutionDataflowBlockOptions
            {
                CancellationToken = cancellationToken
            });

            // Return the block.
            return block;
        }
        private async Task sample_function_to_test_on_dowork(CancellationToken ct)
        {
            HttpClient client = new HttpClient();

            Console.WriteLine("async is starting");
            firebase_Connection fcon = new firebase_Connection();
            SqlConnection con = new SqlConnection(connection_string);
            con.Open();
            string query = "select * from Main_Queue";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader rdr;
            _Main_Queue mq;
            rdr = cmd.ExecuteReader();
            await Task.Run(async () =>
            {
                while (rdr.Read())
                {

                    mq = new _Main_Queue
                    {
                        Customer_Queue_Number = (string)rdr["Customer_Queue_Number"],
                        Full_Name = (string)rdr["Full_Name"],
                        ID = (int)rdr["ID"],
                        Pattern_Current = (int)rdr["Pattern_Current"],
                        Pattern_Max = (int)rdr["Pattern_Max"],
                        Queue_Number = (int)rdr["Queue_Number"],
                        Queue_Status = (string)rdr["Queue_Status"],
                        Servicing_Office = (int)rdr["Servicing_Office"],
                        Student_No = (string)rdr["Student_No"],
                        Time = (DateTime)rdr["Time"],
                        Transaction_Type = (int)rdr["Transaction_Type"],
                        Type = "Guest",
                        Customer_From = "Local"
                    };
                    try { await fcon.App_Insert_MainQueueAsync(mq, true, ct); }
                    catch (FirebaseException) { Console.WriteLine("Disconnected!!!"); }
                    catch (OperationCanceledException) { Console.WriteLine("cancelled"); return; }
                    Console.WriteLine("{0} -> {1}", mq.Customer_Queue_Number, mq.Key);
                    logWrite("debug", mq.Customer_Queue_Number);



                }
            }, ct);
            con.Close();
        }
        private Task DoWork(CancellationToken ct) { return Task.Run(() => { Console.WriteLine("Hello Task library!"); }); }
        private void StartWork()
        {
            // Create the token source.
            wtoken = new CancellationTokenSource();

            // Set the task.
            wtask = CreateNeverEndingTask((now, ct) => PROGRAM_Online_Sync(ct), wtoken.Token);

            // Start the task.  Post the time.
            wtask.Post(DateTimeOffset.Now);

        }
        private void StopWork()
        {
            // CancellationTokenSource implements IDisposable.
            using (wtoken)
            {
                // Cancel.  This will cancel the task.
                wtoken.Cancel();
            }

            // Set everything to null, since the references
            // are on the class level and keeping them around
            // is holding onto invalid state.
            wtoken = null;
            wtask = null;

            // Not a part of StopWork

            txtOnlineStatus.Invoke(new Action(() => txtOnlineStatus.Text = ""));
            txtLocalStatus.Invoke(new Action(() => txtLocalStatus.Text = ""));
        }
        private void run_EveryTick(object sender, EventArgs e)
        {
            //PROGRAM_Online_Sync();

        }
        void btnDisable_Tick(object sender, System.EventArgs e)
        {
            toggleSync.Enabled = true;
            timer2.Stop();
        }

        private void toggleSync_Click(object sender, EventArgs e)
        {
            timer2 = new System.Windows.Forms.Timer();
            timer2.Tick += new EventHandler(btnDisable_Tick);
            timer2.Interval = 5000; // here time in milliseconds
            timer2.Start();

            if (STATE_toggleSync)
            {
                toggleSync.Text = "ON";
                toggleSync.BackColor = Color.DarkSlateGray;
                STATE_toggleSync = false;
                StartWork();
            }
            else
            {
                toggleSync.Text = "OFF";
                toggleSync.BackColor = Color.Gray;
                STATE_toggleSync = true;
                StopWork();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            new Login().Show();
            this.Close();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (evalForm.IsDisposed)
                evalForm = new Evaluation();

            evalForm.Show(); 
        }
    }
}