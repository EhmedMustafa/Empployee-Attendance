using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empployee_Attendance.Dto
{
   public class EmployeeLoginDtos
    {
        public int EmployeeId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string UserName { get; set; }
        public string PassWordHash { get; set; }
        public bool IsAdmin { get; set; }

    }
}
