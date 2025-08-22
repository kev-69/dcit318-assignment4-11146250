using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PharmacyApp
{
    public partial class PharmacyMainForm : Form
    {
        private string connectionString;

        public PharmacyMainForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["PharmacyDB"].ConnectionString;
            LoadAllMedicines();
        }

        private void LoadAllMedicines()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("GetAllMedicines", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable medicineTable = new DataTable();
                            medicineTable.Load(reader);
                            dgvMedicines.DataSource = medicineTable;
                            
                            // Format the DataGridView
                            FormatDataGridView();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading medicines: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            if (dgvMedicines.Columns.Count > 0)
            {
                dgvMedicines.Columns["MedicineID"].HeaderText = "ID";
                dgvMedicines.Columns["MedicineID"].Width = 50;
                dgvMedicines.Columns["Name"].HeaderText = "Medicine Name";
                dgvMedicines.Columns["Name"].Width = 200;
                dgvMedicines.Columns["Category"].HeaderText = "Category";
                dgvMedicines.Columns["Category"].Width = 120;
                dgvMedicines.Columns["Price"].HeaderText = "Price ($)";
                dgvMedicines.Columns["Price"].Width = 80;
                dgvMedicines.Columns["Price"].DefaultCellStyle.Format = "C2";
                dgvMedicines.Columns["Quantity"].HeaderText = "Stock";
                dgvMedicines.Columns["Quantity"].Width = 80;
                
                dgvMedicines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvMedicines.ReadOnly = true;
                dgvMedicines.AllowUserToAddRows = false;
            }
        }

        private void btnAddMedicine_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("AddMedicine", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            
                            command.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = txtMedicineName.Text.Trim();
                            command.Parameters.Add("@Category", SqlDbType.VarChar, 50).Value = txtCategory.Text.Trim();
                            command.Parameters.Add("@Price", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPrice.Text);
                            command.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(txtQuantity.Text);
                            
                            object result = command.ExecuteScalar();
                            
                            if (result != null)
                            {
                                MessageBox.Show($"Medicine added successfully! ID: {result}", "Success", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearInputFields();
                                LoadAllMedicines();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding medicine: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearchTerm.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadAllMedicines();
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SearchMedicine", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@SearchTerm", SqlDbType.VarChar, 100).Value = searchTerm;
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable medicineTable = new DataTable();
                            medicineTable.Load(reader);
                            dgvMedicines.DataSource = medicineTable;
                            FormatDataGridView();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching medicines: {ex.Message}", "Search Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            if (dgvMedicines.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a medicine to update stock.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int medicineId = Convert.ToInt32(dgvMedicines.SelectedRows[0].Cells["MedicineID"].Value);
            string medicineName = dgvMedicines.SelectedRows[0].Cells["Name"].Value.ToString();
            int currentStock = Convert.ToInt32(dgvMedicines.SelectedRows[0].Cells["Quantity"].Value);

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Current stock for {medicineName}: {currentStock}\nEnter new stock quantity:",
                "Update Stock",
                currentStock.ToString());

            if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int newQuantity) && newQuantity >= 0)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("UpdateStock", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add("@MedicineID", SqlDbType.Int).Value = medicineId;
                            command.Parameters.Add("@Quantity", SqlDbType.Int).Value = newQuantity;
                            
                            object result = command.ExecuteScalar();
                            
                            if (Convert.ToInt32(result) > 0)
                            {
                                MessageBox.Show("Stock updated successfully!", "Success", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadAllMedicines();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating stock: {ex.Message}", "Update Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (!string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Please enter a valid quantity (0 or greater).", "Invalid Input", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRecordSale_Click(object sender, EventArgs e)
        {
            if (dgvMedicines.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a medicine to record sale.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int medicineId = Convert.ToInt32(dgvMedicines.SelectedRows[0].Cells["MedicineID"].Value);
            string medicineName = dgvMedicines.SelectedRows[0].Cells["Name"].Value.ToString();
            int currentStock = Convert.ToInt32(dgvMedicines.SelectedRows[0].Cells["Quantity"].Value);

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Recording sale for: {medicineName}\nCurrent stock: {currentStock}\nEnter quantity sold:",
                "Record Sale",
                "1");

            if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int quantitySold) && quantitySold > 0)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("RecordSale", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add("@MedicineID", SqlDbType.Int).Value = medicineId;
                            command.Parameters.Add("@QuantitySold", SqlDbType.Int).Value = quantitySold;
                            
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    int success = Convert.ToInt32(reader["Success"]);
                                    string message = reader["Message"].ToString();
                                    
                                    if (success == 1)
                                    {
                                        MessageBox.Show(message, "Success", 
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        LoadAllMedicines();
                                    }
                                    else
                                    {
                                        MessageBox.Show(message, "Sale Error", 
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error recording sale: {ex.Message}", "Sale Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (!string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Please enter a valid quantity (greater than 0).", "Invalid Input", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            txtSearchTerm.Clear();
            LoadAllMedicines();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMedicineName.Text))
            {
                MessageBox.Show("Please enter medicine name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMedicineName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCategory.Text))
            {
                MessageBox.Show("Please enter category.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategory.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }

            return true;
        }

        private void ClearInputFields()
        {
            txtMedicineName.Clear();
            txtCategory.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            txtMedicineName.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputFields();
        }

        private void dgvMedicines_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dgvMedicines.SelectedRows.Count > 0;
            btnUpdateStock.Enabled = hasSelection;
            btnRecordSale.Enabled = hasSelection;
        }
    }
}
