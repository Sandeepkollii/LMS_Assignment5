
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LMS_Assignment5
{
    public class EmployeeForm : Form
    {
        private TextBox txtEmployeeId;
        private TextBox txtName;
        private TextBox txtLocation;
        private TextBox txtMobile;
        private Button btnCreate;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnReset;
        private DataGridView dgvEmployees;

        public EmployeeForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Employee Management";
            this.Size = new System.Drawing.Size(800, 600);

            // Employee ID
            Label lblEmployeeId = new Label();
            lblEmployeeId.Text = "Employee ID:";
            lblEmployeeId.Location = new System.Drawing.Point(20, 20);

            txtEmployeeId = new TextBox();
            txtEmployeeId.Location = new System.Drawing.Point(100, 20);
            txtEmployeeId.Size = new System.Drawing.Size(200, 20);

            // Name
            Label lblName = new Label();
            lblName.Text = "Name:";
            lblName.Location = new System.Drawing.Point(20, 50);

            txtName = new TextBox();
            txtName.Location = new System.Drawing.Point(100, 50);
            txtName.Size = new System.Drawing.Size(200, 20);

            // Location
            Label lblLocation = new Label();
            lblLocation.Text = "Location:";
            lblLocation.Location = new System.Drawing.Point(20, 80);

            txtLocation = new TextBox();
            txtLocation.Location = new System.Drawing.Point(100, 80);
            txtLocation.Size = new System.Drawing.Size(200, 20);

            // Mobile
            Label lblMobile = new Label();
            lblMobile.Text = "Mobile:";
            lblMobile.Location = new System.Drawing.Point(20, 110);

            txtMobile = new TextBox();
            txtMobile.Location = new System.Drawing.Point(100, 110);
            txtMobile.Size = new System.Drawing.Size(200, 20);

            // Buttons
            btnCreate = new Button();
            btnCreate.Text = "Create";
            btnCreate.Location = new System.Drawing.Point(20, 150);
            btnCreate.Click += new EventHandler(btnCreate_Click);

            btnUpdate = new Button();
            btnUpdate.Text = "Update";
            btnUpdate.Location = new System.Drawing.Point(100, 150);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);

            btnDelete = new Button();
            btnDelete.Text = "Delete";
            btnDelete.Location = new System.Drawing.Point(180, 150);
            btnDelete.Click += new EventHandler(btnDelete_Click);

            btnReset = new Button();
            btnReset.Text = "Reset";
            btnReset.Location = new System.Drawing.Point(260, 150);
            btnReset.Click += new EventHandler(btnReset_Click);

            // DataGridView
            dgvEmployees = new DataGridView();
            dgvEmployees.Location = new System.Drawing.Point(20, 190);
            dgvEmployees.Size = new System.Drawing.Size(740, 350);
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.CellClick += new DataGridViewCellEventHandler(dgvEmployees_CellClick);

            // Add controls
            this.Controls.AddRange(new Control[] {
                lblEmployeeId, txtEmployeeId,
                lblName, txtName,
                lblLocation, txtLocation,
                lblMobile, txtMobile,
                btnCreate, btnUpdate, btnDelete, btnReset,
                dgvEmployees
            });

            // Load data
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Employee", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvEmployees.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Employee (Name, Location, Mobile_no) VALUES (@name, @location, @mobile)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@location", txtLocation.Text);
                        cmd.Parameters.AddWithValue("@mobile", txtMobile.Text);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Employee created successfully!");
                    LoadData();
                    ResetFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating employee: " + ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmployeeId.Text))
            {
                MessageBox.Show("Please select an employee to update!");
                return;
            }

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Employee SET Name=@name, Location=@location, Mobile_no=@mobile WHERE Employee_Id=@id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", txtEmployeeId.Text);
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@location", txtLocation.Text);
                        cmd.Parameters.AddWithValue("@mobile", txtMobile.Text);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Employee updated successfully!");
                    LoadData();
                    ResetFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating employee: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmployeeId.Text))
            {
                MessageBox.Show("Please select an employee to delete!");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this employee?", "Confirm Delete",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Employee WHERE Employee_Id=@id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", txtEmployeeId.Text);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Employee deleted successfully!");
                        LoadData();
                        ResetFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting employee: " + ex.Message);
                    }
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetFields();
        }

        private void ResetFields()
        {
            txtEmployeeId.Clear();
            txtName.Clear();
            txtLocation.Clear();
            txtMobile.Clear();
        }

        private void dgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvEmployees.Rows[e.RowIndex];
                txtEmployeeId.Text = row.Cells["Employee_Id"].Value.ToString();
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtLocation.Text = row.Cells["Location"].Value.ToString();
                txtMobile.Text = row.Cells["Mobile_no"].Value.ToString();
            }
        }
    }
}