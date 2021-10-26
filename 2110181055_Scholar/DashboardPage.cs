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
    public partial class DashboardPage : KryptonForm
    {
        LoginPage loginPage;
        university university;

        public DashboardPage()
        {
            InitializeComponent();
        }

        public DashboardPage(LoginPage loginPage, university university)
        {
            InitializeComponent();
            this.loginPage = loginPage;
            this.university = university;
            universityNameLabel.Text = university.university_name;
            showLecturer();
        }

        public void showLecturer()
        {
            var contex = new db_scholarsEntities();
            var query_lecturers = (from lecturer in contex.lecturers
                                   join faculty in contex.faculties on lecturer.faculty_id equals faculty.faculty_id
                                   where lecturer.university_id == university.university_id
                                   select new
                                   {
                                       ID = lecturer.lecturer_id,
                                       Faculty = faculty.faculty_name,
                                       Name = lecturer.lecturer_name,
                                       Degree = lecturer.degree,
                                       Gender = lecturer.gender
                                   }).ToList();

            lecturerDataGridView.DataSource = query_lecturers;
            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            editButtonColumn.Name = "Edit";
            editButtonColumn.Text = "Edit";
            editButtonColumn.UseColumnTextForButtonValue = true;
            DataGridViewButtonColumn finishButtonColumn = new DataGridViewButtonColumn();
            finishButtonColumn.Name = "Delete";
            finishButtonColumn.Text = "Delete";
            finishButtonColumn.UseColumnTextForButtonValue = true;

            if (lecturerDataGridView.Columns["Edit"] == null)
            {
                lecturerDataGridView.Columns.Insert(5, editButtonColumn);
            }

            if (lecturerDataGridView.Columns["Delete"] == null)
            {
                lecturerDataGridView.Columns.Insert(6, finishButtonColumn);
            }

            for (int i = 0; i < lecturerDataGridView.ColumnCount; i++)
            {
                if (i != 5 && i != 6)
                {
                    lecturerDataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }

                lecturerDataGridView.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            lecturerDataGridView.Columns[5].Width = 100;
            lecturerDataGridView.Columns[6].Width = 100;

            lecturerDataGridView.CellClick += lecturerDataGridView_EditCellClick;
            lecturerDataGridView.CellClick += lecturerDataGridView_DeleteCellClick;
        }

        private void lecturerDataGridView_EditCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == lecturerDataGridView.Columns["Edit"].Index)
            {
                int lecturer_id = Convert.ToInt32(lecturerDataGridView.Rows[e.RowIndex].Cells[2].Value);
                LecturerPage lecturerPage = new LecturerPage(this, university, lecturer_id, "edit");
                lecturerPage.Show();
                this.Hide();
            }
        }

        private void lecturerDataGridView_DeleteCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == lecturerDataGridView.Columns["Delete"].Index)
            {
                int lecturer_id = Convert.ToInt32(lecturerDataGridView.Rows[e.RowIndex].Cells[2].Value);
                var contex = new db_scholarsEntities();
                var query_lecturer = (from lecturer in contex.lecturers
                                      where lecturer.lecturer_id == lecturer_id && lecturer.university_id == university.university_id
                                      select lecturer).Single();

                contex.lecturers.Remove(query_lecturer);
                contex.SaveChanges();
                showLecturer();
                MessageBox.Show("Lecturer successfully deleted.");
            }
        }

        private void addLecturerButton_Click_1(object sender, EventArgs e)
        {
            LecturerPage lecturerPage = new LecturerPage(this, university, "add");
            lecturerPage.Show();
            this.Hide();
        }

        private void addFacultyButton_Click_1(object sender, EventArgs e)
        {
            FacultyPage facultyPage = new FacultyPage(this);
            facultyPage.Show();
            this.Hide();
        }

        private void signOutButton_Click_1(object sender, EventArgs e)
        {
            loginPage.Show();
            this.Close();
        }
    }
}
