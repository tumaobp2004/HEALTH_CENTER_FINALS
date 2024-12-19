using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using WordprocessingParagraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using WordprocessingRun = DocumentFormat.OpenXml.Wordprocessing.Run;
using WordprocessingText = DocumentFormat.OpenXml.Wordprocessing.Text;
using System.IO;


namespace PRE_DEFENSE
{
    public partial class ConsultationHistory : Form
    {

        private int patientID;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Username { get; set; }

        private string selectedConsultationDate;
        private string selectedDoctorName;
        private string selectedDiagnosis;

        public ConsultationHistory(int patientID, string loggedInUsername, string firstName, string lastName, int age)
        {
            InitializeComponent();
            this.patientID = patientID;
            this.Username = loggedInUsername;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;

            txtFirstName.Text = FirstName;
            txtLastName.Text = LastName;
            txtAge.Text = Age.ToString();

            txtFirstName.ReadOnly = true;
            txtLastName.ReadOnly = true;
            txtAge.ReadOnly = true;

            LoadConsultationHistory(patientID);

        }

      
    
        public ConsultationHistory( string loggedInUsername, int patientID)
        {
            this.patientID = patientID;
            Username = loggedInUsername;
        }

        public ConsultationHistory(int patientID, string username)
        {
            this.patientID = patientID;
            Username = username;
        }

        public ConsultationHistory(int patientID)
        {
            this.patientID = patientID;
        }

        private void LoadConsultationHistory(int patientID)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Patient.accdb";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM Checkup WHERE PatientID = @PatientID ORDER BY ConsultationDate DESC";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@PatientID", patientID);

