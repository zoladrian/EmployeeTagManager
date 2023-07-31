using EmployeeTagManagerApp.Data.Interfaces;
using EmployeeTagManagerApp.Data.Models;
using EmployeeTagManagerApp.Events;
using FluentValidation;
using FluentValidation.Results;
using Prism.Events;

namespace EmployeeTagManagerApp.Data.Factories
{
    public class EmployeeFactory : IEmployeeFactory
    {
        private readonly IValidator<Employee> _validator;
        private readonly IEventAggregator _eventAggregator;

        public EmployeeFactory(IValidator<Employee> validator, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _validator = validator;
        }
        public Employee Create(string[] values)
        {
            var employee = new Employee
            {
                Id = int.Parse(values[0]),
                Name = CleanInput(values[1]),
                Surname = CleanInput(values[2]),
                Email = CleanInput(values[3]),
                Phone = CleanInput(values[4])
            };

            ValidationResult results = _validator.Validate(employee);

            if (!results.IsValid)
            {
                var errorMessage = string.Join(", ", results.Errors.Select(error => error.ErrorMessage));
                _eventAggregator.GetEvent<ErrorOccurredEvent>().Publish($"An error occurred while creating an employee: {errorMessage}");
                return null;
            }

            return employee;
        }
        private string CleanInput(string input)
        {
            return input.Replace("'", "''").Trim('\r').Trim();
        }
    }
}
