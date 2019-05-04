using Gong.Common.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Gong.Common.Infrastructure
{
    public  class BaseEFRepository<TEntity> : IDisposable, IRepository<TEntity>
  where TEntity : class
    {
        protected DbContext context;
        internal DbSet<TEntity> dbSet;
        public BaseEFRepository(DbContext Context)
        {
            this.context = Context;
            this.dbSet = context.Set<TEntity>();
        }
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] include)
        {

            IQueryable<TEntity> query = dbSet;
            if (include != null)
            {
                foreach (var table in include)
                {
                    query = query.Include(table);
                }
            }
            if (filter != null)
            {
                
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }


        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }
        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }
        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }

            dbSet.Remove(entityToDelete);
        }
        public virtual void Attach(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        public void Dispose()
        {
            try
            {
                this.context.Dispose();
            }
            catch (Exception) { }
        }
        public virtual int SaveChanges()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}