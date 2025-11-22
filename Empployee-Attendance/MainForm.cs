using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using Empployee_Attendance.Repository;
using Microsoft.Data.SqlClient;
using Timer = System.Windows.Forms.Timer;

namespace Empployee_Attendance
{
    public partial class MainForm : Form
    {
        private readonly string connectionString = @"Data Source=DESKTOP-K64PT03\SQLEXPRESS;Initial Catalog=EmployeeTable;Integrated Security=True";
        private readonly EmployeeRepository _repo = new EmployeeRepository();
        private readonly int _EmplloyeeId;
        private readonly string _employeeName;
        private readonly string _storeName;
        private readonly int _StoreId;
        private System.Windows.Forms.Timer _Timer;

        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:3000/api/";
        public MainForm(string EmployeeName, int EmployeeId)
        {
            InitializeComponent();
            _EmplloyeeId = EmployeeId;
            _employeeName = EmployeeName;
            lblEmployeeName.Text = $"Xoş gəldin,{_employeeName}";
            lblStore.Text = $"Mağaza: {_storeName}";
            lblDate.Text = DateTime.Now.ToString("dd.MM.yyyy");

            LoadTodayShifts(EmployeeId);
            StartClock();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }


        public void StartClock()
        {
            _Timer = new Timer();
            _Timer.Interval = 1000; // 1 second
            _Timer.Tick += (s, e) =>
            {
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss");

            };
            _Timer.Start();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async Task LoadTodayShifts(int employeeId)
        {
            var todayshifts = await _repo.GetTodayShift(employeeId);

            lblStore.Text = $"Mağaza: {todayshifts.StoreName}";

            var load = await _repo.Loadstatus(employeeId);

            if (load == null)
            {
                lblStatus.Text = "Bugün: Hələ giriş edilməyib.";
                btnCheckIn.Enabled = true;
                btnCheckOut.Enabled = false;
            }

            else if (load.CheckIn != null && load.CheckOut == null)
            {
                lblStatus.Text = $"Bugün: Giriş edilib {((DateTime)load.CheckIn).ToString("HH:mm")}, çıxış gözlənilir.";
                btnCheckIn.Enabled = false;
                btnCheckOut.Enabled = true;
            }

            else
            {
                lblStatus.Text = $"Bugün: Giriş : {((DateTime)load.CheckIn).ToString("HH:mm")} - Bugün Çıxış: {((DateTime)load.CheckOut).ToString("HH:mm")}";
                btnCheckIn.Enabled = false;
                btnCheckOut.Enabled = false;
            }

            if (todayshifts != null)
            {
                lblShiftInfo.Text = $"Növbə: {todayshifts.ShiftDisplay.ToString()}";
            }


            else
            {
                lblShiftInfo.Text = "Növbə təyin edilməyib";
            }
        }







        private async void btnCheckIn_Click(object sender, EventArgs e)
        {
            // MessageBox.Show($"Göndərilən URL: {_httpClient.BaseAddress}/attendance/checkin/{_EmplloyeeId}");
            var response = await _httpClient.PostAsync($"attendance/checkin/{_EmplloyeeId}", null);

            if (response.IsSuccessStatusCode)
            {
                var resultMessage = await response.Content.ReadAsStringAsync();
                MessageBox.Show(resultMessage, "Məlumat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadTodayShifts(_EmplloyeeId);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Xəta baş verdi: {errorMessage}", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            await LoadTodayShifts(_EmplloyeeId);

        }

        private async void btnCheckOut_Click(object sender, EventArgs e)
        {
            var response = await _httpClient.PutAsync($"attendance/checkout/{_EmplloyeeId}", null);
            if (response.IsSuccessStatusCode)
            {
                var resultMessage = await response.Content.ReadAsStringAsync();
                MessageBox.Show(resultMessage, "Məlumat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadTodayShifts(_EmplloyeeId);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Xəta baş verdi: {errorMessage}", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            await LoadTodayShifts(_EmplloyeeId);
        }   
    
    }
}
