using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedicalApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnDoctorList_Click(object sender, EventArgs e)
        {
            DoctorListForm doctorForm = new DoctorListForm();
            doctorForm.Show();
        }

        private void btnBookAppointment_Click(object sender, EventArgs e)
        {
            AppointmentForm appointmentForm = new AppointmentForm();
            appointmentForm.Show();
        }

        private void btnManageAppointments_Click(object sender, EventArgs e)
        {
            ManageAppointmentsForm manageForm = new ManageAppointmentsForm();
            manageForm.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
