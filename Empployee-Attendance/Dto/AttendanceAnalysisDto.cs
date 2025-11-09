using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empployee_Attendance.Dto
{
    public class AttendanceAnalysisDto
    {
        public int EmployeeId { get; set; }
        public int StoreId { get; set; }
        public string EmployeeName { get; set; }
        public string StoreName { get; set; }
        public DateTime Date { get; set; }
        public string ShiftStartStr { get; set; }
        public string ShiftEndStr { get; set; }
        public string CheckInStr { get; set; }
        public string CheckOutStr { get; set; }
        public string LateBy { get; set; }
        public string LeftEarlyBy { get; set; }
        public string Status { get; set; }
    }
}
