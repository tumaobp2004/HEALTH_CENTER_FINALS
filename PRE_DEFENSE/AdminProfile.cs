using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace PRE_DEFENSE
{
    public partial class AdminProfile : Form
    {

        private string OriginalFirstName;
        private string OriginalLastName;
        private string OriginalContactNumber;
        private string OriginalEmail;

        private readonly string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\ASUS\\Desktop\\c#\\PRE_DEFENSE\\PRE_DEFENSE\\bin\\Debug\\Login.accdb";
        private bool isEditing = false; 

        public string Username { get; set; }
        public AdminProfile(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;    

            
            btnSave.Visible = false;
            btnCancel.Visible = false;
            Username = loggedInUsername;
        }
        private string GetLoggedInUsername(string username)
        {
            return ""; 
        }

        private void LoadProfileData()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "SELECT FirstName, LastName, ContactNumber, Email, Username, ID FROM Account WHERE Username = ?";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", Username);
                        conn.Open();

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tbFname.Text = reader["FirstName"].ToString();
                                tbLname.Text = reader["LastName"].ToString();
                                tbCnumber.Text = reader["ContactNumber"].ToString();
                                tbEmail.Text = reader["Email"].ToString();
                                tbUsername.Text = reader["Username"].ToString();
                                tbID.Text = reader["ID"].ToString();

                                OriginalFirstName = tbFname.Text;
                                OriginalLastName = tbLname.Text;
                                OriginalContactNumber = tbCnumber.Text;
                                OriginalEmail = tbEmail.Text;
                            }
                            else
                            {
                                MessageBox.Show($"No data found for the username: {Username}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveProfileData()
        {
            if (!ValidateInputs())
            {
                MessageBox.Show("Please fill in all fields before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool isChanged = false;
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "UPDATE Account SET ";
                    List<OleDbParameter> parameters = new List<OleDbParameter>();

                    if (tbFname.Text.Trim() != OriginalFirstName)
                    {
                        query += "FirstName = ?, ";
                        parameters.Add(new OleDbParameter("@FirstName", tbFname.Text.Trim()));
                        isChanged = true;
                    }

                    if (tbLname.Text.Trim() != OriginalLastName)
                    {
                        query += "LastName = ?, ";
                        parameters.Add(new OleDbParameter("@LastName", tbLname.Text.Trim()));
                        isChanged = true;
                    }

                    if (tbCnumber.Text.Trim() != OriginalContactNumber)
                    {
                        query += "ContactNumber = ?, ";
                        parameters.Add(new OleDbParameter("@ContactNumber", tbCnumber.Text.Trim()));
                        isChanged = true;
                    }

                    if (tbEmail.Text.Trim() != OriginalEmail)
                    {
                        query += "Email = ?, ";
                        parameters.Add(new OleDbParameter("@Email", tbEmail.Text.Trim()));
                        isChanged = true;
                    }

                    if (!isChanged)
                    {
                        MessageBox.Show("No changes were made.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    query = query.TrimEnd(',', ' ') + " WHERE Username = ?";
                    parameters.Add(new OleDbParameter("@Username", Username));

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ToggleEditing(false);
                            LoadProfileData();
                        }
                        else
                        {
                            MessageBox.Show("No changes were made.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving profile data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            
            return !string.IsNullOrWhiteSpace(tbFname.Text) &&
                   !string.IsNullOrWhiteSpace(tbLname.Text) &&
                   !string.IsNullOrWhiteSpace(tbCnumber.Text) &&
                   !string.IsNullOrWhiteSpace(tbEmail.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                DialogResult result = MessageBox.Show("Do you want to save the changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveProfileData();
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            ToggleEditing(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to discard the changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                LoadProfileData();
                ToggleEditing(false); 
            }
        }

        private void ToggleEditing(bool isEditing)
        {

            this.isEditing = isEditing;

            tbFname.ReadOnly = !isEditing;
            tbLname.ReadOnly = !isEditing;
            tbCnumber.ReadOnly = !isEditing;
            tbEmail.ReadOnly = !isEditing;

            btnSave.Visible = isEditing;
            btnCancel.Visible = isEditing;

            btnEditProfile.Visible = !isEditing;
        }


        private void btnHome_Click_1(object sender, EventArgs e)
        {
            Admin admin = new Admin(Username);
            admin.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Login login = new Login(Username);
            login.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminPatientRecord record = new AdminPatientRecord(Username);   
            record.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdminAboutUs adminAboutUs = new AdminAboutUs(Username);
            adminAboutUs.Show();
            this.Hide();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AdminProfile_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Username))
            {
                MessageBox.Show("Invalid username. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadProfileData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UserManagement management = new UserManagement(Username);
            management.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AdminChangePassword adminChangePassword = new AdminChangePassword(Username);
            adminChangePassword.Show();
            this.Close();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Admin admin = new Admin(Username);
            admin.Show();
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
        }


        private void button8_Click_1(object sender, EventArgs e)
        {
            adminAddConsultation adminAddConsultation = new adminAddConsultation(Username);
            adminAddConsultation.Show();
            this.Hide();
        }

        private void btnMedicine_Click(object sender, EventArgs e)
        {
            adminMEDICINE admin = new adminMEDICINE(Username);
            admin.Show();
            this.Close();
        }
    }
}
