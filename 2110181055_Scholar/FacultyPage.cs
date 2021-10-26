using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace _2110181055_Scholar
{
    public partial class FacultyPage : KryptonForm
    {
        DashboardPage dashboardPage;

        public FacultyPage()
        {
            InitializeComponent();
        }

        public FacultyPage(DashboardPage dashboardPage)
        {
            InitializeComponent();
            this.dashboardPage = dashboardPage;
            setComboBox();
        }

        public void setComboBox()
        {
            accreditationComboBox.Items.Add("A");
            accreditationComboBox.Items.Add("B");
            accreditationComboBox.Items.Add("C");
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            faculty faculty = new faculty();
            faculty.faculty_name = facultyNameTextBox.Text;
            faculty.accreditation = accreditationComboBox.SelectedItem.ToString();

            var contex = new db_scholarsEntities();
            faculty faculty_check = contex.faculties.Where(x => x.faculty_name == faculty.faculty_name).FirstOrDefault();

            if (faculty_check == null)
            {
                contex.faculties.Add(faculty);
                contex.SaveChanges();
                MessageBox.Show("Faculty added successfully.");
            }
            else
            {
                MessageBox.Show("Faculty already exists.");
            }

            dashboardPage.showLecturer();
            dashboardPage.Show();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            dashboardPage.Show();
            this.Close();
        }
    }
}
