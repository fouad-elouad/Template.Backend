using System.Linq.Expressions;

namespace Template.Backend.Data.Repositories
{
    /// <summary>
    /// IRepository interface
    /// </summary>
    /// <typeparam name="T">class</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Mark the specified entity as added.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(T entity);

        /// <summary>
        /// Mark List of entities as added.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Mark the specified entity as updated.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);

        /// <summary>
        /// Mark the specified entity as deleted.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);

        /// <summary>
        /// Mark as deleted based on where espression.
        /// </summary>
        /// <param name="where">The where clause.</param>
        void Delete(Expression<Func<T, bool>> where);

        /// <summary>
        /// Mark the specified entity Id as deleted.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(int id);

        /// <summary>
        /// Gets entity the by Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        T GetById(int id);


        /// <summary>
        /// Gets entity based on where espression.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns>Entity</returns>
        T Get(Expression<Func<T, bool>>? where);

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
        /// Returns a new query where the entities returned will not be cached in the System.Data.Entity.DbContext
        /// or System.Data.Entity.Core.Objects.ObjectContext. This method works by calling
        /// the AsNoTracking method of the underlying query object.
        /// </summary>
        /// <param name="where">The where clause.</param>
        /// <returns></returns>
        T? GetAsNoTraking(Expression<Func<T, bool>> where);

        /// <summary>
        /// Gets the paged list.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>List of entities</returns>
        IEnumerable<T> GetPagedList(int page, int pageSize);

        /// <summary>
        /// Checks if is unique based on where expression.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns>True or False</returns>
        bool CheckIsUnique(Expression<Func<T, bool>> where);

        /// <summary>
        /// Check if exist
        /// </summary>
        /// <param name="where">filter expression</param>
        /// <returns>true or false</returns>
        bool CheckIfExist(Expression<Func<T, bool>> where);

        /// <summary>
        /// Count records
        /// </summary>
        /// <param name="where">filter expression</param>
        /// <returns>
        /// The Count
        /// </returns>
        int Count(Expression<Func<T, bool>>? where = null);
    }
}
