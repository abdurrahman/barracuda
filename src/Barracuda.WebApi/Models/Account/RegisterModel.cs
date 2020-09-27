using FluentValidation;

namespace Barracuda.WebApi.Models.Account
{
    public class RegisterModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty()
                .WithMessage("Firstname is required")
                .MaximumLength(100)
                .WithMessage("Firstname must be less than 100 character");
            
            RuleFor(c => c.LastName)
                .NotEmpty()
                .WithMessage("Lastname is required")
                .MaximumLength(100)
                .WithMessage("Lastname must be less than 100 character");

            RuleFor(c => c.UserName)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(254);
               
            RuleFor(c => c.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(100);
        }
    }
}