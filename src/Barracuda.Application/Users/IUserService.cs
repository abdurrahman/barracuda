using System.Threading.Tasks;
using Barracuda.Domain;

namespace Barracuda.Application.Users
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByUserName(string username);
        
        Task BlockUser(UserBlockDto model);
    }
}