                try
                {
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dgwHistory.DataSource = dataTable;
                }
                catch (OleDbException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void ConsultationHistory_Load(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\ASUS\Desktop\c#\PRE_DEFENSE\PRE_DEFENSE\bin\Debug\Patient.accdb";

            // Open connection to the database
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                // Ensure the column names match exactly with your database schema
                string query = "SELECT * FROM Checkup WHERE PatientID = @PatientID ORDER BY ConsultationDate DESC";  // Fixed column name

                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@PatientID", patientID);  // Bind PatientID

                // Debugging step: output the query to ensure the parameter is correctly handled
                Console.WriteLine("SQL Query: " + query);  // This can help verify that the query is formed correctly

                try
                {
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);  // Fill the data table with the result from the database

                    // Bind the result to the DataGridView for display
                    dgwHistory.DataSource = dataTable;
                }
                catch (OleDbException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);  // Show error if the query fails
                }
            }
        }

        private void GenerateMedicalCertificate(WordprocessingDocument wordprocessingDocument)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create("MedicalCertificate.docx", WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(); // Specify the correct namespace

                var body = new DocumentFormat.OpenXml.Wordprocessing.Body();

                var title = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
                    new DocumentFormat.OpenXml.Wordprocessing.Run(
                        new DocumentFormat.OpenXml.Wordprocessing.Text("Medical Certificate")));

                // Set alignment
                title.ParagraphProperties = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties
                {
                    Justification = new DocumentFormat.OpenXml.Wordprocessing.Justification { Val = DocumentFormat.OpenXml.Wordprocessing.JustificationValues.Center }
                };

                body.AppendChild(title);

                // Add other content as paragraphs
                body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
                    new DocumentFormat.OpenXml.Wordprocessing.Run(
                        new DocumentFormat.OpenXml.Wordprocessing.Text("\nThis is to certify that:"))));
                body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
                    new DocumentFormat.OpenXml.Wordprocessing.Run(
                        new DocumentFormat.OpenXml.Wordprocessing.Text($"Patient Name: {FirstName} {LastName}"))));
                body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
                    new DocumentFormat.OpenXml.Wordprocessing.Run(
                        new DocumentFormat.OpenXml.Wordprocessing.Text($"Age: {Age}"))));
                body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
                    new DocumentFormat.OpenXml.Wordprocessing.Run(
                        new DocumentFormat.OpenXml.Wordprocessing.Text($"Consultation Date: {selectedConsultationDate}"))));
                body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
                    new DocumentFormat.OpenXml.Wordprocessing.Run(
                        new DocumentFormat.OpenXml.Wordprocessing.Text($"Doctor: Dr. {selectedDoctorName}"))));
                body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
                    new DocumentFormat.OpenXml.Wordprocessing.Run(
                        new DocumentFormat.OpenXml.Wordprocessing.Text($"Diagnosis: {selectedDiagnosis}"))));

                mainPart.Document.AppendChild(body);
                mainPart.Document.Save();
            
                MessageBox.Show("Medical certificate has been generated successfully!");
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            User user = new User(Username);
            user.Show();
            this.Hide();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            PatientRecord patientRecord = new PatientRecord(Username);
            patientRecord.Show();
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

        private void dgwHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgwHistory.Rows[e.RowIndex];

                selectedConsultationDate = row.Cells["ConsultationDate"].Value?.ToString();
                selectedDoctorName = row.Cells["MidWife"].Value?.ToString();
                selectedDiagnosis = row.Cells["Diagnosis"].Value?.ToString();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (dgwHistory.Rows.Count == 0)
            {
                MessageBox.Show("No consultation history available.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Generate the PDF file in memory
                using (MemoryStream ms = new MemoryStream())
                {
                    using (PdfWriter writer = new PdfWriter(ms))
                    using (PdfDocument pdfDoc = new PdfDocument(writer))
                    using (Document document = new Document(pdfDoc))
                    {
                        document.Add(new Paragraph("Medical Certificate")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontSize(20)
                            .SetBold());
                        document.Add(new Paragraph("\n"));
                        document.Add(new Paragraph("This is to certify that:").SetFontSize(12));
                        document.Add(new Paragraph($"Patient Name: {FirstName} {LastName}").SetFontSize(12));
                        document.Add(new Paragraph($"Age: {Age}").SetFontSize(12));
                        document.Add(new Paragraph($"Consultation Date: {selectedConsultationDate}").SetFontSize(12));
                        document.Add(new Paragraph($"Doctor: Dr. {selectedDoctorName}").SetFontSize(12));
                        document.Add(new Paragraph($"Diagnosis: {selectedDiagnosis}").SetFontSize(12));
                        document.Add(new Paragraph("\nRemarks:").SetFontSize(12));
                        document.Add(new Paragraph("This certificate is issued for medical purposes. The patient is fit for work.").SetFontSize(12));
                        document.Add(new Paragraph("\nCertified by:").SetFontSize(12));
                        document.Add(new Paragraph("_________________________").SetFontSize(12));
                        document.Add(new Paragraph("Doctor's Name").SetFontSize(12));
                        document.Add(new Paragraph($"Date: {DateTime.Now:MMMM dd, yyyy}").SetFontSize(12));
                    }

                    // Print the PDF directly
                    PrintPDF(ms.ToArray());
                }

                MessageBox.Show("Medical certificate has been printed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while printing the medical certificate: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintPDF(byte[] pdfBytes)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), "MedicalCertificate.pdf");

            // Save PDF to a temporary file
            File.WriteAllBytes(tempFilePath, pdfBytes);

            // Print the file using the default printer
            using (System.Diagnostics.Process printProcess = new System.Diagnostics.Process())
            {
                printProcess.StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = tempFilePath,
                    Verb = "print",
                    CreateNoWindow = true,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                };
                printProcess.Start();
                printProcess.WaitForExit(); // Wait for the print job to complete
            }

            // Optionally delete the temp file after printing
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }


        private void ConsultationHistory_Load_1(object sender, EventArgs e)
        {
            LoadConsultationHistory(patientID);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddConsultation addConsultation = new AddConsultation(Username);
            addConsultation.Show();
            this.Hide();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            User user = new User(Username);
            user.Show();
            this.Hide();
        }

        private void btnRecord_Click_1(object sender, EventArgs e)
        {
            PatientRecord patientRecord = new PatientRecord(Username);
            patientRecord.Show();
            this.Hide();
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            UserProfile user = new UserProfile(Username);
            user.Show();
            this.Hide();
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            UserAboutUs user = new UserAboutUs(Username);
            user.Show();
            this.Hide();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            Login login = new Login (Username);
            login.Show();
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
