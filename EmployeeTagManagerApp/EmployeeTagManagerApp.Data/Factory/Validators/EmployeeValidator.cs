using EmployeeTagManagerApp.Data.Models;
using FluentValidation;

namespace EmployeeTagManagerApp.Data.Factory.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, 50)
                .Matches(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s\-\`']*$");

            RuleFor(x => x.Surname)
                .NotEmpty()
                .Length(1, 50)
                .Matches(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s\-\`']*$");

            RuleFor(x => x.Email)
                .MaximumLength(255)
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Phone)
                .NotEmpty()
                .Length(8, 11)
                .Matches(@"^\d*$");
        }
    }
}
