using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PRE_DEFENSE
{
    public partial class Login : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;

        private int loginattempts = 0;
        public string Username { get; set; }

        public Login(string loggedInUsername)
        {
            InitializeComponent();

            Username = loggedInUsername;


            txtPassword.PasswordChar = '*';
           
        }

        public Login()
        {

        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void tbUN_TextChanged(object sender, EventArgs e)
        {
            txtUsername.ForeColor = string.IsNullOrWhiteSpace(txtUsername.Text) ? Color.Gray : Color.Black;
        }

        private void PWtb_TextChanged(object sender, EventArgs e)
        {
            txtPassword.ForeColor = string.IsNullOrWhiteSpace(txtPassword.Text) ? Color.Gray : Color.Black;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Initialize connection and declare the login attempts variable at the class level
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\ASUS\Desktop\c#\PRE_DEFENSE\PRE_DEFENSE\bin\Debug\Login.accdb;Persist Security Info=False;";

            string query = "SELECT [Type] FROM Account WHERE Username = @username AND Password = @password";
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.Add("@username", OleDbType.VarWChar).Value = txtUsername.Text;
            cmd.Parameters.Add("@password", OleDbType.VarWChar).Value = txtPassword.Text;

            try
            {
                // Open the connection
                conn.Open();

                // Execute the query and get the result
                object result = cmd.ExecuteScalar();

                // Debugging: Output the result of the query for inspection
                Console.WriteLine("Query Result: " + result);

                if (result != null)
                {
                    string userType = result.ToString().Trim();  // Trim any extra spaces
                    this.Hide();  // Hide the current form

                    // Log the userType to verify it
                    Console.WriteLine("User Type: " + userType);

                    // Handle different user types
                    if (userType.Equals("User", StringComparison.OrdinalIgnoreCase))  // Case insensitive comparison
                    {
                        User user = new User(txtUsername.Text);  // Show FormDashboard for USER
                        user.Show();
                    }
                    else if (userType.Equals("Admin", StringComparison.OrdinalIgnoreCase))  // Case insensitive comparison
                    {
                        Admin admin = new Admin(txtUsername.Text);  // Show Admin form for ADMIN user
                        admin.Show();
                    }
                    else
                    {
                        // If the user type is unrecognized, show a message and show the form again
                        MessageBox.Show("Unrecognized User Type.");
                        this.Show(); // Show the form again
                    }

                    // Reset the login attempts on successful login
                    loginattempts = 0;
                }
                else
                {
                    // If no result is returned, it means the username/password are incorrect
                    loginattempts++;
                    MessageBox.Show("Invalid username or password.");
                    txtUsername.Text = string.Empty;
                    txtPassword.Text = string.Empty;

                    // If login attempts exceed the limit, exit the application
                    if (loginattempts >= 3)
                    {
                        MessageBox.Show("Too many failed login attempts. The application will now close.");
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions such as connection errors
                MessageBox.Show("Error: " + ex.Message);

                // Optionally log the exception message for debugging purposes
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                // Ensure the connection is always closed, even if an error occurs
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            /*try
            {
                // Open the connection
                conn.Open();

                // Define the query with parameters to avoid SQL injection
                string query = "SELECT ID FROM Account WHERE Username = @username AND Password = @password";
                OleDbCommand cmd = new OleDbCommand(query, conn);

                // Add parameters securely to avoid SQL injection
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                // Execute the query using ExecuteScalar to retrieve the first column of the first row
                object result = cmd.ExecuteScalar();

                // Check if the user exists (result is not null)
                if (result != null && result is int userId)
                {
                    // Format the current time without milliseconds
                    string formattedTimeIn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    // Insert the TimeIn value into the LogTable
                    string insertLogQuery = "INSERT INTO LogTable (ID, TimeIn) VALUES (@userId, @timeIn)";
                    OleDbCommand insertCmd = new OleDbCommand(insertLogQuery, conn);
                    insertCmd.Parameters.AddWithValue("@userId", userId);
                    insertCmd.Parameters.AddWithValue("@timeIn", formattedTimeIn);

                    // Execute the insert query to record the login time
                    insertCmd.ExecuteNonQuery();

                    // Show success message
                    MessageBox.Show("Login Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open the FormDashboard and pass the userId to it
                    FormDashboard da = new FormDashboard(userId);  // Pass userId to FormDashboard constructor
                    da.Show();
                    this.Hide();
                }
                else
                {
                    // If the query returned no result (invalid login), show an error
                    MessageBox.Show("Incorrect Username or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here (e.g., database connection issues)
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the connection is always closed, even if an error occurs
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }



            /* OleDbCommand cmd = new OleDbCommand();
             cmd.Connection = conn;
             cmd.CommandText = "Select * from Account where Username ='" + txtUsername.Text + "' and Password ='" + txtPassword.Text + "'";

             OleDbDataReader or = cmd.ExecuteReader();

             int count = 0;
             while (or.Read())
             {
                 count = count + 1;
             }
             if (count == 1)
             {
                 MessageBox.Show("Login Successfully", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

                 FormDashboard da = new FormDashboard();
                 da.Show();
                 this.Hide();

             }
             else
             {
                 MessageBox.Show("Incorrect Username and Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
             conn.Close();*/





            /*string username = txtUsername.Text;
            string password = txtPassword.Text;

           
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (username == "user" && password == "password")
            {
                FormDashboard dashboard = new FormDashboard();
                dashboard.Show();
                this.Hide(); 
            }
            else if (username == "admin" && password == "password")
            {
              
                FormAdmin admin = new FormAdmin();
                admin.Show();
                this.Hide(); 
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtUsername.Text = string.Empty;
                txtPassword.Text = string.Empty;
            }*/
        }

        private void SPcb_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked)
            {
                txtPassword.PasswordChar = '\0'; 
            }
            else
            {
                txtPassword.PasswordChar = '*'; 
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
