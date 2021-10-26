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
    public partial class LecturerPage : KryptonForm
    {
        DashboardPage dashboardPage;
        university university;
        int lecturer_id;
        string status;

        public LecturerPage()
        {
            InitializeComponent();
        }

        public LecturerPage(DashboardPage dashboardPage, university university, string status)
        {
            InitializeComponent();
            this.dashboardPage = dashboardPage;
            this.university = university;
            this.status = status;
            setComboBox();
        }

        public LecturerPage(DashboardPage dashboardPage, university university, int lecturer_id, string status)
        {
            InitializeComponent();
            this.dashboardPage = dashboardPage;
            this.university = university;
            this.lecturer_id = lecturer_id;
            this.status = status;
            setComboBox();
            setHistory();
        }

        private void setComboBox()
        {
            var contex = new db_scholarsEntities();
            var query_faculties = (from faculty in contex.faculties
                                   select faculty.faculty_name).ToList();

            foreach (string faculty in query_faculties)
            {
                facultyComboBox.Items.Add(faculty);
            }

            degreeComboBox.Items.Add("S1");
            degreeComboBox.Items.Add("S2");
            degreeComboBox.Items.Add("S3");
            genderComboBox.Items.Add("Male");
            genderComboBox.Items.Add("Female");
        }

        private void setHistory()
        {
            var contex = new db_scholarsEntities();
            var query_lecturer = (from lecturer in contex.lecturers
                                  join faculty in contex.faculties on lecturer.faculty_id equals faculty.faculty_id
                                  where lecturer.lecturer_id == lecturer_id
                                  select new
                                  {
                                      faculty_name = faculty.faculty_name,
                                      lecturer_name = lecturer.lecturer_name,
                                      degree = lecturer.degree,
                                      gender = lecturer.gender
                                  }).Single();

            if (query_lecturer != null)
            {
                facultyComboBox.SelectedItem = query_lecturer.faculty_name;
                lecturerNameTextBox.Text = query_lecturer.lecturer_name;
                degreeComboBox.SelectedItem = query_lecturer.degree;
                genderComboBox.SelectedItem = query_lecturer.gender;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var contex = new db_scholarsEntities();
            var query_faculty = (from faculty in contex.faculties
                                 where faculty.faculty_name == facultyComboBox.SelectedItem.ToString()
                                 select faculty.faculty_id).Single();

            int faculty_id = query_faculty;

            if (status == "add")
            {
                lecturer lecturer = new lecturer();
                lecturer.university_id = university.university_id;
                lecturer.faculty_id = faculty_id;
                lecturer.lecturer_name = lecturerNameTextBox.Text;
                lecturer.degree = degreeComboBox.SelectedItem.ToString();
                lecturer.gender = genderComboBox.SelectedItem.ToString();

                contex = new db_scholarsEntities();
                contex.lecturers.Add(lecturer);
                contex.SaveChanges();
                MessageBox.Show("Lecturer added successfully.");
            }
            else if (status == "edit")
            {
                contex = new db_scholarsEntities();
                lecturer query_lecturer = (from lecturer in contex.lecturers
                                           where lecturer.lecturer_id == lecturer_id && lecturer.university_id == university.university_id
                                           select lecturer).Single();

                query_lecturer.faculty_id = faculty_id;
                query_lecturer.lecturer_name = lecturerNameTextBox.Text;
                query_lecturer.degree = degreeComboBox.SelectedItem.ToString();
                query_lecturer.gender = genderComboBox.SelectedItem.ToString();
                contex.SaveChanges();
                MessageBox.Show("Lecturer edited successfully.");
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
