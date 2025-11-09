using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empployee_Attendance.Dto
{
    public class EmployeeShiftAttendanceDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        //public string ShiftDisplay { get; set; }
        public TimeSpan? ShiftStart { get; set; }
        public TimeSpan? ShiftEnd { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public DateTime Date { get; set; }

        public bool DayOff { get; set; }
        public string ShiftDisplay => DayOff ? "istirahət" : $"{ShiftStart:hh\\:mm} - {ShiftEnd:hh\\:mm}";
    }
}
