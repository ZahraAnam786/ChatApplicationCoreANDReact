using Domain.Interfaces;
using Domain.UnitOfWork;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
    {
        protected readonly ApplicationDBContext Context;
        protected readonly DbSet<TEntity> Set;
        protected readonly IUnitOfWorkAsync UnitOfWork;

        public Repository(ApplicationDBContext context, IUnitOfWorkAsync unitOfWork)
        {
            UnitOfWork = unitOfWork;
            Context = context;
            Set = context.Set<TEntity>();
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return Set.Find(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await Set.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await Set.FindAsync(keyValues, cancellationToken);
        }

        public virtual void Insert(TEntity entity)
        {
            Set.Add(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            Set.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            Set.Update(entity);
        }

        public virtual void Delete(params object[] keyValues)
        {
            var entity = Set.Find(keyValues);
            if (entity != null)
                Set.Remove(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            Set.Remove(entity);
        }

        public virtual void Delete(object id)
        {
            var entity = Set.Find(id);
            Delete(entity);
        }

        public virtual async Task<bool> DeleteAsync(params object[] keyValues)
        {
            var entity = await FindAsync(keyValues);
            if (entity == null) return false;
            Delete(entity);
            return true;
        }

        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await FindAsync(cancellationToken, keyValues);
            if (entity == null) return false;
            Delete(entity);
            return true;
        }

        public virtual IQueryable<TEntity> Queryable()
        {
            return Set.AsQueryable();
        }

        //public IQueryFluent<TEntity> Query() => new QueryFluent<TEntity>(this);

        //public virtual IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject) => new QueryFluent<TEntity>(this, queryObject);

        //public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query) => new QueryFluent<TEntity>(this, query);

        internal IQueryable<TEntity> Select(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = Set;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (page.HasValue && pageSize.HasValue)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return query;
        }

        internal async Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            return await Select(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> SelectQueryAsync(string query, params object[] parameters)
        {
            return await Set.FromSqlRaw(query, parameters).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> SelectQueryAsync(string query, CancellationToken cancellationToken, params object[] parameters)
        {
            return await Set.FromSqlRaw(query, parameters).ToListAsync(cancellationToken);
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return UnitOfWork.Repository<T>();
        }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return Set.FromSqlRaw(query, parameters).AsQueryable();
        }


        public void ApplyChanges(TEntity entity)
        {
            throw new NotImplementedException();
        }


    }
}
