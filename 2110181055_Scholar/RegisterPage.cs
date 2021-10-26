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
    public partial class RegisterPage : KryptonForm
    {
        LoginPage loginPage;

        public RegisterPage()
        {
            InitializeComponent();
        }

        public RegisterPage(LoginPage loginPage)
        {
            InitializeComponent();
            this.loginPage = loginPage;
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            university university = new university();
            university.university_name = universityNameTextBox.Text;
            university.email = emailTextBox.Text;
            university.password = passwordTextBox.Text;
            university.location = locationTextBox.Text;

            var contex = new db_scholarsEntities();
            university university_check = contex.universities.Where(x => x.email == university.email).FirstOrDefault();

            if (university_check == null)
            {
                contex.universities.Add(university);
                contex.SaveChanges();
                MessageBox.Show("University added successfully.");
            }
            else
            {
                MessageBox.Show("University has been registered previously.");
            }

            loginPage.Show();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            loginPage.Show();
            this.Close();
        }
    }
}
