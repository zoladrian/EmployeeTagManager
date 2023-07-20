using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTagManagerApp.Abstraction.Interfaces
{
    public interface IEmployeeFactory
    {
        Employee Create(string[] values);
    }
}
