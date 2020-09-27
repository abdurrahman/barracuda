using Barracuda.Core;
using Barracuda.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Barracuda.Application
{
    public class BarracudaDbContext : IdentityDbContext<ApplicationUser>
    {
        public BarracudaDbContext(DbContextOptions<BarracudaDbContext> options) 
            : base(options)
        {
        }

        protected BarracudaDbContext()
        {
        }

        public DbSet<Domain.Message> Messages { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
    }
}