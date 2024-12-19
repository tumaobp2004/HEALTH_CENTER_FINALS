using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRE_DEFENSE
{
    public partial class AdminAboutUs : Form
    {

        public string Username { get; set; }
        public AdminAboutUs(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminPatientRecord adminPatient = new AdminPatientRecord(Username);
            adminPatient.Show();
            this.Hide();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            UserManagement management = new UserManagement(Username);
           management.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdminProfile adminProfile = new AdminProfile(Username);
            adminProfile.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Login login = new Login(Username);
            login.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AdminChangePassword adminChangePassword = new AdminChangePassword(Username);
            adminChangePassword.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Admin admin = new Admin(Username);
            admin.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
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
