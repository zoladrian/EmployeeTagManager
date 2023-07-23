using EmployeeTagManagerApp.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeTagManagerApp.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();

        Task<Employee> GetEmployeeByIdAsync(int id);

        Task UpdateEmployeeAsync(Employee employee);

        Task DeleteEmployeeAsync(int id);

        Task AddTagToEmployeeAsync(int employeeId, int tagId);

        Task RemoveTagFromEmployeeAsync(int employeeId, int tagId);

        Task<IEnumerable<Tag>> GetTagsForEmployeeAsync(int employeeId);
    }
}