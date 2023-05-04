using Template.Backend.Data.Helpers;
using Template.Backend.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Template.Backend.Model;

namespace Template.Backend.Data.Repositories
{
    /// <summary>
    /// RepositoryBase class
    /// Abstract repository implemented by specific repositories
    /// provides basic CRUD operations
    /// </summary>
    /// <typeparam name="T">class</typeparam>
    public abstract class RepositoryBase<T> where T : class
    {
        private IDbContext _dataContext;
        protected readonly IDbSet<T> _dbSet;
        private const int _defaultLimit = 10000;

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
            get { return _dataContext ?? (_dataContext = DbFactory.Init()); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbFactory"></param>
        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            _dbSet = DbContext.Set<T>();
        }

        /// <summary>
        /// Add the given entity to the context
        /// it will be inserted into the database when the SaveChanges is called
        /// </summary>
        /// <param name="entity">Entity to add</param>
        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// Add the given list of entities to the context
        /// it will be inserted into the database when the SaveChanges is called
        /// </summary>
        /// <param name="entities">entites to add</param>
        public virtual void AddRange(IEnumerable<T> entities)
        {
            // can't call AddRange on dbSet interface
            var DbSet = (DbSet<T>)_dbSet;
            DbSet.AddRange(entities);
        }

        /// <summary>
        /// Mark the existing entity as modified
        /// it will be updated in database when the SaveChanges is called
        /// </summary>
        /// <param name="entity">Updated entity</param>
        public virtual void Update(T entity)
        {
            if (_dataContext.IsDetached(entity))
                _dbSet.Attach(entity);
            _dataContext.SetModified(entity);
        }

        /// <summary>
        /// Mark the given entity as deleted
        /// it will be deleted from database when the SaveChanges is called
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Mark the given entities based on where expression filter as deleted
        /// it will be deleted from database when the SaveChanges is called
        /// </summary>
        /// <param name="where">filter expression</param>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbSet.Where<T>(where).AsEnumerable();
            // can't call AddRange on dbSet interface
            var DbSet = (DbSet<T>)_dbSet;
            DbSet.RemoveRange(objects);
        }

        /// <summary>
        /// Mark the given entity id as deleted
        /// it will be deleted from database when the SaveChanges is called
        /// </summary>
        /// <param name="id">Id of entity to delete</param>
        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
                Delete(entity);
            else
            {
                throw new IdNotFoundException("No Element found for this id");
            }
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
        /// Finds an entity with the primary key values
        /// </summary>
        /// <param name="id">Id of entity to find</param>
        /// <returns>Founded entity</returns>
        public virtual T GetById(string id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns>List of entities</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.Take(_defaultLimit).ToList();
        }

        /// <summary>
        /// Count records
        /// </summary>
        /// <param name="where">filter expression</param>
        /// <returns>
        /// The Count
        /// </returns>
        public virtual int Count(Expression<Func<T, bool>> where = null)
        {
            if (where == null)
                return _dbSet.Count();
            return _dbSet.Count(where);
        }

        /// <summary>
        /// Get entities based on where expression filter
        /// </summary>
        /// <param name="where">filter expression</param>
        /// <returns>List of entities</returns>
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).Take(_defaultLimit).ToList();
        }

        /// <summary>
        /// Get entitity based on where expression filter
        /// Returns the first element of the sequence, 
        /// or a default value if the sequence contains no element
        /// </summary>
        /// <param name="where">filter expression</param>
        /// <returns>First or default entity</returns>
        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).FirstOrDefault<T>();
        }

        /// <summary>
        /// Get entitity based on where expression filter with as no tracking
        /// Returns the first element of the sequence, 
        /// or a default value if the sequence contains no element
        /// </summary>
        /// <param name="where">filter expression</param>
        /// <returns>First or default entity</returns>
        public T GetAsNoTraking(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).AsNoTracking().FirstOrDefault<T>();
        }

        /// <summary>
        /// Check Unique
        /// </summary>
        /// <param name="where">filter expression</param>
        /// <returns>true or false</returns>
        public bool CheckIsUnique(Expression<Func<T, bool>> where)
        {
            return !_dbSet.Any(where);
        }

        private static Expression<Func<T, int>> PropertyOrderBy(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var memberExpression = Expression.Property(Expression.Convert(parameter, typeof(T)), propertyName);
            var orderBy = Expression.Lambda<Func<T, int>>(memberExpression, parameter);
            return orderBy;
        }

        public IEnumerable<T> GetPagedList(int page, int pageSize)
        {
            if (pageSize > _defaultLimit)
                pageSize = _defaultLimit;
            return _dbSet.OrderByDescending(PropertyOrderBy(nameof(IEntity.ID))).ToPagedList(page, pageSize);
        }

        public bool CheckIfExist(Expression<Func<T, bool>> where)
        {
            return _dbSet.Any(where);
        }
    }
}
