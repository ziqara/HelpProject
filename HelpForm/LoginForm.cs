using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HelpProject.Models;
using HelpProject.Repository;
using HelpProject.Services;

namespace HelpForm
{
    public partial class LoginForm : Form
    {
        private ServiceUser serviceUser_;
        private User user_;

        public LoginForm()
        {
            InitializeComponent();
            NpgRepositoryUser userRepository_ = new NpgRepositoryUser();
            serviceUser_ = new ServiceUser(userRepository_);
        }
        public User GetUser()
        {
            return user_;
        }
        private void btnGuest_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbLogin.Text))
            {
                MessageBox.Show("введите логин");
                return;
            }
            user_ = serviceUser_.GetUser(txbLogin.Text);
            if (user_ == null)
            {
                MessageBox.Show("такого нет");
                return;
            }

            if (string.IsNullOrWhiteSpace(txbPassword.Text))
            {
                MessageBox.Show("введите пароль");
                return;
            }

            Auth(txbLogin.Text, txbPassword.Text);
        }

        private void Auth(string login, string password)
        {
            if (serviceUser_.CheckUser(login, password))
            {
                user_ = serviceUser_.GetUser(login);
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("не тот пароль");
                return;
            }
        }
    }
}
