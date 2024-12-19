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
    public partial class AdminChangePassword : Form
    {
        public string Username { get; set; }

        public AdminChangePassword(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;

        }

        public AdminChangePassword()
        {
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Admin admin = new Admin(Username);
            this.Hide();
            admin.ShowDialog();
        }

        private void FormAdminListOfUser_Load(object sender, EventArgs e)
        {
           /* label2.Parent = pictureBox1;
            label2.BackColor = Color.Transparent;
            label3.Parent = pictureBox1;
            label3.BackColor = Color.Transparent;
            pictureBox3.Parent = pictureBox1;
            pictureBox3.BackColor = Color.Transparent;*/
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Login login = new Login(Username);
            login.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminPatientRecord adminPatientRecord = new AdminPatientRecord(Username);
            adminPatientRecord.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdminAboutUs adminAboutUs = new AdminAboutUs(Username);
            adminAboutUs.Show();
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
    }
}
