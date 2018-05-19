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
    public partial class Evaluation : Form
    {
        private String connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["dbString"].ConnectionString;
        
        public Evaluation()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
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
                Name = "All Offices",
                Address = "None",
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
        private void Evaluation_Load(object sender, EventArgs e)
        {
            officeBox.DataSource = LIST_getServicingOffices();
            officeBox.DisplayMember = "Name";
            officeBox.ValueMember = "id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label5.ResetText();
            label6.ResetText();
            label10.ResetText();
            label12.ResetText();
            SqlConnection con = new SqlConnection(connection_string);
            try
            {
                con.Open();
                int Students = 0; int Guests = 0; float all_score = 0; float stud_score = 0; float guest_score = 0;
                int temp1 = 0; int temp2 = 0; int temp3 = 0;
                string feedback_query = "select * from Feedbacks where Date_Of_Feedback BETWEEN @param1 and @param2 and Score > 0";
                string feedback2_query = "select * from Feedbacks where Date_Of_Feedback BETWEEN @param1 and @param2 and Servicing_Office = @param3 and Score > 0";
                SqlDataReader rdr;
                SqlCommand f_cmd = new SqlCommand();
                if ((int)officeBox.SelectedValue > 0)
                {
                    Console.WriteLine(feedback2_query);
                    Console.WriteLine((int)officeBox.SelectedValue);
                    f_cmd = new SqlCommand(feedback2_query, con);
                    f_cmd.Parameters.AddWithValue("@param1", dateTimePicker1.Value);
                    f_cmd.Parameters.AddWithValue("@param2", dateTimePicker2.Value);
                    f_cmd.Parameters.AddWithValue("@param3", (int)officeBox.SelectedValue);
                }
                else
                {
                    Console.WriteLine(feedback_query);
                    f_cmd = new SqlCommand(feedback_query, con);
                    f_cmd.Parameters.AddWithValue("@param1", dateTimePicker1.Value);
                    f_cmd.Parameters.AddWithValue("@param2", dateTimePicker2.Value);
                }
                Console.WriteLine(dateTimePicker1.Value);
                Console.WriteLine(dateTimePicker2.Value);
                Console.WriteLine((int)officeBox.SelectedValue);
                rdr = f_cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if ((bool)rdr["isStudent"] == true)
                    {
                        temp1 += (int)rdr["Score"];
                        Students++;
                    }
                    else if ((bool)rdr["isStudent"] == false)
                    {
                        temp2 += (int)rdr["Score"];
                        Guests++;
                    }
                    temp3 += (int)rdr["Score"];
                    Console.WriteLine("Servicing_Office : " + (int)rdr["Servicing_Office"] + " Score :" + (int)rdr["Score"]);
                }
                if (temp1 != 0 && Students != 0)
                    stud_score = temp1 / Students;
                if (temp2 != 0 && Guests != 0 )
                    guest_score = temp2 / Guests;
                if (temp3 != 0 && Students != 0 || Guests != 0)
                    all_score = temp3 / (Students + Guests);
                // please check average score here
                //f_cmd.Parameters.Clear();
                string a = (Students > 1 ? "Students" : "Student");
                string b = (Guests > 1 ? "Guests" : "Guest");
                label5.Text = Students + " " + a + " and " + Guests + " " + b;
                label6.Text = all_score.ToString();
                label10.Text = stud_score.ToString();
                label12.Text = guest_score.ToString();
                con.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Please check connection to local database.", "Database Error!");
            }
            catch (Exception ea)
            {
                MessageBox.Show("Something went wrong. Error -> " + ea.Message);
            }
        }
    }
}
