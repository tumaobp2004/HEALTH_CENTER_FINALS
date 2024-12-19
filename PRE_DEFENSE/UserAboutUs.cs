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
    public partial class UserAboutUs : Form
    {
        public string Username { get; set; }

        public UserAboutUs(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;
        }

        private void button10_Click(object sender, EventArgs e)
        {
           UserProfile userProfile = new UserProfile(Username);
           userProfile.Show();
           this.Close();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            UserChangePassword userChangePassword = new UserChangePassword(Username);
            userChangePassword.Show();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            UserAboutUs userAboutUs = new UserAboutUs(Username);
            userAboutUs.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Login login = new Login(Username);
            login.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User user = new User(Username);
            user.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PatientRecord patient = new PatientRecord(Username);
            patient.Show();
            this.Close();
        }

      

        private void button2_Click(object sender, EventArgs e)
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
    }
}
