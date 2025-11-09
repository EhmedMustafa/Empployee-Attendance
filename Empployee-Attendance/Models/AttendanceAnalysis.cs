using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empployee_Attendance.Models
{
    public class AttendanceAnalysis
    {
        public int EmployeeId { get; set; }
        public int StoreId { get; set; }
        public string EmployeeName { get; set; }
        public string StoreName { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? ShiftStart { get; set; }
        public TimeSpan? ShiftEnd { get; set; }
        public string ShiftDisplay { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string LateBy { get; set; }
        public string LeftEarlyBy { get; set; }
        public string Status { get; set; }

        public string Gəlmə_vaxtı => ShiftStart.ToString();
        public string Getmə_vaxtı => ShiftEnd.ToString();
        public string Giriş_vaxtı => CheckIn?.ToString("HH:mm:ss");
        public string Çıxış_Vaxtı => CheckOut?.ToString("HH:mm:ss");
        public string Tarix => Date?.ToString("dd-MM-yyyy");

        public string Gecikmə => LateBy ?? "0 dəq";
        public string Icazə => LeftEarlyBy ?? "0 dəq";

        public string Ad_Familiya => EmployeeName ?? "-";
        public string Mağaza_Adı => StoreName ?? "-";


        public string Növbə => ShiftDisplay ?? "-";
    }
}
