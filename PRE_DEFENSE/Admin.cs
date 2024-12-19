using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRE_DEFENSE
{
    public partial class Admin : Form
    {
        public string Username { get; set; }


        public Admin(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;
        }
      

        private void button3_Click(object sender, EventArgs e)
        {
            AdminProfile adminProfile = new AdminProfile(Username);
            adminProfile.Show();    
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AdminChangePassword adminChangePassword = new AdminChangePassword();
            adminChangePassword.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Login log = new Login(Username);
            log.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdminAboutUs adminAboutUs = new AdminAboutUs(Username); 
            adminAboutUs.Show();
            this.Hide();
        }

        private void btnManageUser_Click(object sender, EventArgs e)
        {
            UserManagement userManagement = new UserManagement(Username);
            userManagement.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminPatientRecord adminPatient = new AdminPatientRecord(Username);
            adminPatient.Show();
            this.Hide();
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            int totalPatients = GetTotalPatients();
            lblTotalPatients.Text = "TOTAL RECORD OF PATIENTS: " + totalPatients.ToString();
        }

        private int GetTotalPatients()
        {
            int count = 0;
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\ASUS\\Desktop\\c#\\PRE_DEFENSE\\PRE_DEFENSE\\bin\\Debug\\PatientDatabase.accdb";  // Palitan ng tamang path

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Patients";  
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    count = Convert.ToInt32(cmd.ExecuteScalar()); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            return count;
        }
        private void GetTotalUsers()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\ASUS\Desktop\c#\PRE_DEFENSE\PRE_DEFENSE\bin\Debug\Login.accdb";
            string query = "SELECT COUNT(*) FROM Account";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        int totalUsers = Convert.ToInt32(command.ExecuteScalar());
                        MessageBox.Show("TOTAL USERS: " + totalUsers, "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void btnGetTotalUsers_Click(object sender, EventArgs e)
        {
            GetTotalUsers();
        }

        private void lblTotalPatients_Click(object sender, EventArgs e)
        {

        }


        private void button6_Click(object sender, EventArgs e)
        {
            adminAddConsultation adminAddConsultation = new adminAddConsultation(Username);
            adminAddConsultation.Show();
            this.Hide();
        }

        private void btnMedicine_Click(object sender, EventArgs e)
        {
            adminMEDICINE admin = new adminMEDICINE(Username);
            admin.Show();
            this.Hide();
        }
    }
}
