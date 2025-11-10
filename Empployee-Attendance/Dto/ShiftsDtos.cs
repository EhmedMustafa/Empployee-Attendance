using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Empployee_Attendance.Dto
{
    public class ShiftsDtos
    {
        public int ShiftId { get; set; }
        public int StoreId { get; set; }

        [JsonProperty("StoreName")]
        public string StoreName { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }

        //public string ShiftDisplay { get; set; }
        [JsonProperty("StartTime")]
        public TimeSpan? StartTime { get; set; }

        [JsonProperty("EndTime")]
        public TimeSpan? EndTime { get; set; }

        [JsonProperty("DayOff")]
        public bool DayOff { get; set; }

        public string ShiftDisplay => DayOff ? "istirahət" : $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
    }
}
