using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;
using Empployee_Attendance.Dto;
using Empployee_Attendance.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Empployee_Attendance.Repository
{
   public class EmployeeRepository
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:3000/api/";
        private string _token;

        public EmployeeRepository()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task<EmployeeLoginDtos> GetUserByname(string username, string password)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/login", new { 
                    username, 
                    password 
                });

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    _token = result.Token;
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                    return new EmployeeLoginDtos
                    {
                        EmployeeId = result.Employee.Id,
                        UserName = result.Employee.FullName,
                        IsAdmin = result.Employee.IsAdmin
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return null;
            }
        }

        private class LoginResponse
        {
            public string Token { get; set; }
            public EmployeeResponse Employee { get; set; }
        }

        private class EmployeeResponse
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public bool IsAdmin { get; set; }
        }


        public async Task<ShiftsDtos> GetTodayShift(int employeeId)
        {
            try 
            {
                var response = await _httpClient.GetAsync($"shifts/today/{employeeId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var shifts = JsonConvert.DeserializeObject<ShiftsDtos>(json);
                    return shifts?? new ShiftsDtos();

                }
                return new ShiftsDtos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get shift error: {ex.Message}");
                return null;
            }
        }


        public async Task<Attendance> Loadstatus(int employeeId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/attendance/status/{employeeId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Attendance>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Load status error: {ex.Message}");
                return null;
            }
        }

        public AttendanceAnalysis AnalyzeAttendance(ShiftsDtos shifts, Attendance attendance)

        {



            if (shifts != null && attendance != null)
            {
                var result = new AttendanceAnalysis
                {
                    Date = attendance.Date,
                    ShiftStart = shifts.StartTime,
                    ShiftEnd = shifts.EndTime,
                    // ShiftDisplay = shifts.ShiftDisplay,
                    CheckIn = attendance.CheckIn,
                    CheckOut = attendance.CheckOut

                };

                if (attendance.CheckIn != null && shifts.StartTime.HasValue)
                {
                    DateTime shfstart = shifts.Date + shifts.StartTime.Value;
                    if (attendance.CheckIn > shfstart)
                    {
                        TimeSpan? late = attendance.CheckIn.Value - shfstart;
                        var dayweek = shifts.Date.DayOfWeek;
                        if (dayweek == DayOfWeek.Monday || dayweek == DayOfWeek.Sunday)
                        {
                            late = TimeSpan.FromMinutes(late.Value.TotalMinutes * 2);
                        }
                        result.LateBy = $"{late?.TotalMinutes:F0} dəq";
                    }

                }
                else result.LateBy = $"0 dəq";



                if (attendance.CheckOut != null && shifts.EndTime.HasValue)
                {
                    DateTime sftendtime = shifts.Date + shifts.EndTime.Value;

                    if (attendance.CheckOut < sftendtime)
                    {
                        TimeSpan? early = attendance.CheckOut.Value - sftendtime;
                        var dayweek = shifts.Date.DayOfWeek;

                        if (dayweek == DayOfWeek.Saturday || dayweek == DayOfWeek.Sunday)
                        {
                            early = TimeSpan.FromMinutes(early.Value.TotalMinutes * 2);
                        }
                        result.LeftEarlyBy = $"{early?.TotalMinutes:F0} dəq";
                    }

                }
                else result.LeftEarlyBy = $"0 dəq";


                if (attendance.CheckIn == null && attendance.CheckOut == null)
                {
                    var dayweek = shifts.Date.DayOfWeek;

                    if (dayweek == DayOfWeek.Saturday || dayweek == DayOfWeek.Sunday)
                    {
                        result.Status = "Gəlməyib(2 qat)";
                    }

                    else result.Status = "İşə gəlməyib";

                }

                // else if (attendance)

                else if (result.LateBy != null && result.LeftEarlyBy == null)
                {
                    result.Status = "Gec gəlib";
                }
                else if (result.LeftEarlyBy != null && result.LateBy == null)
                {
                    result.Status = "Tez çıxıb";
                }
                else if (result.LateBy != null && result.LeftEarlyBy != null)
                {
                    result.Status = "Gec gəlib və tez çıxıb";
                }
                else if (shifts.StartTime == null && shifts.EndTime == null) result.Status = "Istirahət";

                else result.Status = "Tam";

                return result;
            }

            else return null;

        }


        public async Task<List<Employee>> GetAll()
        {
            try
            {
                var response = await _httpClient.GetAsync("employees");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var employess = JsonConvert.DeserializeObject<List<Employee>>(json);
                    return employess?? new List<Employee>();
                }
                return new List<Employee>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get all employees error: {ex.Message}");
                return new List<Employee>();
            }
        }


        public async Task<List<EmployeeShiftAttendanceDto>> GetAllEmployeeWithAttendance()
        {
            try
            {
                var response = await _httpClient.GetAsync("attendance/all");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var attendances = JsonConvert.DeserializeObject<List<EmployeeShiftAttendanceDto>>(json);
                    return attendances ?? new List<EmployeeShiftAttendanceDto>();
                    //return await response.Content.ReadFromJsonAsync<List<EmployeeShiftAttendanceDto>>();
                }
                return new List<EmployeeShiftAttendanceDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get all attendance error: {ex.Message}");
                return new List<EmployeeShiftAttendanceDto>();
            }
        }

        public async Task<IEnumerable<EmployeeShiftAttendanceDto>> GetAttendanceByStore(int storeId, DateTime selectedDate)
        {
            try

            {
                var response = await _httpClient.GetAsync($"attendance/store/{storeId}?date={selectedDate:yyyy-MM-dd}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var employeesshift = JsonConvert.DeserializeObject<IEnumerable<EmployeeShiftAttendanceDto>>(json);
                    return employeesshift ?? new List<EmployeeShiftAttendanceDto>();
                   // return await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeShiftAttendanceDto>>();
                }
                return new List<EmployeeShiftAttendanceDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get attendance by store error: {ex.Message}");
                return new List<EmployeeShiftAttendanceDto>();
            }
        }


        public async Task<List<Store>> GetAllStores()
        {
            try
            {
                var response = await _httpClient.GetAsync("stores");
                if (response.IsSuccessStatusCode)
                {
                    var json= await response.Content.ReadAsStringAsync();
                    var stores = JsonConvert.DeserializeObject<List<Store>>(json);
                    return stores ?? new List<Store>();
                    //return await response.Content.ReadFromJsonAsync<List<Store>>();
                }
                return new List<Store>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get all stores error: {ex.Message}");
                return new List<Store>();
            }
        }



        public async Task<IEnumerable<EmployeeShiftAttendanceDto>> GetAttendanceByStoreAndDate(int storeId, DateTime selectedDate)
        {
            try
            {
                var response = await _httpClient.GetAsync($"attendance?storeId={storeId}&date={selectedDate:yyyy-MM-dd}");

                if (response.IsSuccessStatusCode)
                {
                    var json= await response.Content.ReadAsStringAsync();
                    var employeesshift = JsonConvert.DeserializeObject<IEnumerable<EmployeeShiftAttendanceDto>>(json);
                    return employeesshift ?? new List<EmployeeShiftAttendanceDto>();
                   // return await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeShiftAttendanceDto>>();
                }
                return new List<EmployeeShiftAttendanceDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get attendance by store and date error: {ex.Message}");
                return new List<EmployeeShiftAttendanceDto>();
            }
        }



        public async Task<List<EmployeeShiftAttendanceDto>> Search(int storeId, DateTime selectedDate, string search)
        {
            try
            {
                var response = await _httpClient.GetAsync($"attendance/search?storeId={storeId}&date={selectedDate:yyyy-MM-dd}&search={Uri.EscapeDataString(search)}");
                if (response.IsSuccessStatusCode)
                {
                    var json= await response.Content.ReadAsStringAsync();
                    var employeesshift = JsonConvert.DeserializeObject<List<EmployeeShiftAttendanceDto>>(json);
                    return employeesshift ?? new List<EmployeeShiftAttendanceDto>();
                    //return await response.Content.ReadFromJsonAsync<List<EmployeeShiftAttendanceDto>>();
                }
                return new List<EmployeeShiftAttendanceDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Search error: {ex.Message}");
                return new List<EmployeeShiftAttendanceDto>();
            }
        }


    }
}
