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
    public partial class adminAddConsultation : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\ASUS\Desktop\c#\PRE_DEFENSE\PRE_DEFENSE\bin\Debug\Patient.accdb";

        private DataTable diagnosisTable;

        public string Username { get; set; }

        public adminAddConsultation( string loggedInUsername)
        {
            InitializeComponent();
            diagnosisTable = new DataTable();
            Username = loggedInUsername;
        }

        private void PopulatePatientComboBox()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT PatientID, FirstName, LastName FROM Patient";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dt.Columns.Add("FullName", typeof(string), "FirstName + ' ' + LastName"); // Combine names

                    cboPatientID.DisplayMember = "FullName";
                    cboPatientID.ValueMember = "PatientID";
                    cboPatientID.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Patient data: " + ex.Message);
            }
        }

        private void adminAddConsultation_Load(object sender, EventArgs e)
        {
            PopulatePatientComboBox(); // Populating the ComboBox with patient data
            LoadCheckupData();
            PopulateMedicineComboBox(); // Populate Medicine ComboBox
            dtpConsultationDate.Value = DateTime.Now;
            dtpConsultationDate.Enabled = false; // Make the DateTimePicker non-editable
        }

        private void PopulateMedicineComboBox()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MedicineID, MedicineName FROM Medicine";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cbMedicine.DisplayMember = "MedicineName"; // Display MedicineName in ComboBox
                    cbMedicine.ValueMember = "MedicineID";     // Use MedicineID as the value
                    cbMedicine.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Medicine data: " + ex.Message);
            }
        }
        private void LoadCheckupData()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "SELECT * FROM Checkup";  // Query to get data from Checkup table
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Bind Checkup data to DataGridView
                    dgvDiagnosis.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Checkup data: " + ex.Message);
            }
        }

        private void LoadConsultationData(int patientID)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "SELECT * FROM Checkup WHERE PatientID = ?";  // Fetch data based on selected PatientID
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("?", patientID);  // Add parameter for PatientID

                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Bind the filtered data to the DataGridView
                    dgvDiagnosis.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading consultation data: " + ex.Message);
            }
        }

        private void cboPatientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Clear all the inputs when the selected patient changes
                ClearInputs();

                // Get the selected PatientID from the ComboBox
                if (cboPatientID.SelectedValue != null)
                {
                    int selectedPatientID = Convert.ToInt32(cboPatientID.SelectedValue);

                    // Load the consultation data for the selected PatientID
                    LoadConsultationData(selectedPatientID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageBox.Show("Please fill out all required fields correctly.");
                return;
            }

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Checkup ([PatientID], [MidWife], [Diagnosis], [Complaint], [Treatments], [Remarks], [TypeOfCheckUp], [MedicineName], [Quantity]) " +
                                   "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", Convert.ToInt32(cboPatientID.SelectedValue));
                        cmd.Parameters.AddWithValue("?", cbMidWife.SelectedItem?.ToString() ?? string.Empty);
                        cmd.Parameters.AddWithValue("?", txtDiagnosis.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtComplaint.Text.Trim());
                        cmd.Parameters.AddWithValue("?", txtTreatments.Text.Trim());
                        cmd.Parameters.AddWithValue("?", string.IsNullOrWhiteSpace(txtRemarks.Text) ? (object)DBNull.Value : txtRemarks.Text.Trim());
                        cmd.Parameters.AddWithValue("?", cbTypeOfCheckUp.SelectedItem?.ToString() ?? string.Empty);
                        cmd.Parameters.AddWithValue("?", cbMedicine.SelectedValue != null ? Convert.ToInt32(cbMedicine.SelectedValue) : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("?", Convert.ToInt32(txtQuantity.Text));
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Consultation record added successfully.");
                            ClearInputs();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add consultation record.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding consultation record: " + ex.Message);
            }
        }

        private bool ValidateInputs()
        {
            if (cboPatientID.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtDiagnosis.Text) ||
                string.IsNullOrWhiteSpace(txtComplaint.Text) ||
                string.IsNullOrWhiteSpace(txtTreatments.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                !int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                return false;
            }
            return true;
        }

        private void ClearInputs()
        {
            cboPatientID.SelectedIndex = -1; // Reset the Patient ComboBox
            dtpConsultationDate.Value = DateTime.Now; // Reset the Consultation Date to the current date
            cbMidWife.SelectedIndex = -1; // Reset the MidWife ComboBox
            txtDiagnosis.Clear(); // Clear the Diagnosis TextBox
            txtComplaint.Clear(); // Clear the Complaint TextBox
            txtTreatments.Clear(); // Clear the Treatments TextBox
            txtRemarks.Clear(); // Clear the Remarks TextBox
            cbTypeOfCheckUp.SelectedIndex = -1; // Reset the Type of CheckUp ComboBox
            cbMedicine.SelectedIndex = -1;
            txtQuantity.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Admin admin = new Admin(Username);
            admin.Show();
            this.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminPatientRecord adminpatientrecord = new AdminPatientRecord(Username);
            adminpatientrecord.Show();
            this.Hide();
        }

        private void btnManageUser_Click(object sender, EventArgs e)
        {
            UserManagement userManagement = new UserManagement(Username);
            userManagement.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdminProfile adminProfile = new AdminProfile(Username);
            adminProfile.Show();
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

        private void button6_Click(object sender, EventArgs e)
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
