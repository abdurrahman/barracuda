using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Barracuda.Core.Repositories
{
    public class EfRepository<TDbContext, TEntity> : EfRepository<TDbContext, TEntity, int>, IRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class, IEntity<int>
    {
        public EfRepository(TDbContext dbContext)
            : base(dbContext)
        {
        }
    }

    /// <summary>
    /// Represents the Entity Framework repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TKey">Identifier type</typeparam>
    public class EfRepository<TDbContext, TEntity, TKey> : IRepository<TEntity, TKey>
        where TDbContext : DbContext
        where TEntity : class, IEntity<TKey>
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly TDbContext _dbContext;

        public EfRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Table => _dbSet;
        public virtual IQueryable<TEntity> TableNoTracking => _dbSet.AsNoTracking();
        
        public virtual async Task<TEntity> FindByIdAsync(TKey id)
            => await _dbSet.FindAsync(id);

        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await FindByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException();
            }

            await DeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            var firstEntity = entities.FirstOrDefault();
            if (firstEntity == null) return;
            _dbSet.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            if (_dbContext.Entry(entity).State != EntityState.Detached)
            {
                _dbContext.Attach(entity);
            }

            var entry = _dbContext.Entry(entity);
            foreach (var property in entry.Properties)
            {
                property.IsModified = false;
            }

            foreach (var selector in properties)
            {
                entry.Property(selector).IsModified = true;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}