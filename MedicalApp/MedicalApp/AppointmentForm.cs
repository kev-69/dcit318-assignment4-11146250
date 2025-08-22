using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MedicalApp
{
    public partial class AppointmentForm : Form
    {
        private string connectionString;

        public AppointmentForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalDB"].ConnectionString;
            LoadDoctors();
            LoadPatients();
            dtpAppointmentDate.MinDate = DateTime.Now;
        }

        private void LoadDoctors()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DoctorID, FullName, Specialty FROM Doctors WHERE Availability = 1";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable doctorTable = new DataTable();
                            doctorTable.Load(reader);
                            
                            cmbDoctor.DataSource = doctorTable;
                            cmbDoctor.DisplayMember = "FullName";
                            cmbDoctor.ValueMember = "DoctorID";
                            cmbDoctor.SelectedIndex = -1;
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

        private void LoadPatients()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT PatientID, FullName, Email FROM Patients";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable patientTable = new DataTable();
                            patientTable.Load(reader);
                            
                            cmbPatient.DataSource = patientTable;
                            cmbPatient.DisplayMember = "FullName";
                            cmbPatient.ValueMember = "PatientID";
                            cmbPatient.SelectedIndex = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading patients: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBookAppointment_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        
                        // Check if doctor is available at the selected time
                        string checkQuery = @"SELECT COUNT(*) FROM Appointments 
                                            WHERE DoctorID = @DoctorID 
                                            AND CAST(AppointmentDate AS DATE) = CAST(@AppointmentDate AS DATE)
                                            AND DATEPART(HOUR, AppointmentDate) = DATEPART(HOUR, @AppointmentDate)";
                        
                        using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                        {
                            checkCommand.Parameters.Add("@DoctorID", SqlDbType.Int).Value = cmbDoctor.SelectedValue;
                            checkCommand.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = dtpAppointmentDate.Value;
                            
                            int existingAppointments = (int)checkCommand.ExecuteScalar();
                            
                            if (existingAppointments > 0)
                            {
                                MessageBox.Show("Doctor is not available at the selected date and time. Please choose a different time.", 
                                    "Time Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        
                        // Book the appointment
                        string insertQuery = @"INSERT INTO Appointments (DoctorID, PatientID, AppointmentDate, Notes) 
                                             VALUES (@DoctorID, @PatientID, @AppointmentDate, @Notes)";
                        
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.Add("@DoctorID", SqlDbType.Int).Value = cmbDoctor.SelectedValue;
                            insertCommand.Parameters.Add("@PatientID", SqlDbType.Int).Value = cmbPatient.SelectedValue;
                            insertCommand.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = dtpAppointmentDate.Value;
                            insertCommand.Parameters.Add("@Notes", SqlDbType.VarChar, 500).Value = 
                                string.IsNullOrEmpty(txtNotes.Text) ? DBNull.Value : (object)txtNotes.Text;
                            
                            int rowsAffected = insertCommand.ExecuteNonQuery();
                            
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Appointment booked successfully!", "Success", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearForm();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error booking appointment: {ex.Message}", "Booking Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInput()
        {
            if (cmbDoctor.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a doctor.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDoctor.Focus();
                return false;
            }

            if (cmbPatient.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a patient.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPatient.Focus();
                return false;
            }

            if (dtpAppointmentDate.Value <= DateTime.Now)
            {
                MessageBox.Show("Please select a future date and time for the appointment.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpAppointmentDate.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            cmbDoctor.SelectedIndex = -1;
            cmbPatient.SelectedIndex = -1;
            dtpAppointmentDate.Value = DateTime.Now.AddHours(1);
            txtNotes.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDoctor.SelectedIndex != -1)
            {
                DataRowView selectedRow = (DataRowView)cmbDoctor.SelectedItem;
                lblDoctorSpecialty.Text = $"Specialty: {selectedRow["Specialty"]}";
            }
            else
            {
                lblDoctorSpecialty.Text = "Specialty: ";
            }
        }
    }
}
