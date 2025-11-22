using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Empployee_Attendance.Dto;
using Empployee_Attendance.Models;
using Empployee_Attendance.Repository;
using Timer = System.Windows.Forms.Timer;

namespace Empployee_Attendance
{
    public partial class AdminDashboardForm : Form
    {
        private Timer _timer;
        private readonly EmployeeRepository _repo = new EmployeeRepository();
        public AdminDashboardForm()
        {
            InitializeComponent();
            startclock();
            dgvAttendance.AutoGenerateColumns = true;
        }


        public void startclock()
        {
            _timer = new Timer();
            _timer.Interval = 1000; // 1 second
            _timer.Tick += (s, e) =>
            {
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            };
            _timer.Start();
        }

        private void AdminDashboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void txtaxtar_Click(object sender, EventArgs e)
        {
            txtaxtar.Clear();
            txtaxtar.ForeColor = Color.Black;
        }

        //private async Task AdminDashboardForm_Load(object sender, EventArgs e)
        //{
        //    var employees = await _repo.GetAll();
        //    dgvEmployees.DataSource = employees.Select(a => new
        //    {
        //        a.Ad_Familiya,
        //        a.Istifadəçi_Adı,
        //        a.Şifrə,
        //        a.Admin
        //    }).ToList();
        //}

