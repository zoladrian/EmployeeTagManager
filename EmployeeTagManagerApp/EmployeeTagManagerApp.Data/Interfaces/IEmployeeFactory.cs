using EmployeeTagManagerApp.Data.Models;

namespace EmployeeTagManagerApp.Data.Interfaces
{
    public interface IEmployeeFactory
    {
        Employee Create(string[] values);
    }
}