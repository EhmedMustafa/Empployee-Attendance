using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Empployee_Attendance.Repository;

namespace Empployee_Attendance
{
    public partial class LoginForm : Form
    {
        private readonly EmployeeRepository _repo = new EmployeeRepository();
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void txtUsername_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtUsername.ForeColor = Color.White;
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {
            txtPassword.Clear();
            txtPassword.ForeColor = Color.White;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else txtPassword.UseSystemPasswordChar = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Text;



            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("İstifadeçi adı və ya Şifrə boş ola bilməz!", "Xəbardarlıq", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var user = _repo.GetUserByname(username, password);
            if (user == null)
            {
                MessageBox.Show("Yanlış İstifadəçi adı/Parol", "Xəbardarlıq", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int empid = user.EmployeeId;

            if (user.IsAdmin)
            {
                MessageBox.Show("Uğurla daxil olun!", "Məlumat Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AdminDashboardForm admin = new AdminDashboardForm();
                this.Hide();
                admin.ShowDialog();
            }
            else
            {
                MessageBox.Show("Uğurla daxil olun!", "Məlumat Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MainForm mainForm = new MainForm(username, empid);
                this.Hide();
                mainForm.ShowDialog();
            }
        }
    }
}
