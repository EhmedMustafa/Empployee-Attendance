using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empployee_Attendance.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PassWordHash { get; set; }
        public bool IsAdmin { get; set; }

        public string Ad_Familiya => FullName.ToString();
        public string Istifadəçi_Adı => UserName.ToString();
        public string Şifrə => PassWordHash.ToString();
        public bool Admin => IsAdmin;
    }
}
