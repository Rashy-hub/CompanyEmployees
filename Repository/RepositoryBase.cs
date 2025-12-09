using Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    internal abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext;

        protected RepositoryBase(RepositoryContext context)
        {
            RepositoryContext=context;  
        }
        public void Create(T entity)
        {
           RepositoryContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);    
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return trackChanges ? RepositoryContext.Set<T>() : RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges)
        {
            return trackChanges ? RepositoryContext.Set<T>().Where<T>(condition) : RepositoryContext.Set<T>().AsNoTracking().Where<T>(condition);
        }

        public void Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
        }
    }
}