        private void dgvAttendance_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvAttendance.Columns[e.ColumnIndex].Name == "Status")
            {
                var value = e.Value?.ToString();

                if (value == "İşə gəlməyib")
                {
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightGray;
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
                else if (value == "Gec gəlib")
                {
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Khaki;
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
                else if (value == "Tez çıxıb")
                {
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightGray;
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
                else if (value == "Gec gəlib və tez çıxıb")
                {
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightSeaGreen;
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
                else if (value == "Tam")
                {
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightGreen;
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
                else if (value == "Istirahət")
                {
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightSkyBlue;
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
                else
                {
                    // Default (əgər heç biri uyğun deyilsə)
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                    dgvAttendance.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                }
            }
        }


        private async Task LoadComboshop()
        {
            var shop = await _repo.GetAllStores();

            shop.Insert(0, new Store
            {
                StoreId = 0,
                StoreName = "---Hamsı---"
            });
            cmbStore.DisplayMember = "StoreName";
            cmbStore.ValueMember = "StoreId";
            cmbStore.DataSource = shop;
        }

        private void AdminDashboardForm_Enter(object sender, EventArgs e)
        {
            LoadComboshop();
        }



        private void tabControl_Enter(object sender, EventArgs e)
        {
            LoadComboshop();
        }



        private void cmbStore_Click(object sender, EventArgs e)
        {
            LoadComboshop();
        }



        private async Task FilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (FilterStatus.SelectedItem == null)
            {
                return;
            }

            int storeId = Convert.ToInt32(cmbStore.SelectedValue);
            DateTime selectedDate = dtpDate.Value.Date;

            var alldata = await _repo.GetAttendanceByStoreAndDate(storeId, selectedDate);
            string selectedStatus = FilterStatus.SelectedItem.ToString();
            var analysisList = new List<AttendanceAnalysis>();




            foreach (var item in alldata)
            {
                if (item.ShiftStart.HasValue && item.ShiftEnd.HasValue)
                {
                    var shift = new ShiftsDtos
                    {
                        StartTime = item.ShiftStart.Value,
                        EndTime = item.ShiftEnd.Value,
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,

                    };

                    var attendance = new Attendance

                    {
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,
                        Date = item.Date,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut
                    };

                    var analitics = _repo.AnalyzeAttendance(shift, attendance);
                    analitics.EmployeeId = item.EmployeeId;
                    analitics.StoreId = item.StoreId;
                    analitics.EmployeeName = item.EmployeeName;
                    analitics.StoreName = item.StoreName;
                    analitics.ShiftDisplay = item.ShiftDisplay;
                    //analitics.Date = item.Date;



                    analysisList.Add(analitics);

                }

                if (item.DayOff == true)
                {

                    var shift = new ShiftsDtos
                    {
                        StartTime = item.ShiftStart,
                        EndTime = item.ShiftEnd,
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,

                    };

                    var attendance = new Attendance

                    {
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,
                        Date = item.Date,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut
                    };

                    var analitics = _repo.AnalyzeAttendance(shift, attendance);
                    analitics.EmployeeId = item.EmployeeId;
                    analitics.StoreId = item.StoreId;
                    analitics.EmployeeName = item.EmployeeName;
                    analitics.StoreName = item.StoreName;
                    analitics.ShiftDisplay = item.ShiftDisplay;
                    analitics.Status = "Istirahət";

                    analysisList.Add(analitics);


                }
            }
            List<AttendanceAnalysis> filteredList;
            if (selectedStatus == "Bütün")
                filteredList = analysisList;
            else
                filteredList = analysisList
                    .Where(x => string.Equals(x.Status?.Trim(), selectedStatus, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            dgvAttendance.DataSource = filteredList
                     .Select(a => new
                     {
                         a.Ad_Familiya,
                         a.Mağaza_Adı,
                         a.Tarix,
                         a.Növbə,
                         a.Giriş_vaxtı,
                         a.Çıxış_Vaxtı,
                         a.Gecikmə,
                         a.Icazə,
                         a.Status
                     }).ToList();
        }



        private async void AdminDashboardForm_Load(object sender, EventArgs e)
        {
            await loadEmployee();
        }


        private async Task loadEmployee()
        {
            var employees = await _repo.GetAll();
            dgvEmployees.DataSource = employees.Select(a => new
            {
                a.Ad_Familiya,
                a.Istifadəçi_Adı,
                a.Şifrə,
                Admin = a.Admin ? "Admin" : "------------"
            }).ToList();

        }

        private async void tabAttendance_Enter(object sender, EventArgs e)
        {
            DateTime selectedDate = DateTime.Now;


            int storeId = Convert.ToInt32(cmbStore.SelectedValue);


            var alldata = await _repo.GetAttendanceByStoreAndDate(storeId, selectedDate);


            var analysisList = new List<AttendanceAnalysis>();





            foreach (var item in alldata)
            {
                if (item.ShiftStart.HasValue && item.ShiftEnd.HasValue)
                {
                    var shift = new ShiftsDtos
                    {
                        StartTime = item.ShiftStart.Value,
                        EndTime = item.ShiftEnd.Value,
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,

                    };

                    var attendance = new Attendance

                    {
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,
                        Date = item.Date,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut
                    };

                    var analitics = _repo.AnalyzeAttendance(shift, attendance);
                    analitics.EmployeeId = item.EmployeeId;
                    analitics.StoreId = item.StoreId;
                    analitics.EmployeeName = item.EmployeeName;
                    analitics.StoreName = item.StoreName;
                    analitics.ShiftDisplay = item.ShiftDisplay;
                    //analitics.Date = item.Date;



                    analysisList.Add(analitics);

                }

                if (item.DayOff == true)
                {

                    var shift = new ShiftsDtos
                    {
                        StartTime = item.ShiftStart,
                        EndTime = item.ShiftEnd,
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,

                    };

                    var attendance = new Attendance

                    {
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,
                        Date = item.Date,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut,
                    };

                    var analitics = _repo.AnalyzeAttendance(shift, attendance);
                    analitics.EmployeeId = item.EmployeeId;
                    analitics.StoreId = item.StoreId;
                    analitics.EmployeeName = item.EmployeeName;
                    analitics.StoreName = item.StoreName;
                    analitics.ShiftDisplay = item.ShiftDisplay;
                    analitics.Status = "Istirahət";


                    //analitics.Date = item.Date;


                    analysisList.Add(analitics);


                }
            }

            dgvAttendance.DataSource = analysisList
                     .Select(a => new
                     {
                         a.Ad_Familiya,
                         a.Mağaza_Adı,
                         a.Tarix,
                         a.Növbə,
                         a.Giriş_vaxtı,
                         a.Çıxış_Vaxtı,
                         a.Gecikmə,
                         a.Icazə,
                         a.Status
                     }).ToList();


            //dgvAttendance.Columns[0].Visible = false;
            //dgvAttendance.Columns[1].Visible = false;

            var statusList = new List<string>
            {
                "Bütün",       // bütün statusları göstərmək üçün
                "Normal",      // vaxtında gəlib çıxanlar
                "Gec gəlib",   // gecikənlər
                "Tez çıxıb",   // tez çıxanlar
                "Gec gəlib və tez çıxıb",
                "Tam deyil",   // giriş və ya çıxış qeyd olunmayıb
                "Istirahət"    // DayOff olanlar
            };

            FilterStatus.DataSource = statusList;   // ComboBox-a əlavə et
            FilterStatus.SelectedIndex = 0;
            dtpDate.Value = DateTime.Today;
        }

        private void cmbStore_Enter(object sender, EventArgs e)
        {
            LoadComboshop();
        }

        private async void btnViewAttendance_Click(object sender, EventArgs e)
        {
            int storeId = Convert.ToInt32(cmbStore.SelectedValue);
            DateTime selectedDate = dtpDate.Value.Date;

            var data = await _repo.GetAttendanceByStoreAndDate(storeId, selectedDate);
            string selectedStatus = FilterStatus.SelectedItem.ToString();
            var analysisList = new List<AttendanceAnalysis>();

            foreach (var item in data)
            {
                if (item.ShiftStart.HasValue && item.ShiftEnd.HasValue)
                {
                    var shift = new ShiftsDtos
                    {
                        StartTime = item.ShiftStart.Value,
                        EndTime = item.ShiftEnd.Value,
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,
                    };

                    var attendance = new Attendance

                    {
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,
                        Date = item.Date,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut
                    };

                    var analitics = _repo.AnalyzeAttendance(shift, attendance);
                    analitics.EmployeeId = item.EmployeeId;
                    analitics.StoreId = item.StoreId;
                    //analitics.Date = item.Date;
                    analitics.EmployeeName = item.EmployeeName;
                    analitics.StoreName = item.StoreName;
                    analitics.ShiftDisplay = item.ShiftDisplay;



                    analysisList.Add(analitics);

                }
                else if (item.DayOff == true)
                {

                    var shift = new ShiftsDtos
                    {
                        StartTime = item.ShiftStart,
                        EndTime = item.ShiftEnd,
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,

                    };

                    var attendance = new Attendance

                    {
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,
                        Date = item.Date,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut
                    };

                    var analitics = _repo.AnalyzeAttendance(shift, attendance);
                    analitics.EmployeeId = item.EmployeeId;
                    analitics.StoreId = item.StoreId;
                    analitics.EmployeeName = item.EmployeeName;
                    analitics.StoreName = item.StoreName;
                    analitics.ShiftDisplay = item.ShiftDisplay;
                    analitics.Status = "Istirahət";

                    analysisList.Add(analitics);

                }
            }
            List<AttendanceAnalysis> filteredList;
            if (selectedStatus == "Bütün")
                filteredList = analysisList;
            else
                filteredList = analysisList
                    .Where(x => string.Equals(x.Status?.Trim(), selectedStatus, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            dgvAttendance.DataSource = filteredList
                     .Select(a => new
                     {
                         a.Ad_Familiya,
                         a.Mağaza_Adı,
                         a.Tarix,
                         a.Növbə,
                         a.Giriş_vaxtı,
                         a.Çıxış_Vaxtı,
                         a.Gecikmə,
                         a.Icazə,
                         a.Status
                     }).ToList();
        }

        //private async void cmbStore_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    await LoadAttendanceAsync();
        //}

        private async Task LoadAttendanceAsync()
        {
            if (cmbStore.SelectedValue == null || cmbStore.SelectedIndex == -1)
                return;

            if (cmbStore.SelectedValue is DataRowView)
                return;

            int storeId = Convert.ToInt32(cmbStore.SelectedValue);
            DateTime selectedDate = DateTime.Now; // Əgər gələcəkdə tarix seçmək istəsən, DateTimePicker ilə dəyişəcəksən

            var data = await _repo.GetAttendanceByStoreAndDate(storeId, selectedDate);




            var analysisList = new List<AttendanceAnalysis>();

            foreach (var item in data)
            {
                if (item.ShiftStart.HasValue && item.ShiftEnd.HasValue)
                {
                    var shift = new ShiftsDtos
                    {
                        StartTime = item.ShiftStart.Value,
                        EndTime = item.ShiftEnd.Value,
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,
                    };

                    var attendance = new Attendance

                    {
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,
                        Date = item.Date,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut
                    };

                    var analitics = _repo.AnalyzeAttendance(shift, attendance);
                    analitics.EmployeeId = item.EmployeeId;
                    analitics.StoreId = item.StoreId;
                    analitics.EmployeeName = item.EmployeeName;
                    analitics.StoreName = item.StoreName;
                    analitics.ShiftDisplay = item.ShiftDisplay;



                    analysisList.Add(analitics);

                }
                else if (item.DayOff == true)
                {

                    var shift = new ShiftsDtos
                    {
                        StartTime = item.ShiftStart,
                        EndTime = item.ShiftEnd,
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,

                    };

                    var attendance = new Attendance

                    {
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,
                        Date = item.Date,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut
                    };

                    var analitics = _repo.AnalyzeAttendance(shift, attendance);
                    analitics.EmployeeId = item.EmployeeId;
                    analitics.StoreId = item.StoreId;
                    analitics.EmployeeName = item.EmployeeName;
                    analitics.StoreName = item.StoreName;
                    analitics.ShiftDisplay = item.ShiftDisplay;
                    analitics.Status = "Istirahət";

                    analysisList.Add(analitics);

                }
            }
            dgvAttendance.DataSource = analysisList
                     .Select(a => new
                     {
                         a.Ad_Familiya,
                         a.Mağaza_Adı,
                         a.Tarix,
                         a.Növbə,
                         a.Giriş_vaxtı,
                         a.Çıxış_Vaxtı,
                         a.Gecikmə,
                         a.Icazə,
                         a.Status
                     }).ToList();
        }

        private async void txtaxtar_TextChanged(object sender, EventArgs e)
        {
            int storeId = Convert.ToInt32(cmbStore.SelectedValue);
            DateTime selectedDate = dtpDate.Value.Date;
            var str = txtaxtar.Text;
            var data = await _repo.Search(storeId, selectedDate, str);



            var analysisList = new List<AttendanceAnalysis>();

            foreach (var item in data)
            {
                if (item.ShiftStart.HasValue && item.ShiftEnd.HasValue)
                {
                    var shift = new ShiftsDtos
                    {
                        StartTime = item.ShiftStart.Value,
                        EndTime = item.ShiftEnd.Value,
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,
                    };

                    var attendance = new Attendance

                    {
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,
                        Date = item.Date,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut
                    };

                    var analitics = _repo.AnalyzeAttendance(shift, attendance);
                    analitics.EmployeeId = item.EmployeeId;
                    analitics.StoreId = item.StoreId;
                    analitics.EmployeeName = item.EmployeeName;
                    analitics.StoreName = item.StoreName;
                    analitics.ShiftDisplay = item.ShiftDisplay;



                    analysisList.Add(analitics);

                }
                else if (item.DayOff == true)
                {

                    var shift = new ShiftsDtos
                    {
                        StartTime = item.ShiftStart,
                        EndTime = item.ShiftEnd,
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,

                    };

                    var attendance = new Attendance

                    {
                        EmployeeId = item.EmployeeId,
                        StoreId = item.StoreId,
                        Date = item.Date,
                        CheckIn = item.CheckIn,
                        CheckOut = item.CheckOut
                    };

                    var analitics = _repo.AnalyzeAttendance(shift, attendance);
                    analitics.EmployeeId = item.EmployeeId;
                    analitics.StoreId = item.StoreId;
                    analitics.EmployeeName = item.EmployeeName;
                    analitics.StoreName = item.StoreName;
                    analitics.ShiftDisplay = item.ShiftDisplay;
                    analitics.Status = "Istirahət";

                    analysisList.Add(analitics);

                }
            }
            dgvAttendance.DataSource = analysisList
                     .Select(a => new
                     {
                         a.Ad_Familiya,
                         a.Mağaza_Adı,
                         a.Tarix,
                         a.Növbə,
                         a.Giriş_vaxtı,
                         a.Çıxış_Vaxtı,
                         a.Gecikmə,
                         a.Icazə,
                         a.Status
                     }).ToList();
        }

        private void dgvEmployees_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvEmployees.Columns[e.ColumnIndex].Name == "Şifrə")
            {
                e.Value = "********";
                e.FormattingApplied = true;
            }
        }



        private async Task searchload()
        {
            var searchText = txtSearchEmployee.Text.Trim();
            var employees = await _repo.SearchEmployee(searchText);
            //dgvAttendance.DataSource = null;
            //dgvAttendance.Rows.Clear();
            //dgvAttendance.Columns.Clear();
            var emp = employees.Select(a => new
            {
                a.Ad_Familiya,
                a.Istifadəçi_Adı,
                a.Şifrə,
                Admin = a.Admin ? "Admin" : "------------"
            }).ToList();
            dgvEmployees.DataSource = emp;
        }

        private async void btnAddEmployee_Click(object sender, EventArgs e)
        {

        }

        private void tabControl_EnabledChanged(object sender, EventArgs e)
        {

        }

        private async void txtSearchEmployee_TextChanged(object sender, EventArgs e)
        {
            await searchload();
        }

        private void dungeonComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void hopeDatePicker1_Click(object sender, EventArgs e)
        {

        }
    }
}
