using System.Threading.Tasks;
using Barracuda.Core.Authorization;
using Barracuda.Domain;
using Barracuda.WebApi.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Barracuda.WebApi.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenHelper<ApplicationUser> _tokenHelper;

        public AccountController(SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, 
            ITokenHelper<ApplicationUser> tokenHelper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenHelper = tokenHelper;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return ApiResponse(ModelState);
            
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, 
            model.RememberMe, true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                
                var tokenResponse = _tokenHelper.GenerateToken(user);
                
                return ApiResponse(tokenResponse);
            }

            if (result.IsLockedOut)
            {
                AddError("This user is temporarily blocked");
                return ApiResponse();
            }

            AddError("Incorrect user or password");
            return ApiResponse();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return ApiResponse(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var tokenResponse = _tokenHelper.GenerateToken(user);
                return ApiResponse(tokenResponse);
            }

            foreach (var error in result.Errors)
            {
                AddError(error.Description);
            }

            return ApiResponse();
        }
    }
}