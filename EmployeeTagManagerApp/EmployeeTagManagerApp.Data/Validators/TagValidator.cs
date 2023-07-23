using EmployeeTagManagerApp.Data.Models;
using FluentValidation;

namespace EmployeeTagManagerApp.Data.Validators
{
    public class TagValidator : AbstractValidator<Tag>
    {
        public TagValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, 50)
                .Matches(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s\-\`']*$");

            RuleFor(x => x.Description)
                .Length(1, 255)
                .Matches(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s\-\`']*$");
        }
    }
}
