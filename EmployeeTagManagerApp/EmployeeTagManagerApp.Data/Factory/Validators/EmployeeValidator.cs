using EmployeeTagManagerApp.Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTagManagerApp.Data.Factory.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(1, 50).Matches(@"^[a-zA-Z\s]*$");
            RuleFor(x => x.Surname).NotEmpty().Length(1, 50).Matches(@"^[a-zA-Z\s]*$");
            RuleFor(x => x.Email).NotEmpty().Length(1, 255).EmailAddress();
            RuleFor(x => x.Phone).NotEmpty().Length(10).Matches(@"^\d+$");
        }
    }
}
