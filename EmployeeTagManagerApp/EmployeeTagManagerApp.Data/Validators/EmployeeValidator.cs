using EmployeeTagManagerApp.Data.Models;
using FluentValidation;

namespace EmployeeTagManagerApp.Data.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(employee => employee.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Matches(@"^[\p{L} .'-]+$").WithMessage("Name can't contain numbers or special characters.")
                .MaximumLength(50).WithMessage("Name can't be longer than 50 characters.");

            RuleFor(employee => employee.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .Matches(@"^[\p{L} .'-]+$").WithMessage("Surname can't contain numbers or special characters.")
                .MaximumLength(50).WithMessage("Surname can't be longer than 50 characters.");

            RuleFor(employee => employee.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.")
                .Must(email => email.Contains("@")).WithMessage("Email must contain '@' symbol.")
                .MaximumLength(100).WithMessage("Email can't be longer than 100 characters.");

            RuleFor(employee => employee.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^[0-9 \-+]+$").WithMessage("Phone number can contain only digits, spaces, hyphen (-), and plus sign (+).")
                .MaximumLength(20).WithMessage("Phone number can't be longer than 20 characters.");
        }
    }


}
