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

namespace a3
{
    public partial class main : Form
    {
        private String connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["dbString"].ConnectionString;
        private static int VARIABLE_Priority_Sync_Time = 0;
        private static int VARIABLE_System_Sync_Time = 0;
        private static bool VARIABLE_Allowed_To_Sync = false;
        private static System.Windows.Forms.Timer timer1;
        private static int counter = 0;
        private static _Main_Queue _mq;
        private static _Main_Queue _mq_t;
        private static _Main_Queue _pq_mq;
        private static _Queue_Info _qi;
        private static _Queue_Transaction _qt;
        private static _Transfer_Queue _t_q;
        private static _Servicing_Terminal _st;
        private static _Transaction_Type _tt_t;
        private static bool DEBUG_deleteEveryRun = false;
        Stopwatch stopp = new Stopwatch();
        DataTable table_Servicing_Office;
        DataTable table_Transactions;
        DataTable table_Transaction_Table;
        public main()
        {
            InitializeComponent();

            // Variable Init
            VARIABLE_Allowed_To_Sync = true;
            VARIABLE_Priority_Sync_Time = 60000;
            VARIABLE_System_Sync_Time = 60000;
            stopp.Start();


            // Function Init
            // Functions that will be run once
            table_Servicing_Office = getServicingOffice();
            table_Transactions = getTransactionList();
            table_Transaction_Table = getTransactionType();
            PROGRAM_Sync_Once();
            Queue_Info_Update();

            // Functions that will be always updated
            PROGRAM_Online_Sync(); //This can be transferred on a button (switch) to manually start. Currently on automatic.
            InitSyncTimer();

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
                    Console.Write(" write getTransactionType -> Added a row! ");
                }
                con.Close();
            }
            Console.Write(" \n returning getTransasctionType... \n ");
            return a;
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
                cmd = new SqlCommand(query,con);

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
        public void PROGRAM_Sync_Once()
        {
            // Sync items that are default for queuing 
            // Or items need to be inserted and not always updated

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
                    // Clean the online DB first before any sync happens
                    if(DEBUG_deleteEveryRun)
                    Parallel.Invoke(
                        async () => { await fcon.App_Delete_QueueInfoAsync(); },
                        async () => { await fcon.App_Delete_MainQueueAsync(); },
                        async () => { await fcon.App_Delete_TransferQueueAsync(); },
                        async () => { await fcon.App_Delete_ServicingTerminalAsync(); }
                        );

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
                    Parallel.Invoke( 
                        async ()=> 
                        {
                            await fcon.App_Delete_TransactionTypeAsync();
                            foreach (_Transaction_Type a in LIST_Transaction_Type)
                                await fcon.App_Insert_TransactionTypeAsync(a);
                            
                        } 
                        );
                }

            }

        }
        public Object CheckIfNull(SqlDataReader reader, int colIndex)
        {
            if (reader.GetFieldType(colIndex) == typeof(int))
                if(!reader.IsDBNull(colIndex))
                    return reader.GetInt32(colIndex);
                else
                    return 0;
            else
                if(!reader.IsDBNull(colIndex))
                    return reader.GetString(colIndex);
                else
                    return string.Empty;
        }
        private async void PROGRAM_Online_Sync()
        {
            //This function fetches data from the local database, then uploads it online.
            if (VARIABLE_Allowed_To_Sync)
            {
                
                SqlConnection con = new SqlConnection(connection_string);

                String QUERY_Select_QueueInfo = "select Current_Number, Current_Queue, Servicing_Office, Status, Customer_Queue_Number, Window, Avg_Serving_Time, id, Office_Name from Queue_Info";
                String QUERY_Select_MainQueue = "select * from Main_Queue";
                String QUERY_Select_TransferQueue = "select * from Transfer_Queue";
                String QUERY_Select_ServicingTerminal = "select * from Servicing_Terminal";
                // String QUERY_Select_TransactionQueue = "select * from Queue_Transaction";

                List<_Main_Queue> LIST_MainQueue = new List<_Main_Queue>();
                List<_Queue_Info> LIST_QueueInfo = new List<_Queue_Info>();
                // List<_Queue_Transaction> LIST_QueueTransaction = new List<_Queue_Transaction>();
                List<_Transfer_Queue> LIST_TransferQueue = new List<_Transfer_Queue>();
                List<_Servicing_Terminal> LIST_ServicingTerminal = new List<_Servicing_Terminal>();

                List<_Main_Queue> LIST_MainQueue_fromFirebase = new List<_Main_Queue>();
                List<_Transfer_Queue> LIST_TransferQueue_fromFirebase = new List<_Transfer_Queue>();

                List<string> fromOnline_MainQueue = new List<string>();
                List<string> fromOnline_TransferQueue = new List<string>();
                List<string> fromLocal_MainQueue = new List<string>();
                List<string> fromLocal_TransferQueue = new List<string>();

                List<_Main_Queue> LOCAL_MainQueueList = new List<_Main_Queue>();
                List<_Transfer_Queue> LOCAL_TransferQueueList = new List<_Transfer_Queue>();

                List<_Main_Queue> ONLINE_MainQueueList = new List<_Main_Queue>();
                List<_Transfer_Queue> ONLINE_TransferQueueList = new List<_Transfer_Queue>();
                
                List<_Queue_Info> temp_QueueInfoList = new List<_Queue_Info>();
                List<_Servicing_Terminal> temp_ServicingTerminalList = new List<_Servicing_Terminal>();

                List<_Main_Queue> COMBINE_MainQueueList = new List<_Main_Queue>();
                List<_Transfer_Queue> COMBINE_TransferQueueList = new List<_Transfer_Queue>();

                List<_Main_Queue> PREQUEUE_TO_MAINQUEUE = new List<_Main_Queue>();
                List<_Pre_Queue> PREQUEUE_LIST = new List<_Pre_Queue>();
                using (con)
                {
                    try { con.Open(); }
                    catch (SqlException e) { MessageBox.Show("Local SQL Database could not be found. This app will now close.");
                        Environment.Exit(0);
                    }

                    SqlCommand CMD_select_MainQueue = new SqlCommand(QUERY_Select_MainQueue, con);
                    SqlCommand CMD_select_QueueInfo = new SqlCommand(QUERY_Select_QueueInfo, con);
                    SqlCommand CMD_select_TransferQueue = new SqlCommand(QUERY_Select_TransferQueue, con);
                    // SqlCommand CMD_select_TransactionQueue = new SqlCommand(QUERY_Select_TransactionQueue, con);
                    SqlCommand CMD_select_ServicingTerminal = new SqlCommand(QUERY_Select_ServicingTerminal, con);

                    SqlDataReader RDR_mq_select;
                    SqlDataReader RDR_qi_select;
                 // SqlDataReader RDR_q_transaction_select;
                    SqlDataReader RDR_transfer_q_select;
                    SqlDataReader RDR_st_select;

                   
                    firebase_Connection fcon = new firebase_Connection();
                    

                    Parallel.Invoke(
                        () => {
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
                                    Customer_Queue_Number = (string)RDR_mq_select["Customer_Queue_Number"],
                                    ID = (int)RDR_mq_select["id"],
                                    Pattern_Current = (int)RDR_mq_select["Pattern_Current"],
                                    Time = (DateTime)RDR_mq_select["Time"],
                                    Student_No = (string)RDR_mq_select["Student_No"]
                                };
                                // insert it to temporary list of _Main_Queue
                                LIST_MainQueue.Add(_mq);
                                // insert same list but only Customer_Queue_Number as string
                                fromLocal_MainQueue.Add((string)RDR_mq_select["Customer_Queue_Number"]);
                            }
                        }, 
                        () => {
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
                        },
                        () => {
                            RDR_transfer_q_select = CMD_select_TransferQueue.ExecuteReader();

                            while (RDR_transfer_q_select.Read())
                            {
                                // set the class
                                _mq_t = new _Main_Queue
                                {
                                    Queue_Number = (int)RDR_transfer_q_select["Queue_Number"],
                                    Full_Name = (string)RDR_transfer_q_select["Full_Name"],
                                    Servicing_Office = (int)RDR_transfer_q_select["Servicing_Office"],
                                    Transaction_Type = (int)RDR_transfer_q_select["Transaction_Type"],
                                    Type = ((Boolean)RDR_transfer_q_select["Type"] == false) ? "Student" : "Guest",
                                    Customer_Queue_Number = (string)RDR_transfer_q_select["Customer_Queue_Number"],
                                    ID = (int)RDR_transfer_q_select["id"],
                                    Pattern_Current = (int)RDR_transfer_q_select["Pattern_Current"],
                                    Time = (DateTime)RDR_transfer_q_select["Time"],
                                    Student_No = (string)RDR_transfer_q_select["Student_No"]
                                };
                                LIST_MainQueue.Add(_mq_t);
                            }
                        },
                        () => {
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
                        }

                        );
                    Console.WriteLine("Initializing lists finished.");
                    try
                    {
                        Parallel.Invoke(
                            async () =>
                            {
                                foreach (_Queue_Info a in LIST_QueueInfo)
                                    await fcon.App_Insert_QueueInfoAsync(a);
                            }
                            ,
                            async () =>
                            {
                                // await fcon.App_Delete_MainQueueAsync(); // April 02, 2018
                                fromOnline_MainQueue = await fcon.CQN_Retrieve_MainQueueAsync();
                                LIST_MainQueue_fromFirebase = await fcon.App_Retrieve_MainQueueAsync();

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

                                 Console.WriteLine();
                                Console.WriteLine("List of items found on local but not online");
                                foreach (_Main_Queue a in LOCAL_MainQueueList)
                                {
                                    Console.WriteLine("<< {0} >>", a.Customer_Queue_Number);
                                    // If not on Firebase but exists on Local (new updates) => Insert
                                    if (a.Type == "Guest")
                                        await fcon.App_Insert_MainQueueAsync(a, true);
                                    else
                                        await fcon.App_Insert_MainQueueAsync(a, false);
                                }
                                Console.WriteLine("Supposed items found online but not local");
                                foreach (_Main_Queue b in ONLINE_MainQueueList)
                                {
                                    Console.WriteLine("## {0} {1} ##", b.Customer_Queue_Number, b.Key);
                                    // If not on Local but exists on Firebase (outdated) => Delete
                                    if (b.Type == "Student")
                                        await fcon.Specific_Delete_MainQueueAsync(b.Student_No);
                                    else
                                        await fcon.Specific_Delete_MainQueueAsync(b.Key);
                                }
                                Console.WriteLine("List of items found on both");
                                foreach (_Main_Queue c in COMBINE_MainQueueList)
                                {
                                    Console.WriteLine("/- {1} {0}-/", c.Key, c.Customer_Queue_Number);
                                    if (c.Type == "Student")
                                        await fcon.App_Insert_MainQueueAsync(c, false);
                                    else
                                        await fcon.App_Update_MainQueue(c);
                                }

                            },
                            async () =>
                            {
                                foreach (_Transfer_Queue c in LIST_TransferQueue)
                                {
                                    if (c.Type == "Guest")
                                        await fcon.App_Insert_TransferQueueAsync(c, true);
                                    else
                                        await fcon.App_Insert_TransferQueueAsync(c, false);
                                }

                            },
                            async () =>
                            {
                                foreach (_Servicing_Terminal d in LIST_ServicingTerminal)
                                    await fcon.App_Insert_ServicingTerminalAsync(d);
                            },
                            async () => 
                            {
                                PREQUEUE_LIST = await fcon.App_Retrieve_PreQueue();
                                foreach (_Pre_Queue e in PREQUEUE_LIST)
                                {
                                    int firstServicingOffice = getFirstServicingOffice(e.Transaction_Type);
                                    int newQueueNumber = getQueueNumber(firstServicingOffice);
                                    _pq_mq = new _Main_Queue
                                    {
                                        Queue_Number = newQueueNumber,
                                        Servicing_Office = firstServicingOffice,
                                        Pattern_Max = retrievePatternMax(e.Transaction_Type),
                                        Customer_Queue_Number = generateQueueShortName(e.Transaction_Type,newQueueNumber),
                                        Full_Name = e.Full_Name,
                                        Student_No = e.Student_No,
                                        Transaction_Type = e.Transaction_Type,
                                        Time =  UnixTimeStampToDateTime(e.timestamp_date),
                                        Pattern_Current = 1,
                                        Queue_Status = "Waiting"
                                    };
                                    PREQUEUE_TO_MAINQUEUE.Add(_pq_mq);
                                }

                                string QUERY_Add_PreQueue_To_MainQueue = "insert into Main_Queue " +
                                "(Queue_Number,Full_Name,Servicing_Office,Student_No," +
                                "Transaction_Type,Type,Time,Pattern_Current,Pattern_Max," +
                                "Customer_Queue_Number,Queue_Status) " +
                                " values (@q_qn,@q_fn,@q_so,@q_sn,@q_tt,1,GETDATE(),@q_pc,@q_pm,@q_cqn,@q_qs)";
                                SqlConnection temp_con = new SqlConnection(connection_string);
                                SqlCommand cmdPreQueue = new SqlCommand(QUERY_Add_PreQueue_To_MainQueue, temp_con);
                                using (temp_con)
                                {
                                    temp_con.Open();
                                    foreach (_Main_Queue f in PREQUEUE_TO_MAINQUEUE)
                                    {

                                        Console.WriteLine("=== {0} ===", f.Customer_Queue_Number);
                                        // Add to local DB -> MainQueue
                                        cmdPreQueue.Parameters.AddWithValue("@q_qn", f.Queue_Number);
                                        cmdPreQueue.Parameters.AddWithValue("@q_fn", f.Full_Name);
                                        cmdPreQueue.Parameters.AddWithValue("@q_so", f.Servicing_Office);
                                        cmdPreQueue.Parameters.AddWithValue("@q_sn", f.Student_No);
                                        cmdPreQueue.Parameters.AddWithValue("@q_tt", f.Transaction_Type);
                                        cmdPreQueue.Parameters.AddWithValue("@q_pc", f.Pattern_Current);
                                        cmdPreQueue.Parameters.AddWithValue("@q_pm", f.Pattern_Max);
                                        cmdPreQueue.Parameters.AddWithValue("@q_cqn", f.Customer_Queue_Number);
                                        cmdPreQueue.Parameters.AddWithValue("@q_qs", f.Queue_Status);
                                        cmdPreQueue.ExecuteNonQuery();
                                        cmdPreQueue.Parameters.Clear();

                                    }
                                    temp_con.Close();
                                }
                                // Delete after retrieving all PreQueue and inserting them to MainQueue
                                
                                 await fcon.App_Delete_PreQueueAsync();
                            }
                            );
                        Console.WriteLine("Last work for synching finished. /n /n /n ");
                        Console.WriteLine("------------------------------------------");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Problem when connecting online.");
                        Console.WriteLine("Check internet connection.");
                    }

                    con.Close();
                    }
                
                counter++;
                Console.WriteLine("How many times update run? -> " + counter + " at TIME -> " + stopp.Elapsed);
                Console.WriteLine("Hours at " + stopp.Elapsed.Hours);
                Console.WriteLine("Minutes at " + stopp.Elapsed.Minutes);
                Console.WriteLine("Seconds at " + stopp.Elapsed.Seconds);
                Console.WriteLine("TotalSeconds at " + stopp.Elapsed.TotalSeconds);
            }
            else
            {
                Console.WriteLine("Sync is not allowed.");
            }
                
        }
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
                Console.Write(" generateQueueShortName - > searching for short name");
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
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = VARIABLE_Priority_Sync_Time; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            PROGRAM_Online_Sync();
        }
    }
}
