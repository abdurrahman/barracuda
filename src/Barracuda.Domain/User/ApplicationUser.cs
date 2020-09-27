using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Barracuda.Core;

namespace Barracuda.Domain
{
    public class ApplicationUser : IdentityUser, IAggregateRoot
    {
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public void AddActivityLog(ActivityLog log)
        {
            throw new NotImplementedException();
        }

        public ImmutableList<ActivityLog> GetUncommittedActivityLogs()
        {
            throw new NotImplementedException();
        }
    }
}