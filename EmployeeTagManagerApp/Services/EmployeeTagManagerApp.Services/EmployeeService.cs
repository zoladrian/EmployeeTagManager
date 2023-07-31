using EmployeeTagManagerApp.Data;
using EmployeeTagManagerApp.Data.Models;
using EmployeeTagManagerApp.Events;
using EmployeeTagManagerApp.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
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
        private readonly IValidator<Employee> _validator;

        public EmployeeService(ManagerDbContext dbContext, IEventAggregator eventAggregator, IValidator<Employee> validator)
        {
            _eventAggregator = eventAggregator;
            _dbContext = dbContext;
            _validator = validator;
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
            ValidationResult results = _validator.Validate(employee);

            if (!results.IsValid)
            {
                string errorMessage = string.Join(", ", results.Errors.Select(x => x.ErrorMessage));
                _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish(errorMessage);
                return;
            }

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
            else
            {
                _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish($"Employee with ID {employee.Id} does not exist.");
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
            else
            {
                _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish($"Cannot delete. Employee with ID {id} does not exist.");
            }
        }

        public async Task AddTagToEmployeeAsync(int employeeId, int tagId)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == tagId);

            if (employee == null)
            {
                _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish($"Employee with ID {employeeId} does not exist.");
                return;
            }

            if (tag == null)
            {
                _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish($"Tag with ID {tagId} does not exist.");
                return;
            }

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
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == tagId);

            if (employee == null)
            {
                _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish($"Employee with ID {employeeId} does not exist.");
                return;
            }

            if (tag == null)
            {
                _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish($"Tag with ID {tagId} does not exist.");
                return;
            }

            var employeeTag = await _dbContext.EmployeeTags
                .FirstOrDefaultAsync(et => et.EmployeeId == employeeId && et.TagId == tagId);

            if (employeeTag != null)
            {
                _dbContext.EmployeeTags.Remove(employeeTag);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish($"Tag with ID {tagId} does not exist for employee with ID {employeeId}.");
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