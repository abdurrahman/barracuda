using Barracuda.Core;
using Barracuda.Core.Repositories;

namespace Barracuda.Application
{
    public class BarracudaEfRepository<TEntity> : EfRepository<BarracudaDbContext, TEntity>
        where TEntity : class, IEntity<int>
    {
        public BarracudaEfRepository(BarracudaDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
    
    public class BarracudaEfRepository<TEntity, TKey> : EfRepository<BarracudaDbContext, TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public BarracudaEfRepository(BarracudaDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}