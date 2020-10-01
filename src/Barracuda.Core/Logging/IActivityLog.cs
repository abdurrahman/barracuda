using System.Threading.Tasks;

namespace Barracuda.Core.Logging
{
    public interface IActivityLog
    {
        Task AddActivityLog(ActivityLogType logType, string userId);
    }
}