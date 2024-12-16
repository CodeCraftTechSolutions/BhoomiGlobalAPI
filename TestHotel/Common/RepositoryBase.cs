
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BhoomiGlobalAPI.Common
{
    public abstract class RepositoryBase<T> where T : class
        {
            #region Properties
            private RepositoryContext dataContext;
            //private readonly IDbSet<T> dbSet;

            protected IDbFactory DbFactory
            {
                get;
                set;
            }


            protected RepositoryContext DbContext
            {
                get
                {

                    return dataContext ??
                        (dataContext = DbFactory.Init());
                }
            }


            #endregion

            protected RepositoryBase(IDbFactory dbFactory)
            {
                DbFactory = dbFactory;


            }

        #region Implementation
        public virtual IQueryable<T> GetAll()
        {
            return DbContext.Set<T>();
        }
        public virtual IQueryable<T> All
        {
            get
            {
                return GetAll();
            }
        }
        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
        public virtual IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbContext.Set<T>().Where(predicate);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
        public async Task<T> GetSingle(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }
        public T GetById(int id)
        {
            return DbContext.Set<T>().Find(id);
        }
        public T GetById(Int64 id)
        {
            return DbContext.Set<T>().Find(id);
        }
        public T GetById(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbContext.Set<T>().Where(predicate);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.FirstOrDefault();
        }
        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbContext.Set<T>().Where(predicate);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task<T> GetSingle(Int64 id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }
        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where(predicate);
        }

        public virtual async Task Add(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
        }
        //public virtual void Edit(T entity)
        //{
        //    DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
        //    dbEntityEntry.State = EntityState.Modified;
        //}
        public virtual void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }

        public virtual void DeleteRange(IList<T> entityCollection)
        {
            DbContext.Set<T>().RemoveRange(entityCollection);
        }

        public async Task<List<TElement>> SQLQuery<TElement>(string sqlQuery, params object[] parameters) where TElement : class
        {

            var result = DbContext.Set<TElement>().FromSqlRaw(sqlQuery, parameters);
            return await result.ToListAsync();

        }



        #endregion

    }

}
