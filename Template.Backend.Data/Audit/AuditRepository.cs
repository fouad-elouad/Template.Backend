using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using Template.Backend.Data.Repositories;

namespace Template.Backend.Data.Audit
{
    /// <summary>
    /// AuditRepository class
    /// provides basic selection operations
    /// </summary>
    /// <typeparam name="T">class</typeparam>
    public class AuditRepository<T> : IAuditRepository<T> where T : class
    {
        private IDbContext dataContext;
        private readonly IDbSet<T> dbSet;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        /// <summary>
        /// Return current context if not instantiate new one
        /// </summary>
        protected IDbContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbFactory"></param>
        public AuditRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }


        /// <summary>
        /// Finds an entity with the primary key values
        /// </summary>
        /// <param name="id">Id of entity to find</param>
        /// <returns>Founded entity</returns>
        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns>List of entities</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.AsNoTracking().ToList();
        }

        /// <summary>
        /// Get entities based on where expression filter
        /// </summary>
        /// <param name="where">filter expression</param>
        /// <returns>List of entities</returns>
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).AsNoTracking().ToList();
        }

        /// <summary>
        /// Gets all objects snapshot (AsNoTracking).
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="groupBy">The group by.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="AuditOperationwhere">The audit operation where.</param>
        /// <returns>List of objects snapshot</returns>
        public virtual IEnumerable<T> GetAllSnapshot(Expression<Func<T, bool>> where, Func<T, DateTime?> orderBy,
                        Func<T, int> groupBy, Func<IGrouping<int, T>, T> selector,
                        Expression<Func<T, bool>> AuditOperationwhere)
        {
            return dbSet.Where(where).OrderBy(orderBy).GroupBy(groupBy).Select(selector)
                        .AsQueryable().Where(AuditOperationwhere).AsNoTracking().ToList();
        }

        /// <summary>
        /// Gets object snapshot (AsNoTracking).
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>object snapshot</returns>
        public virtual T GetByIdSnapshot(Expression<Func<T, bool>> where, Func<T, DateTime?> orderBy)
        {
            return dbSet.Where(where).OrderByDescending(orderBy).AsQueryable()
                        .AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Gets First Or Default object that satisfy conditions (AsNoTracking,).
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>object</returns>
        public virtual T Get(Expression<Func<T, bool>> where, Func<T, DateTime> orderBy)
        {
            return dbSet.Where(where).OrderByDescending(orderBy).AsQueryable()
                        .AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Gets all objects that satisfy conditions (AsNoTracking).
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="groupBy">The group by.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="AuditOperationwhere">The audit operation where.</param>
        /// <returns>List of objects</returns>
        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> where, Func<T, DateTime> orderBy,
                        Func<T, int> groupBy, Func<IGrouping<int, T>, T> selector,
                        Expression<Func<T, bool>> AuditOperationwhere)
        {
            return dbSet.Where(where).OrderBy(orderBy).GroupBy(groupBy).Select(selector)
                        .AsQueryable().Where(AuditOperationwhere).AsNoTracking().ToList();
        }
    }
}
