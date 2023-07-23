using EmployeeTagManagerApp.Data.Interfaces;
using EmployeeTagManagerApp.Data.Models;
using FluentValidation;
using FluentValidation.Results;

namespace EmployeeTagManagerApp.Data
{
    public class EmployeeFactory : IEmployeeFactory
    {
        private readonly IValidator<Employee> _validator;

        public EmployeeFactory(IValidator<Employee> validator)
        {
            _validator = validator;
        }

        public Employee Create(string[] values)
        {
            var employee = new Employee
            {
                Id = int.Parse(values[0]),
                Name = values[1].Trim(),
                Surname = values[2].Trim(),
                Email = values[3].Trim(),
                Phone = values[4].Trim()
            };

            ValidationResult results = _validator.Validate(employee);

            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            return employee;
        }
    }
}
