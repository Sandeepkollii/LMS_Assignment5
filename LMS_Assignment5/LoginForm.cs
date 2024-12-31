// LoginForm.cs
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LMS_Assignment5
{
    public partial class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private ComboBox cmbUserType;
        private Button btnLogin;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Login";
            this.Size = new System.Drawing.Size(300, 250);

          
            Label lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new System.Drawing.Point(20, 20);

            txtUsername = new TextBox();
            txtUsername.Location = new System.Drawing.Point(20, 40);
            txtUsername.Size = new System.Drawing.Size(200, 20);

           
            Label lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Location = new System.Drawing.Point(20, 70);

            txtPassword = new TextBox();
            txtPassword.Location = new System.Drawing.Point(20, 90);
            txtPassword.Size = new System.Drawing.Size(200, 20);
            txtPassword.PasswordChar = '*';

            Label lblUserType = new Label();
            lblUserType.Text = "User Type:";
            lblUserType.Location = new System.Drawing.Point(20, 120);

            cmbUserType = new ComboBox();
            cmbUserType.Location = new System.Drawing.Point(20, 140);
            cmbUserType.Size = new System.Drawing.Size(200, 20);
            cmbUserType.Items.AddRange(new string[] { "admin", "non-admin" });

          
            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new System.Drawing.Point(20, 170);
            btnLogin.Click += new EventHandler(btnLogin_Click);

          
            this.Controls.AddRange(new Control[] {
                lblUsername, txtUsername,
                lblPassword, txtPassword,
                lblUserType, cmbUserType,
                btnLogin
            });
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT UserType FROM Users WHERE Username=@username AND Password=@password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                        string userType = cmd.ExecuteScalar()?.ToString();

                        if (userType != null)
                        {
                            if (userType == "admin")
                                new AdminForm().Show();
                            else
                                new UserForm().Show();

                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid credentials!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }

    public class AdminForm : Form
    {
        public AdminForm()
        {
            this.Text = "Admin Dashboard";
            this.Size = new System.Drawing.Size(300, 200);

            Label lblWelcome = new Label();
            lblWelcome.Text = "Hello Admin!";
            lblWelcome.Location = new System.Drawing.Point(20, 20);
            lblWelcome.AutoSize = true;

            this.Controls.Add(lblWelcome);
        }
    }

    public class UserForm : Form
    {
        public UserForm()
        {
            this.Text = "User Dashboard";
            this.Size = new System.Drawing.Size(300, 200);

            Label lblWelcome = new Label();
            lblWelcome.Text = "Hello User!";
            lblWelcome.Location = new System.Drawing.Point(20, 20);
            lblWelcome.AutoSize = true;

            this.Controls.Add(lblWelcome);
        }
    }
}