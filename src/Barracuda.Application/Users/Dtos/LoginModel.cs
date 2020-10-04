using FluentValidation;

namespace Barracuda.Application.Users.Dtos
{
    public class LoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public LoginModel()
        {
        }
        
        public LoginModel(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
        
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(c => c.UserName).NotEmpty();
            RuleFor(c => c.Password).NotEmpty();
        }
    }
}