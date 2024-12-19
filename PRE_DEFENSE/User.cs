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
    public partial class User : Form
    {

        public string Username { get; set; }

        public User(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;
        }


        private void btnRecord_Click_1(object sender, EventArgs e)
        {
            PatientRecord record = new PatientRecord(Username);
            record.ShowDialog();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserProfile profile = new UserProfile(Username);
            profile.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UserChangePassword cpass = new UserChangePassword(Username);
            cpass.ShowDialog();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserAboutUs aboutUs = new UserAboutUs(Username);
            aboutUs.ShowDialog();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Login login = new Login(Username);
            login.Show(); 
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.Yellow;
            PatientRecord rec = new PatientRecord(Username);
            rec.Show();
           
            this.Hide();
            

            // Itago ang Form1
            this.Hide();
        }

        private void User_Load(object sender, EventArgs e)
        {
            
            int totalPatients = GetTotalPatients();
            lblTotalPatients.Text = "TOTAL RECORD OF PATIENT :  "    +  totalPatients.ToString();
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

        private void lblTotalPatients_Click(object sender, EventArgs e)
        {

        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            AddConsultation addConsultation = new AddConsultation(Username);
            addConsultation.Show();
            this.Hide();
        }

        private void btnMedicine_Click(object sender, EventArgs e)
        {
            MEDICINE mEDICINE = new MEDICINE(Username);
            mEDICINE.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
