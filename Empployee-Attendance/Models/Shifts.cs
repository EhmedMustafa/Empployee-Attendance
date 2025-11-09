using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empployee_Attendance.Models
{
    public class Shifts
    {
        public int ShiftId { get; set; }
        public int StoreId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        //public string ShiftDisplay { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool DayOff { get; set; }


        public Store Stores { get; set; }
        public Employee Employees { get; set; }

        public string ShiftDisplay => DayOff ? "istirahət" : $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
    }
}
