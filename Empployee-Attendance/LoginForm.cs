using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Empployee_Attendance.Models;
using Empployee_Attendance.Repository;
using Newtonsoft.Json;

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

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Text;



            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("İstifadeçi adı və ya Şifrə boş ola bilməz!", "Xəbardarlıq", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            string apiUrl = "http://localhost:3000/api/login";

            var loginData = new { username = username, password = password };

            var json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(apiUrl, content);
                var responseText = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var loginResult = JsonConvert.DeserializeObject<LoginResponse>(responseText);
                    var user = loginResult.Employee;
                    MessageBox.Show("Uğurla daxil oldunuz!", "Məlumat", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (user.IsAdmin)
                    {
                        AdminDashboardForm admin = new AdminDashboardForm();
                        this.Hide();
                        admin.ShowDialog();
                    }
                    else
                    {
                        MainForm mainForm = new MainForm(username, user.EmployeeId);
                        this.Hide();
                        mainForm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Yanlış istifadəçi adı və ya şifrə!", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //var user = _repo.GetUserByname(username, password);
            //if (user == null)
            //{
            //    MessageBox.Show("Yanlış İstifadəçi adı/Parol", "Xəbardarlıq", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //int empid = user.EmployeeId;

            //if (user.IsAdmin)
            //{
            //    MessageBox.Show("Uğurla daxil olun!", "Məlumat Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    AdminDashboardForm admin = new AdminDashboardForm();
            //    this.Hide();
            //    admin.ShowDialog();
            //}
            //else
            //{
            //    MessageBox.Show("Uğurla daxil olun!", "Məlumat Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    MainForm mainForm = new MainForm(username, empid);
            //    this.Hide();
            //    mainForm.ShowDialog();
            //}
        }
    }
}
