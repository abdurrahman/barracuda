using System;
using System.Threading.Tasks;
using Barracuda.Core;
using Barracuda.Core.Authorization;
using Barracuda.Core.Logging;
using Barracuda.Domain;
using Barracuda.WebApi.Hubs;
using Barracuda.WebApi.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Barracuda.WebApi.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenHelper<ApplicationUser> _tokenHelper;
        private readonly IActivityLog _activityLog;
        private readonly IHubContext<MessageHub> _hubContext;

        public AccountController(SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, 
            ITokenHelper<ApplicationUser> tokenHelper, 
            IActivityLog activityLog, 
            IHubContext<MessageHub> hubContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenHelper = tokenHelper;
            _activityLog = activityLog;
            _hubContext = hubContext;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return ApiResponse(ModelState);
            
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, 
            model.RememberMe, true);
            if (result.Succeeded)
            {
                await _activityLog.AddActivityLog(ActivityLogType.Login, UserId);
                
                var user = await _userManager.FindByNameAsync(model.UserName);
                // Send onConnected event to hub
                await _hubContext.Clients.All.SendAsync("OnConnected", DateTime.Now, 
                    $"{user.FirstName} {user.LastName} logged in" );
                
                var tokenResponse = _tokenHelper.GenerateToken(user);
                return ApiResponse(tokenResponse);
            }

            if (result.IsLockedOut)
            {
                await _activityLog.AddActivityLog(ActivityLogType.IsAccountLockedOut, UserId);
                AddError("This user is temporarily blocked");
                return ApiResponse();
            }

            await _activityLog.AddActivityLog(ActivityLogType.InvalidLogin, UserId);

            AddError("Incorrect user or password");
            return ApiResponse();
        }

        [HttpPost]
        [AllowAnonymous]
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
                await _activityLog.AddActivityLog(ActivityLogType.Register, user.Id);
                
                await _hubContext.Clients.All.SendAsync("NewUser", DateTime.Now, 
                    $"{user.FirstName} {user.LastName} welcome aboard" );
                
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