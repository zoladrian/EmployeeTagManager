using EmployeeTagManagerApp.Data;
using EmployeeTagManagerApp.Data.Models;
using EmployeeTagManagerApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTagManagerApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ManagerDbContext _dbContext;
        private readonly IEventAggregator _eventAggregator;

        public EmployeeService(ManagerDbContext dbContext, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _dbContext.Employees
                .Include(e => e.EmployeeTags)
                .ThenInclude(et => et.Tag)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _dbContext.Employees
                .AsNoTracking()
                .Include(e => e.EmployeeTags)
                .ThenInclude(et => et.Tag)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employee.Id);

            if (existingEmployee != null)
            {
                existingEmployee.Name = employee.Name;
                existingEmployee.Surname = employee.Surname;
                existingEmployee.Email = employee.Email;
                existingEmployee.Phone = employee.Phone;
                existingEmployee.EmployeeTags = employee.EmployeeTags;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task AddTagToEmployeeAsync(int employeeId, int tagId)
        {
            var employeeTag = new EmployeeTag
            {
                EmployeeId = employeeId,
                TagId = tagId
            };

            _dbContext.EmployeeTags.Add(employeeTag);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveTagFromEmployeeAsync(int employeeId, int tagId)
        {
            var employeeTag = await _dbContext.EmployeeTags
                .FirstOrDefaultAsync(et => et.EmployeeId == employeeId && et.TagId == tagId);

            if (employeeTag != null)
            {
                _dbContext.EmployeeTags.Remove(employeeTag);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Tag>> GetTagsForEmployeeAsync(int employeeId)
        {
            return await _dbContext.EmployeeTags
                .Where(et => et.EmployeeId == employeeId)
                .Select(et => et.Tag)
                .ToListAsync();
        }
    }
}