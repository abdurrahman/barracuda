using FluentValidation;

namespace Barracuda.Application.Users
{
    public class UserBlockDto
    {
        public string ComplaintUserId { get; set; }
        public string BlockedUserId { get; set; }
    }
    
    public class BlockDtoValidator : AbstractValidator<UserBlockDto>
    {
        public BlockDtoValidator()
        {
            RuleFor(c => c.ComplaintUserId).NotEmpty();
            RuleFor(c => c.BlockedUserId).NotEmpty();
        }
    }
}