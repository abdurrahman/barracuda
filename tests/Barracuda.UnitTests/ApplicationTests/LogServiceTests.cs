using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Barracuda.Application;
using Barracuda.Core;
using NSubstitute;
using Xunit;

namespace Barracuda.UnitTests.ApplicationTests
{
    public class LogServiceTests
    {
        private readonly IRepository<ActivityLog> _logRepo;

        public LogServiceTests()
        {
            _logRepo = Substitute.For<IRepository<ActivityLog>>();
        }

        [Fact]
        public async Task AddActivityLog_InvokeRepository_AsOnce()
        {
            var logType = ActivityLogType.Login;
            var userId = "13";
            var activityLogList = new List<ActivityLog>();
            _logRepo.When(c => c.InsertAsync(Arg.Any<ActivityLog>()))
                .Do(c => activityLogList.Add(c.ArgAt<ActivityLog>(0)));
            
            var activityLog = new ActivityLog
            {
                IpAddress = "127.0.0.0",
                UserId = userId,
                ActivityLogType = logType
            };
            
            var logService = new LogService(_logRepo);
            await logService.AddActivityLog(logType, userId);

            Assert.Single(activityLogList);
            Assert.Equal(activityLog.UserId, activityLogList.First().UserId);
            Assert.Equal(activityLog.ActivityLogType, activityLogList.First().ActivityLogType);
        }
    }
}