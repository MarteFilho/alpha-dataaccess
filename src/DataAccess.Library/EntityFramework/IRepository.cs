using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Library.EntityFramework
{
    public interface IRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<bool> ExistsByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<List<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllByAsync(params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<List<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<List<TEntity>> FindAllWithOrderBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy, params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<IList<TEntity>> FindAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeExpressions);
        TEntity Update(TEntity entity);
        Task UpdateRangeAsync(IList<TEntity> entities);
        Task<bool> DeleteAsync(TEntity entity);
        Task AddRange(List<TEntity> entites);

        void RemoveRange(List<TEntity> entites);
    }
}