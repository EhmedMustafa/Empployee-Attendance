using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Empployee_Attendance.Dto;
using Newtonsoft.Json;

namespace Empployee_Attendance.Models
{
    public class LoginResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("employee")]
        public EmployeeDto Employee { get; set; }
    }
}
