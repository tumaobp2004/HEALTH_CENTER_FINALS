using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PRE_DEFENSE
{
    public partial class UserChangePassword : Form
    {
        // Connection string para sa SQL Server (palitan ayon sa iyong setup)
        private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\ASUS\\Desktop\\c#\\PRE_DEFENSE\\PRE_DEFENSE\\bin\\Debug\\Login.accdb"; 

        public string Username { get; set; }

        public UserChangePassword(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = tbUN.Text;
            string currentPassword = tbOldPW.Text;
            string newPassword = tbNewPW.Text;
            string confirmPassword = tbConfirmPW.Text;

            if (VerifyCredentials(username, currentPassword))
            {
                if (newPassword == confirmPassword)
                {
                    if (ChangePassword(username, newPassword))
                    {
                        MessageBox.Show("Password successfully changed!");
                    }
                    else
                    {
                        MessageBox.Show("Error: Failed to change password.");
                    }
                }
                else
                {
                    MessageBox.Show("Error: New password and confirmation do not match.");
                }
            }
            else
            {
                MessageBox.Show("Error: Invalid username or current password.");
            }
        }

        private bool VerifyCredentials(string username, string currentPassword)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Account WHERE Username = @Username AND Password = @Password";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", currentPassword);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }

        private bool ChangePassword(string username, string newPassword)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                // I-verify na ang table name ay tama at ang column ng Password
                string query = "UPDATE Account SET Password = @NewPassword WHERE Username = @Username";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@NewPassword", newPassword);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;  // Kung mayroong pagbabago, magbabalik ng true
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }

        private void ChangePasswordForm_Load(object sender, EventArgs e)
        {
          
        }


        private void btnHome_Click(object sender, EventArgs e)
        {
            User u = new User(Username);
            u.Show();
            this.Hide();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            PatientRecord rec = new PatientRecord(Username);
            rec.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserProfile prof = new UserProfile(Username);
            prof.Show();
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
            UserAboutUs us = new UserAboutUs(Username);
            us.Show();
            this.Hide();

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
