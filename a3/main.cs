using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        public main()
        {
            InitializeComponent();

            // Variable Init
            VARIABLE_Allowed_To_Sync = true;
            VARIABLE_Priority_Sync_Time = 5000;
            VARIABLE_System_Sync_Time = 60000;

            // Function Init
            PROGRAM_Online_Sync(); //This can be transferred on a button (switch) to manually start. Currently on automatic.
            InitSyncTimer();

        }
        private async void PROGRAM_Online_Sync()
        {
            //This function fetches data from the local database, then uploads it online.
            if (VARIABLE_Allowed_To_Sync)
            {
                SqlConnection con = new SqlConnection(connection_string);
                String QUERY_Select_QueueInfo = "select * from Queue_Info";
                String QUERY_Select_MainQueue = "select * from Main_Queue";
                String QUERY_Select_TransferQueue = "select * from Transfer_Queue";
                String QUERY_Select_TransactionQueue = "select * from Queue_Transaction";
                using (con)
                {
                    con.Open();
                    SqlCommand CMD_select_QueueInfo = new SqlCommand(QUERY_Select_MainQueue, con);
                    SqlDataReader RDR_select;
                    RDR_select = CMD_select_QueueInfo.ExecuteReader();
                    firebase_Connection fcon = new firebase_Connection();
                    while (RDR_select.Read())
                    {
                        // set the class
                        _mq = new _Main_Queue
                        {
                            Queue_Number = (int)RDR_select["Queue_Number"],
                            Full_Name = (string)RDR_select["Full_Name"],
                            Servicing_Office = (int)RDR_select["Servicing_Office"],
                            Transaction_Type = (int)RDR_select["Transaction_Type"],
                            Type = ((Boolean)RDR_select["Type"] == false) ? "Student" : "Guest",
                            Customer_Queue_Number = (string)RDR_select["Customer_Queue_Number"],
                            ID = (int)RDR_select["id"],
                            Pattern_Current = (int)RDR_select["Pattern_Current"],
                            Time = (DateTime)RDR_select["Time"],
                            Student_No = (string)RDR_select["Student_No"]
                        };
                        // insert it to temporary datatable
                        await fcon.App_Insert_MainQueueAsync(_mq);
                        
                    }

                    counter++;
                    Console.WriteLine("Sync? at " + counter);
                    con.Close();
                }
                
            }
            else
            {
                Console.WriteLine("Sync is not allowed.");
            }
                
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
