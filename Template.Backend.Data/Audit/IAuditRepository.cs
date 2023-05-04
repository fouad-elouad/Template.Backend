using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Template.Backend.Data.Audit
{
    /// <summary>
    /// IAuditRepository interface
    /// </summary>
    /// <typeparam name="T">class</typeparam>
    public interface IAuditRepository<T> where T : class
    {
        /// <summary>
        /// Gets entity the by Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        T GetById(int id);

        /// <summary>
        /// Gets all Entites.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets List of entities based on where espression.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns>List of entities</returns>
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

        /// <summary>
        /// Gets all objects that satisfy conditions (AsNoTracking).
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="groupBy">The group by.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="AuditOperationwhere">The audit operation where.</param>
        /// <returns>List of objects</returns>
        IEnumerable<T> GetAll(Expression<Func<T, bool>> where, Func<T, DateTime> orderBy,
                        Func<T, int> groupBy, Func<IGrouping<int, T>, T> selector,
                        Expression<Func<T, bool>> AuditOperationwhere);

        /// <summary>
        /// Gets First Or Default object that satisfy conditions (AsNoTracking,).
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>object</returns>
        T Get(Expression<Func<T, bool>> where, Func<T, DateTime> orderBy);

        /// <summary>
        /// Gets all objects snapshot (AsNoTracking).
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="groupBy">The group by.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="AuditOperationwhere">The audit operation where.</param>
        /// <returns>List of objects snapshot</returns>
        IEnumerable<T> GetAllSnapshot(Expression<Func<T, bool>> where, Func<T, DateTime?> orderBy,
                        Func<T, int> groupBy, Func<IGrouping<int, T>, T> selector,
                        Expression<Func<T, bool>> AuditOperationwhere);

        /// <summary>
        /// Gets object snapshot (AsNoTracking).
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>object snapshot</returns>
        T GetByIdSnapshot(Expression<Func<T, bool>> where, Func<T, DateTime?> orderBy);
    }
}
