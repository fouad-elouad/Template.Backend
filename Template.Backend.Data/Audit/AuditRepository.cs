using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Template.Backend.Data.Audit
{
    /// <summary>
    /// AuditRepository class
    /// provides basic selection operations
    /// </summary>
    /// <typeparam name="T">class</typeparam>
    public class AuditRepository<T> : IAuditRepository<T> where T : class
    {

        protected StarterDbContext _dataContext;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbFactory"></param>
        public AuditRepository(StarterDbContext dbContext)
        {
            _dataContext = dbContext;
            _dbSet = _dataContext.Set<T>();
        }


        /// <summary>
        /// Finds an entity with the primary key values
        /// </summary>
        /// <param name="id">Id of entity to find</param>
        /// <returns>Founded entity</returns>
        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns>List of entities</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        /// <summary>
        /// Get entities based on where expression filter
        /// </summary>
        /// <param name="where">filter expression</param>
        /// <returns>List of entities</returns>
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).AsNoTracking().ToList();
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
            return _dbSet.Where(where).OrderBy(orderBy).GroupBy(groupBy).Select(selector)
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
            return _dbSet.Where(where).OrderByDescending(orderBy).AsQueryable()
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
            return _dbSet.Where(where).OrderByDescending(orderBy).AsQueryable()
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
            return _dbSet.Where(where).OrderBy(orderBy).GroupBy(groupBy).Select(selector)
                        .AsQueryable().Where(AuditOperationwhere).AsNoTracking().ToList();
        }
    }
}
