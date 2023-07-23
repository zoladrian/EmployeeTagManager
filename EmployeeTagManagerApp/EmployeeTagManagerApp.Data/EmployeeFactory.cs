﻿using EmployeeTagManagerApp.Data.Interfaces;
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
                Name = CleanInput(values[1]),
                Surname = CleanInput(values[2]),
                Email = CleanInput(values[3]),
                Phone = CleanInput(values[4])
            };

            ValidationResult results = _validator.Validate(employee);

            if (!results.IsValid)
            {
                var the = employee;
                throw new ValidationException(results.Errors);
            }

            return employee;
        }
        private string CleanInput(string input)
        {
            return input.Replace("'", "''").Trim('\r').Trim();
        }
    }
}
