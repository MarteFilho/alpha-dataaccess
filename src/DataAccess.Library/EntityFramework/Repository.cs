using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Library.EntityFramework
{
    public class Repository<TEntity, TContext> : IRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        protected TContext _context;
        public Repository(TContext context)
        {
            _context = context;
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await CreateSet().AddAsync(entity);
            return entity;
        }
        public async Task AddRange(List<TEntity> entities)
        {
            await CreateSet().AddRangeAsync(entities);
        }

        private DbSet<TEntity> CreateSet()
        {
            return _context.Set<TEntity>();
        }

        public async Task<bool> ExistsByAsync(Expression<Func<TEntity, bool>> query)
        {
            return await CreateSet().AsNoTracking().AnyAsync(query);
        }

        public async Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> query)
        {
            return await CreateSet().AsNoTracking().Where(query).FirstOrDefaultAsync();
        }
        public async Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return await Where(predicate, includeExpressions).FirstOrDefaultAsync();
        }

        public TEntity Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public async Task UpdateRangeAsync(IList<TEntity> entities)
        {
            CreateSet().UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            return Task.FromResult(true);
        }

        public async Task<List<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> query)
        {
            return await CreateSet().Where(query).AsNoTracking().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllByAsync(params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return await SetIncludeExpressions(CreateSet(), includeExpressions).AsNoTracking().ToListAsync();
        }
        public Task<List<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return Where(predicate, includeExpressions).ToListAsync();
        }

        private static IQueryable<TEntity> SetIncludeExpressions(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            foreach (var item in includeExpressions)
            {
                query = query.Include(item);
            }

            return query;
        }
        private IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return CreateSet().Where(predicate);
        }

        private IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            var query = Where(predicate);

            return SetIncludeExpressions(query, includeExpressions);
        }
        public async Task<List<TEntity>> FindAllWithOrderBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return await Where(predicate, includeExpressions).AsNoTracking().OrderByDescending(orderBy).ToListAsync();
        }

        public virtual async Task<IList<TEntity>> FindAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return await Where(predicate, includeExpressions).AsNoTracking().ToListAsync();
        }
        public async Task<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return await Where(predicate, includeExpressions).AsNoTracking().FirstOrDefaultAsync();
        }

        public void RemoveRange(List<TEntity> entites) => CreateSet().RemoveRange(entites);
    }
}