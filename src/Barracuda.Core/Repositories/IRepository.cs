using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Barracuda.Core
{
    public interface IRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : class, IEntity<int>
    {
    }
    
    /// <summary>
    /// Represents an entity repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TKey">Identifier type</typeparam>
    public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<TEntity> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature)
        /// Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<TEntity> TableNoTracking { get; }
        
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        Task<TEntity> FindByIdAsync(TKey id);

        /// <summary>
        /// Get entities by predicate
        /// </summary>
        /// <param name="predicate">Expression to filter by</param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="id">Entity identifier</param>
        Task DeleteAsync(TKey id);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task DeleteAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update entity by specific property or more than one properties
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="properties">Properties</param>
        Task UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
    }
}