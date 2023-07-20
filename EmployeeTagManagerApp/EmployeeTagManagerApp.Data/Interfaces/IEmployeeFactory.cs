using EmployeeTagManagerApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTagManagerApp.Data.Interfaces
{
    public interface IEmployeeFactory
    {
        Employee Create(string[] values);
    }
}
