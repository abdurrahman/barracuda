using System.Collections.Generic;
using System.Collections.Immutable;

namespace Barracuda.Core
{
    public class AggregateRoot : IAggregateRoot
    {
        public AggregateRoot()
        {
            ActivityLogs = new List < ActivityLog>();
        }
        
        private ICollection<ActivityLog> ActivityLogs { get; }

        public void AddActivityLog(ActivityLog log)
        {
            ActivityLogs.Add(log);
        }

        public ImmutableList<ActivityLog> GetUncommittedActivityLogs()
        {
            return ActivityLogs.ToImmutableList();
        }
    }

    public interface IAggregateRoot
    {
        void AddActivityLog(ActivityLog log);
        ImmutableList<ActivityLog> GetUncommittedActivityLogs();
    }
}