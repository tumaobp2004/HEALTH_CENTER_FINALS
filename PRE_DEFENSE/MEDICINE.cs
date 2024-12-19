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
    public partial class MEDICINE : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\ASUS\Desktop\c#\PRE_DEFENSE\PRE_DEFENSE\bin\Debug\Patient.accdb";
        public string Username { get; set; }
        public MEDICINE(string loggedInUsername)
        {
            InitializeComponent();
            LoadMedicineData();
            Username = loggedInUsername;
        }

        private void ClearInputs()
        {
            txtMedicine.Clear(); // Clear the Diagnosis TextBox
            txtQuantity.Clear(); // Clear the Diagnosis TextBox
            dtpDate.Value = DateTime.Now;
           
        }
        private void LoadMedicineData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Medicine ORDER BY DateAdded DESC";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgwMedicine.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Medicine (MedicineName, Quantity, DateAdded) VALUES (@Name, @Quantity, @DateAdded)";
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", txtMedicine.Text);
                    cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtQuantity.Text));
                    cmd.Parameters.AddWithValue("@DateAdded", dtpDate.Value);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Medicine added successfully!");
                    LoadMedicineData();
                    ClearInputs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dgwMedicine.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a medicine record to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int medicineID = Convert.ToInt32(dgwMedicine.SelectedRows[0].Cells["MedicineID"].Value);

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Medicine SET MedicineName = @Name, Quantity = @Quantity, DateAdded = @DateAdded WHERE MedicineID = @ID";
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", txtMedicine.Text);
                    cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtQuantity.Text));
                    cmd.Parameters.AddWithValue("@DateAdded", dtpDate.Value);
                    cmd.Parameters.AddWithValue("@ID", medicineID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Medicine updated successfully!");
                    LoadMedicineData();
                    ClearInputs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dgwMedicine.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a medicine record to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int medicineID = Convert.ToInt32(dgwMedicine.SelectedRows[0].Cells["MedicineID"].Value);

            var confirmResult = MessageBox.Show("Are you sure you want to delete this medicine record?",
                                                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Medicine WHERE MedicineID = @ID";
                        OleDbCommand cmd = new OleDbCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ID", medicineID);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Medicine deleted successfully!");
                        LoadMedicineData();
                        ClearInputs();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void dgwMedicine_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgwMedicine.Rows[e.RowIndex];
                txtMedicine.Text = row.Cells["MedicineName"].Value.ToString();
                txtQuantity.Text = row.Cells["Quantity"].Value.ToString();
                dtpDate.Value = Convert.ToDateTime(row.Cells["DateAdded"].Value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User user = new User(Username);
            user.Show();
            this.Hide();
        }

        private void btnMedicine_Click(object sender, EventArgs e)
        {
            MEDICINE mEDICINE = new MEDICINE(Username);
            mEDICINE.Show();
            this.Hide();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            PatientRecord patientRecord = new PatientRecord(Username);
            patientRecord.Show();
            this.Hide();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            AddConsultation addConsultation = new AddConsultation(Username);
            addConsultation.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            UserProfile userProfile = new UserProfile(Username);
            userProfile.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            UserAboutUs userAboutUs = new UserAboutUs(Username);
            userAboutUs.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Login login = new Login(Username);
            login.Show();
            this.Hide();
        }
    }
}
