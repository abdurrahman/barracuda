using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Barracuda.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected string UserId => GetAuthenticatedUserId();
        
        private readonly ICollection<string> _errors = new List<string>();
        
        protected ActionResult ApiResponse(object result = null)
        {
            if (IsOperationValid())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", _errors.ToArray() }
            }));
        }
        
        protected ActionResult ApiResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                AddError(error.ErrorMessage);
            }

            return ApiResponse();
        }
        
        protected ActionResult ApiResponse(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddError(error.ErrorMessage);
            }

            return ApiResponse();
        }
        
        protected bool IsOperationValid()
        {
            return !_errors.Any();
        }
        
        protected void AddError(string error)
        {
            _errors.Add(error);
        }

        protected void ClearErrors()
        {
            _errors.Clear();
        }
        
        /// <summary>
        ///     Get user id info from user claim
        /// </summary>
        private string GetAuthenticatedUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Claims.First(i => i.Type == "UserId").Value;
                return userId;
            }

            return string.Empty;
        }
    }
}