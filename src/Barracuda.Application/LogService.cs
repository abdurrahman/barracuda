using System.Threading.Tasks;
using Barracuda.Core;
using Barracuda.Core.Logging;

namespace Barracuda.Application
{
    public class LogService : IActivityLog
    {
        private readonly IRepository<ActivityLog> _activityRepository;

        public LogService(IRepository<ActivityLog> activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task AddActivityLog(ActivityLogType logType, string userId)
        {
            await _activityRepository.InsertAsync(new ActivityLog
            {
                IpAddress = "127.0.0.1", // Todo: Add IP address getter extension
                UserId = userId,
                ActivityLogType = logType
            });
        }
    }
}