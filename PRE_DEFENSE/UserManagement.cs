using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;


namespace PRE_DEFENSE
{
    public partial class UserManagement : Form
    {

        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\ASUS\Desktop\c#\PRE_DEFENSE\PRE_DEFENSE\bin\Debug\Login.accdb";
        private int currentID = -1; 
        OleDbConnection conn = new OleDbConnection();
        private OleDbDataAdapter dataAdapter;
        private DataTable dt; 

        public string Username { get; set; }

        public UserManagement(string loggedInUsername)
        {
            InitializeComponent();
            OleDbConnection conn = new OleDbConnection(connectionString);
            conn = new OleDbConnection(connectionString);
            LoadPatients();
            Username = loggedInUsername;


        }

        private void DisableInputFields()
        {
            /*// Disable all input fields initially
            txtFirstName.Enabled = false;
            txtLastName.Enabled = false;
            txtAge.Enabled = false;
            cmbGender.Enabled = false;
            cmbCivilStatus.Enabled = false;
            txtAddress.Enabled = false;
            cmbTypeOfID.Enabled = false;
            txtIDNumber.Enabled = false;
            dtp.Enabled = false;*/
        }
        private void EnableInputFields()
        {
            tbUN.Enabled = true;
            tbPW.Enabled = true;
            cmbType.Enabled = true;
            tbFN.Enabled = true;
            tbLN.Enabled = true;
            tbCnumber.Enabled = true;
            tbEmail.Enabled = true;
        }



        private void ToggleAddSaveCancelButtons(bool isAddMode)
        {
            btnSave.Visible = isAddMode;
            btnCancel.Visible = isAddMode;
            btnAdd.Visible = !isAddMode;
            btnUpdate.Visible = !isAddMode;
            btnDelete.Visible = !isAddMode; 
        }

        private void LoadPatients()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "SELECT ID, Username, Password, Type, FirstName, LastName, ContactNumber, Email FROM Account";
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dgwUser.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }


        private void GetUser()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))  
            {
                string query = "SELECT * FROM Account";
                dataAdapter = new OleDbDataAdapter(query, conn); 
                dt = new DataTable(); 
                dataAdapter.Fill(dt); 
                dgwUser.DataSource = dt; 
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ToggleAddSaveCancelButtons(true);
            btnDelete.Visible = false;
            EnableInputFields();
            ClearInputFields();
            tbUN.Focus();

        }
        
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string updateQuery = "UPDATE Account SET [Username] = ?, [Password] = ?, [Type] = ?, [FirstName] = ?, [LastName] = ?, [ContactNumber] = ?, [Email] = ? WHERE [ID] = ?";
                    OleDbCommand cmd = new OleDbCommand(updateQuery, conn);

                    // Assign parameters
                    cmd.Parameters.AddWithValue("?", tbUN.Text.Trim());
                    cmd.Parameters.AddWithValue("?", tbPW.Text.Trim());
                    cmd.Parameters.AddWithValue("?", cmbType.Text.Trim());
                    cmd.Parameters.AddWithValue("?", tbFN.Text.Trim());
                    cmd.Parameters.AddWithValue("?", tbLN.Text.Trim());
                    cmd.Parameters.AddWithValue("?", tbCnumber.Text.Trim());
                    cmd.Parameters.AddWithValue("?", tbEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("?", currentID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record updated successfully.");
                }

                LoadPatients();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating record: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string deleteQuery = "DELETE FROM Account WHERE ID = ?";
                    OleDbCommand cmd = new OleDbCommand(deleteQuery, conn);

                    cmd.Parameters.AddWithValue("?", tbID.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record deleted successfully.");
                }

                LoadPatients();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting record: " + ex.Message);
            }
        }

        private void tbS_TextChanged(object sender, EventArgs e)
        {

        }

        private void UserManagement_Load(object sender, EventArgs e)
        {
            LoadUsers();
            LoadPatients();
            ToggleAddSaveCancelButtons(false);
            DisableInputFields();
        }

        private void dgwUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgwUser.Rows[e.RowIndex];
                tbID.Text = row.Cells["ID"].Value.ToString();
                tbUN.Text = row.Cells["Username"].Value.ToString();
                tbPW.Text = row.Cells["Password"].Value.ToString();
                cmbType.Text = row.Cells["Type"].Value.ToString();
                tbFN.Text = row.Cells["FirstName"].Value.ToString();
                tbLN.Text = row.Cells["LastName"].Value.ToString();
                tbCnumber.Text = row.Cells["ContactNumber"].Value.ToString();
                tbEmail.Text = row.Cells["Email"].Value.ToString();

                currentID = int.Parse(tbID.Text);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string insertQuery = "INSERT INTO Account ([Username], [Password], [Type], [FirstName], [LastName], [ContactNumber], [Email]) " +
                                         "VALUES (@username, @password, @type, @fname, @lname, @cnum, @email)";

                    OleDbCommand cmd = new OleDbCommand(insertQuery, conn);

                    cmd.Parameters.AddWithValue("@username", tbUN.Text.Trim());
                    cmd.Parameters.AddWithValue("@password", tbPW.Text.Trim());
                    cmd.Parameters.AddWithValue("@type", cmbType.Text.Trim());
                    cmd.Parameters.AddWithValue("@fname", tbFN.Text.Trim());
                    cmd.Parameters.AddWithValue("@lname", tbLN.Text.Trim());
                    cmd.Parameters.AddWithValue("@cnum", tbCnumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", tbEmail.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("New record added successfully.");
                }

                LoadUsers();

                ClearInputFields();
                ToggleAddSaveCancelButtons(false);
                DisableInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data: " + ex.Message);
            }
        }


        private void ClearInputFields()
        {
            tbUN.Clear();
            tbPW.Clear();
            cmbType.SelectedIndex = -1;
            tbFN.Clear();
            tbLN.Clear();
            tbCnumber.Clear();
            tbEmail.Clear();
            tbID.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearInputFields();
            ToggleAddSaveCancelButtons(false);
            DisableInputFields();
        }

        private void tbS_TextChanged_1(object sender, EventArgs e)
        {
            string searchTerm = tbS.Text.ToLower(); 

            
            if (string.IsNullOrEmpty(searchTerm))
            {
                
                LoadPatients();
            }
            else
            {
                SearchUsers(searchTerm);
            }
        }
        private void SearchUsers(string searchTerm)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "SELECT * FROM Account WHERE [ID] LIKE ? OR [FirstName] LIKE ?";
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, conn);

                    dataAdapter.SelectCommand.Parameters.AddWithValue("?", "%" + searchTerm + "%");
                    dataAdapter.SelectCommand.Parameters.AddWithValue("?", "%" + searchTerm + "%");

                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dgwUser.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching data: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminPatientRecord adminPatientRecord = new AdminPatientRecord(Username);
            adminPatientRecord.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdminProfile adminProfile = new AdminProfile(Username);
            adminProfile.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AdminChangePassword adminChangePassword = new AdminChangePassword(Username);
            adminChangePassword.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdminAboutUs adminAboutUs = new AdminAboutUs(Username);
            adminAboutUs.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Login login = new Login(Username);
            login.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Admin admin = new Admin(Username);
            admin.Show();
            this.Hide();
        }
       


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LoadUsers()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "SELECT ID, Username, Password, Type, FirstName, LastName, ContactNumber, Email FROM Account";
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dgwUser.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            UserManagement userManagement = new UserManagement (Username);
            userManagement.Show();
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
            adminMEDICINE adminMEDICINE = new adminMEDICINE(Username);
            adminMEDICINE.Show();
            this.Hide();
        }
    }
}

