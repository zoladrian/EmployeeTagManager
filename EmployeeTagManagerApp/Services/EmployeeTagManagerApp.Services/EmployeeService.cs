using EmployeeTagManagerApp.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeTagManagerApp.Data;
using System;
using Microsoft.EntityFrameworkCore;
using EmployeeTagManagerApp.Data.Models;

namespace EmployeeTagManagerApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ManagerDbContext _dbContext;

        public EmployeeService(ManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _dbContext.Employees.Include(e => e.Tags).ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _dbContext.Employees.Include(e => e.Tags).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateEmployeeAsync(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
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
    }
}
