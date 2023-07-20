using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTagManagerApp.Data.Models
{
    public class EmployeeTag
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
