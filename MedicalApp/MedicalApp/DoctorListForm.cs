using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MedicalApp
{
    public partial class DoctorListForm : Form
    {
        private string connectionString;

        public DoctorListForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalDB"].ConnectionString;
            LoadDoctors();
        }

        private void LoadDoctors()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DoctorID, FullName, Specialty, Availability FROM Doctors";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable doctorTable = new DataTable();
                            doctorTable.Load(reader);
                            
                            // Add a computed column for availability display
                            doctorTable.Columns.Add("AvailabilityStatus", typeof(string));
                            foreach (DataRow row in doctorTable.Rows)
                            {
                                bool isAvailable = Convert.ToBoolean(row["Availability"]);
                                row["AvailabilityStatus"] = isAvailable ? "Available" : "Not Available";
                            }
                            
                            dgvDoctors.DataSource = doctorTable;
                            
                            // Format the DataGridView
                            dgvDoctors.Columns["DoctorID"].HeaderText = "ID";
                            dgvDoctors.Columns["FullName"].HeaderText = "Doctor Name";
                            dgvDoctors.Columns["Specialty"].HeaderText = "Specialty";
                            dgvDoctors.Columns["Availability"].Visible = false; // Hide boolean column
                            dgvDoctors.Columns["AvailabilityStatus"].HeaderText = "Status";
                            
                            dgvDoctors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                            dgvDoctors.ReadOnly = true;
                            dgvDoctors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading doctors: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDoctors();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterDoctors();
        }

        private void FilterDoctors()
        {
            try
            {
                string searchText = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchText))
                {
                    LoadDoctors();
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT DoctorID, FullName, Specialty, Availability 
                                   FROM Doctors 
                                   WHERE FullName LIKE @SearchTerm OR Specialty LIKE @SearchTerm";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@SearchTerm", SqlDbType.VarChar).Value = $"%{searchText}%";
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable doctorTable = new DataTable();
                            doctorTable.Load(reader);
                            
                            // Add availability status column
                            doctorTable.Columns.Add("AvailabilityStatus", typeof(string));
                            foreach (DataRow row in doctorTable.Rows)
                            {
                                bool isAvailable = Convert.ToBoolean(row["Availability"]);
                                row["AvailabilityStatus"] = isAvailable ? "Available" : "Not Available";
                            }
                            
                            dgvDoctors.DataSource = doctorTable;
                            
                            // Hide the boolean Availability column
                            if (dgvDoctors.Columns["Availability"] != null)
                                dgvDoctors.Columns["Availability"].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching doctors: {ex.Message}", "Search Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
