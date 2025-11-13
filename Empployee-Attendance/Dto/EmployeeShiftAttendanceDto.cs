using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Empployee_Attendance.Dto
{
    public class EmployeeShiftAttendanceDto
    {
        [JsonProperty("employeeId")]
        public int EmployeeId { get; set; }

        [JsonProperty("EmployeeName")]
        public string EmployeeName { get; set; }

        [JsonProperty("id")]
        public int StoreId { get; set; }

        [JsonProperty("StoreName")]
        public string StoreName { get; set; }
        //public string ShiftDisplay { get; set; }

        [JsonProperty("shiftStart")]
        public TimeSpan? ShiftStart { get; set; }

        [JsonProperty("shiftEnd")]
        public TimeSpan? ShiftEnd { get; set; }

        [JsonProperty("checkIn")]
        public DateTime? CheckIn { get; set; }

        [JsonProperty("checkOut")]
        public DateTime? CheckOut { get; set; }

        public DateTime Date { get; set; }

        [JsonProperty("dayOff")]
        public bool DayOff { get; set; }

       
        public string ShiftDisplay => DayOff ? "istirahət" : $"{ShiftStart:hh\\:mm} - {ShiftEnd:hh\\:mm}";
    }
}
