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
    public partial class LoginPage : KryptonForm
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            university university;
            string email = emailTextBox.Text;
            string password = passwordTextBox.Text;

            if (email == "" && password == "")
            {
                MessageBox.Show("Please fill in your username and password.");
            }
            else
            {
                university = new university();
                university.email = email;
                university.password = password;

                using (db_scholarsEntities contex = new db_scholarsEntities())
                {
                    university = contex.universities.Where(x => x.email == university.email && x.password == university.password).FirstOrDefault();
                }

                if (university != null)
                {
                    university = new university();
                    university.email = email;

                    using (db_scholarsEntities contex = new db_scholarsEntities())
                    {
                        university = contex.universities.Where(x => x.email == university.email).FirstOrDefault();
                    }

                    DashboardPage dashboardPage = new DashboardPage(this, university);
                    dashboardPage.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Your username or password is incorrect.");
                }
            }
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            RegisterPage registerPage = new RegisterPage(this);
            registerPage.Show();
            this.Hide();
        }
    }
}
