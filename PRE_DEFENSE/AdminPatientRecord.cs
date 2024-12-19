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
    public partial class AdminPatientRecord : Form
    {

        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\ASUS\Desktop\c#\PRE_DEFENSE\PRE_DEFENSE\bin\Debug\Patient.accdb";
        private int currentID = -1;  
        private OleDbDataAdapter dataAdapter; 
        private DataTable dt; 
        

        public string Username { get; set; }

        public AdminPatientRecord(string loggedInUsername)
        {
            InitializeComponent();
            ToggleAddSaveCancelButtons(false); 
            LoadPatients();
            DisableInputFields();  
            Username = loggedInUsername;
            txtPhone.KeyPress += TxtPhone_KeyPress;

        }
        private void TxtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and control keys (like backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // Prevent entering more than 11 characters
            if (txtPhone.Text.Length >= 11 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
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


        private void ToggleAddSaveCancelButtons(bool isAddMode)
        {
            if (isAddMode)
            {
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnAdd.Visible = false;
                btnUpdate.Visible = false;
            }
            else
            {
                btnSave.Visible = false;
                btnCancel.Visible = false;
                btnAdd.Visible = true;
                btnUpdate.Visible = true;
            }
        }

        private void GetPatients()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))  
            {
                string query = "SELECT * FROM Patient";
                dataAdapter = new OleDbDataAdapter(query, conn); 
                dt = new DataTable();  
                dataAdapter.Fill(dt); 
                dgwPatients.DataSource = dt; 
            }
        }



        private void btnHome_Click(object sender, EventArgs e)
        {
            Admin admin = new Admin(Username);
            this.Hide();
            admin.ShowDialog();

        }



        private void FormAdminListOfRecord_Load(object sender, EventArgs e)
        {

            dtp.Value = DateTime.Now;
            dtp.Enabled = false; // Make the DateTimePicker non-editable

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM Patient";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dgwPatients.DataSource = dataTable;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ToggleAddSaveCancelButtons(true);

            btnDelete.Visible = false;

            txtFirstName.Enabled = true;
            txtLastName.Enabled = true;
            txtAge.Enabled = true;
            cmbGender.Enabled = true;
            cmbCivilStatus.Enabled = true;
            cmbAddress.Enabled = true;
            txtPhone.Enabled = true;
            dtp.Enabled = true;

            txtFirstName.Clear();
            txtLastName.Clear();
            txtAge.Clear();
            cmbGender.SelectedIndex = -1;
            cmbCivilStatus.SelectedIndex = -1;
            cmbAddress.SelectedIndex = -1;
            txtPhone.Clear();
            dtp.Value = DateTime.Now;

            dtp.Enabled = false; // Make the DateTimePicker non-editable
        }

        private void dgwPatients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgwPatients.SelectedRows.Count > 0)
            {
                var row = dgwPatients.SelectedRows[0];
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtLastName.Text = row.Cells["LastName"].Value.ToString();
                txtAge.Text = row.Cells["Age"].Value.ToString();
                cmbGender.Text = row.Cells["Gender"].Value.ToString();
                cmbCivilStatus.Text = row.Cells["CivilStatus"].Value.ToString();
                cmbAddress.Text = row.Cells["Address"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                dtp.Value = Convert.ToDateTime(row.Cells["RegisteredDateTime"].Value);

                currentID = Convert.ToInt32(row.Cells["PatientID"].Value);

                btnUpdate.Visible = true;

                btnDelete.Visible = true;

            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {

            ToggleAddSaveCancelButtons(false);
            LoadPatients(); 
            ToggleAddSaveCancelButtons(false);

            // Clear the input fields
            txtFirstName.Clear();
            txtLastName.Clear();
            dtpBirthDate.Value = DateTime.Now;
            txtAge.Clear();
            cmbGender.SelectedIndex = -1;
            cmbCivilStatus.SelectedIndex = -1;
            cmbAddress.SelectedIndex = -1;
            txtPhone.Clear();
            dtp.Value = DateTime.Now;

            txtFirstName.Enabled = true;
            txtLastName.Enabled = true;
            dtpBirthDate.Enabled = true;
            txtAge.Enabled = true;
            cmbGender.Enabled = true;
            cmbCivilStatus.Enabled = true;
            cmbAddress.Enabled = true;
            txtPhone.Enabled = true;
            dtp.Enabled = true;

            ClearInputFields();
            ToggleAddSaveCancelButtons(false);
            LoadPatients();
            DisableInputFields();
        }

        private void tbS_TextChanged(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM Patient WHERE PatientID LIKE ? OR FirstName LIKE ? OR LastName LIKE ?";
                OleDbDataAdapter searchAdapter = new OleDbDataAdapter(query, conn);

                searchAdapter.SelectCommand.Parameters.AddWithValue("?", "%" + tbS.Text + "%"); 
                searchAdapter.SelectCommand.Parameters.AddWithValue("?", "%" + tbS.Text + "%"); 
                searchAdapter.SelectCommand.Parameters.AddWithValue("?", "%" + tbS.Text + "%"); 

                DataTable searchTable = new DataTable();
                searchAdapter.Fill(searchTable);
                dgwPatients.DataSource = searchTable;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string insertQuery = "INSERT INTO Patient (FirstName, LastName, BirthDate, Age, Gender, CivilStatus, Address, Phone, RegisteredDateTime) " +
                                     "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

                OleDbCommand cmd = new OleDbCommand(insertQuery, conn);

                // Adding parameters to the command
                cmd.Parameters.AddWithValue("?", txtFirstName.Text.Trim());
                cmd.Parameters.AddWithValue("?", txtLastName.Text.Trim());
                cmd.Parameters.AddWithValue("?", dtpBirthDate.Value.ToString("MM/dd/yyyy"));

                // Validate age input
                if (int.TryParse(txtAge.Text.Trim(), out int age))
                {
                    cmd.Parameters.AddWithValue("?", age);
                }
                else
                {
                    MessageBox.Show("Please enter a valid age.");
                    return;
                }

                cmd.Parameters.AddWithValue("?", cmbGender.Text);
                cmd.Parameters.AddWithValue("?", cmbCivilStatus.Text);
                cmd.Parameters.AddWithValue("?", cmbAddress.Text);
                cmd.Parameters.AddWithValue("?", txtPhone.Text.Trim());

                cmd.Parameters.AddWithValue("?", dtp);

                try
                {
                    conn.Open(); // Open the database connection
                    cmd.ExecuteNonQuery(); // Execute the query

                    MessageBox.Show("New record added successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message); // Show error message in case of an exception
                }
                finally
                {
                    conn.Close(); // Close the database connection
                }

                // Refresh the list of patients
                LoadPatients();

                // Clear the input fields
                ClearInputFields();

                // Toggle UI controls as needed
                ToggleAddSaveCancelButtons(false);
                DisableInputFields();
                dtp.Value = DateTime.Now;

            }
        }

        private void ClearInputFields()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            dtpBirthDate.Value = DateTime.Now;
            txtAge.Clear();
            cmbGender.SelectedIndex = -1;
            cmbCivilStatus.SelectedIndex = -1;
            cmbAddress.SelectedIndex = -1;
            txtPhone.Clear();
            dtp.Value = DateTime.Now;
        }



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgwPatients.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to update.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string updateQuery = @"UPDATE Patient 
                               SET FirstName = ?, LastName = ?, BirthDate = ?, Age = ?, 
                                   Gender = ?, CivilStatus = ?, Address = ?, Phone = ?, RegisteredDateTime = ? 
                               WHERE PatientID = ?";

                OleDbCommand cmd = new OleDbCommand(updateQuery, conn);

                try
                {
                    // Validate and set parameters
                    cmd.Parameters.AddWithValue("?", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("?", txtLastName.Text);
                    cmd.Parameters.AddWithValue("?", dtpBirthDate.Value.ToString("MM/dd/yyyy"));

                    if (!int.TryParse(txtAge.Text, out int age))
                    {
                        MessageBox.Show("Please enter a valid Age.");
                        return;
                    }
                    cmd.Parameters.AddWithValue("?", age);

                    cmd.Parameters.AddWithValue("?", cmbGender.Text);
                    cmd.Parameters.AddWithValue("?", cmbCivilStatus.Text);
                    cmd.Parameters.AddWithValue("?", cmbAddress.Text);
                    cmd.Parameters.AddWithValue("?", txtPhone.Text);
                    cmd.Parameters.AddWithValue("?", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                    cmd.Parameters.AddWithValue("?", currentID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            LoadPatients();
            ClearInputFields();
            ToggleAddSaveCancelButtons(false);
            DisableInputFields();
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgwPatients.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                int idToDelete = Convert.ToInt32(dgwPatients.SelectedRows[0].Cells["PatientID"].Value);

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string deleteQuery = "DELETE FROM Patient WHERE PatientID = ?";
                    OleDbCommand cmd = new OleDbCommand(deleteQuery, conn);
                    cmd.Parameters.AddWithValue("?", idToDelete);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                LoadPatients();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Login login = new Login(Username);
            login.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UserChangePassword change = new UserChangePassword(Username);
            change.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdminProfile profile = new AdminProfile(Username);
            profile.Show();
            this.Hide();
        }

        private void LoadPatients()
        {

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM Patient";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dgwPatients.DataSource = dataTable;
            }

            dgwPatients.ReadOnly = false;  

        }

        private void dgwPatients_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
          /*  txtID.Text = dgwPatients.CurrentRow.Cells[0].Value.ToString();
            txtFirstName.Text = dgwPatients.CurrentRow.Cells[1].Value.ToString();
            txtLastName.Text = dgwPatients.CurrentRow.Cells[2].Value.ToString();
            txtAge.Text = dgwPatients.CurrentRow.Cells[3].Value.ToString();
            cmbGender.Text = dgwPatients.CurrentRow.Cells[4].Value.ToString();
            cmbCivilStatus.Text = dgwPatients.CurrentRow.Cells[5].Value.ToString();
            txtAddress.Text = dgwPatients.CurrentRow.Cells[6].Value.ToString();
            cmbTypeOfID.Text = dgwPatients.CurrentRow.Cells[7].Value.ToString();
            txtIDNumber.Text = dgwPatients.CurrentRow.Cells[8].Value.ToString();
            dtp.Text = dgwPatients.CurrentRow.Cells[9].Value.ToString();*/
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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
            adminMEDICINE admin = new adminMEDICINE  (Username);
            admin.Show();
            this.Hide();
        }

        private void btnViewHistory_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected from the DataGridView
            if (dgwPatients.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dgwPatients.SelectedRows[0];

                // Retrieve the necessary values from the selected row
                int patientID = Convert.ToInt32(selectedRow.Cells["PatientID"].Value);
                string firstName = selectedRow.Cells["FirstName"].Value.ToString();
                string lastName = selectedRow.Cells["LastName"].Value.ToString();
                int age = CalculateAge(Convert.ToDateTime(selectedRow.Cells["BirthDate"].Value));

                // Open the ConsultationHistory form and pass all required data
                ConsultationHistory consultationHistory = new ConsultationHistory(patientID, Username, firstName, lastName, age);
                consultationHistory.Show();
            }
            else
            {
                // Display a warning if no row is selected
                MessageBox.Show("Please select a patient from the list.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private int CalculateAge(DateTime? birthDate)
        {
            if (!birthDate.HasValue)
            {
                return 0;  // O pwede mong ibalik ang default na edad kung wala ang birthdate
            }

            DateTime validBirthDate = birthDate.Value;
            int age = DateTime.Now.Year - validBirthDate.Year;
            if (DateTime.Now < validBirthDate.AddYears(age)) age--;
            return age;
        }
    }
}

