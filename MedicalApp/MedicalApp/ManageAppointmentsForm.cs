using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MedicalApp
{
    public partial class ManageAppointmentsForm : Form
    {
        private string connectionString;
        private DataSet appointmentsDataSet;
        private SqlDataAdapter dataAdapter;

        public ManageAppointmentsForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalDB"].ConnectionString;
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
                                        a.AppointmentID,
                                        a.AppointmentDate,
                                        d.FullName AS DoctorName,
                                        d.Specialty,
                                        p.FullName AS PatientName,
                                        p.Email AS PatientEmail,
                                        a.Notes,
                                        a.DoctorID,
                                        a.PatientID
                                    FROM Appointments a
                                    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
                                    INNER JOIN Patients p ON a.PatientID = p.PatientID
                                    ORDER BY a.AppointmentDate DESC";

                    dataAdapter = new SqlDataAdapter(query, connection);
                    appointmentsDataSet = new DataSet();
                    dataAdapter.Fill(appointmentsDataSet, "Appointments");

                    dgvAppointments.DataSource = appointmentsDataSet.Tables["Appointments"];
                    
                    // Format the DataGridView
                    FormatDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            if (dgvAppointments.Columns.Count > 0)
            {
                dgvAppointments.Columns["AppointmentID"].HeaderText = "ID";
                dgvAppointments.Columns["AppointmentID"].Width = 50;
                dgvAppointments.Columns["AppointmentDate"].HeaderText = "Date & Time";
                dgvAppointments.Columns["AppointmentDate"].Width = 130;
                dgvAppointments.Columns["DoctorName"].HeaderText = "Doctor";
                dgvAppointments.Columns["DoctorName"].Width = 150;
                dgvAppointments.Columns["Specialty"].HeaderText = "Specialty";
                dgvAppointments.Columns["Specialty"].Width = 100;
                dgvAppointments.Columns["PatientName"].HeaderText = "Patient";
                dgvAppointments.Columns["PatientName"].Width = 150;
                dgvAppointments.Columns["PatientEmail"].HeaderText = "Email";
                dgvAppointments.Columns["PatientEmail"].Width = 200;
                dgvAppointments.Columns["Notes"].HeaderText = "Notes";
                dgvAppointments.Columns["Notes"].Width = 200;
                
                // Hide ID columns
                dgvAppointments.Columns["DoctorID"].Visible = false;
                dgvAppointments.Columns["PatientID"].Visible = false;
                
                dgvAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvAppointments.ReadOnly = false;
                dgvAppointments.AllowUserToAddRows = false;
                dgvAppointments.AllowUserToDeleteRows = false;
                
                // Make only specific columns editable
                foreach (DataGridViewColumn column in dgvAppointments.Columns)
                {
                    column.ReadOnly = true;
                }
                dgvAppointments.Columns["AppointmentDate"].ReadOnly = false;
                dgvAppointments.Columns["Notes"].ReadOnly = false;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void btnUpdateAppointment_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to update.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    foreach (DataGridViewRow row in dgvAppointments.SelectedRows)
                    {
                        int appointmentId = Convert.ToInt32(row.Cells["AppointmentID"].Value);
                        DateTime appointmentDate = Convert.ToDateTime(row.Cells["AppointmentDate"].Value);
                        string notes = row.Cells["Notes"].Value?.ToString() ?? "";

                        string updateQuery = @"UPDATE Appointments 
                                             SET AppointmentDate = @AppointmentDate, Notes = @Notes 
                                             WHERE AppointmentID = @AppointmentID";

                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        {
                            command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentId;
                            command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = appointmentDate;
                            command.Parameters.Add("@Notes", SqlDbType.VarChar, 500).Value = 
                                string.IsNullOrEmpty(notes) ? DBNull.Value : (object)notes;

                            command.ExecuteNonQuery();
                        }
                    }
                    
                    MessageBox.Show("Appointment(s) updated successfully!", "Update Successful", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAppointments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating appointment: {ex.Message}", "Update Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteAppointment_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to delete.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete the selected appointment(s)?", 
                "Confirm Delete", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        
                        foreach (DataGridViewRow row in dgvAppointments.SelectedRows)
                        {
                            int appointmentId = Convert.ToInt32(row.Cells["AppointmentID"].Value);

                            string deleteQuery = "DELETE FROM Appointments WHERE AppointmentID = @AppointmentID";

                            using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                            {
                                command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentId;
                                command.ExecuteNonQuery();
                            }
                        }
                        
                        MessageBox.Show("Appointment(s) deleted successfully!", "Delete Successful", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAppointments();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting appointment: {ex.Message}", "Delete Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterAppointments();
        }

        private void FilterAppointments()
        {
            try
            {
                string searchText = txtSearch.Text.Trim();
                
                if (appointmentsDataSet != null && appointmentsDataSet.Tables["Appointments"] != null)
                {
                    DataView dataView = appointmentsDataSet.Tables["Appointments"].DefaultView;
                    
                    if (string.IsNullOrEmpty(searchText))
                    {
                        dataView.RowFilter = "";
                    }
                    else
                    {
                        dataView.RowFilter = $"DoctorName LIKE '%{searchText}%' OR PatientName LIKE '%{searchText}%' OR Specialty LIKE '%{searchText}%'";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering appointments: {ex.Message}", "Filter Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvAppointments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAppointments.Rows[e.RowIndex];
                string info = $"Appointment Details:\n\n" +
                             $"ID: {row.Cells["AppointmentID"].Value}\n" +
                             $"Date & Time: {row.Cells["AppointmentDate"].Value}\n" +
                             $"Doctor: {row.Cells["DoctorName"].Value}\n" +
                             $"Specialty: {row.Cells["Specialty"].Value}\n" +
                             $"Patient: {row.Cells["PatientName"].Value}\n" +
                             $"Email: {row.Cells["PatientEmail"].Value}\n" +
                             $"Notes: {row.Cells["Notes"].Value}";

                MessageBox.Show(info, "Appointment Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
