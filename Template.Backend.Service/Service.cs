using Template.Backend.Data.Repositories;
using Template.Backend.Service.Validation;
using System.Linq.Expressions;

namespace Template.Backend.Service
{
    public abstract class Service<T> where T : class
    {
        private readonly IRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationDictionary _validationDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="Service{T}"/> class.
        /// </summary>
        /// <param name="Repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="validationDictionary">The validaton dictionary.</param>
        public Service(IRepository<T> Repository, IUnitOfWork unitOfWork, IValidationDictionary validationDictionary)
        {
            _repository = Repository;
            _unitOfWork = unitOfWork;
            _validationDictionary = validationDictionary;
        }

        /// <summary>
        /// Add the given entity to the context
        /// it will be inserted into the database when the Save is called
        /// </summary>
        /// <param name="entity">Entity to add</param>
        public virtual void Add(T entity)
        {
            _repository.Add(entity);

        }

        /// <summary>
        /// Add the given list of entities to the context
        /// it will be inserted into the database when the Save is called
        /// </summary>
        /// <param name="entities">entites to add</param>
        public virtual void AddRange(IEnumerable<T> entities)
        {
            _repository.AddRange(entities);
        }

        /// <summary>
        /// Mark the existing entity as modified
        /// it will be updated in database when the Save is called
        /// </summary>
        /// <param name="entity">Updated entity</param>
        public virtual void Update(T entity)
        {
            _repository.Update(entity);
        }

        /// <summary>
        /// Mark the given entity as deleted
        /// it will be deleted from database when the Save is called
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        public virtual void Delete(T entity)
        {
            _repository.Delete(entity);
        }

        /// <summary>
        /// Mark the given entities based on where expression filter as deleted
        /// it will be deleted from database when the Save is called
        /// </summary>
        /// <param name="where">filter expression</param>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            _repository.Delete(where);
        }

        /// <summary>
        /// Mark the given entity id as deleted
        /// it will be deleted from database when the Save is called
        /// </summary>
        /// <param name="id">Id of entity to delete</param>
        public virtual void Delete(int id)
        {
            _repository.Delete(id);
        }

        /// <summary>
        /// Finds an entity with the primary key values
        /// </summary>
        /// <param name="id">Id of entity to find</param>
        /// <returns>Founded entity</returns>
        public virtual T GetById(int id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns>List of entities</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Get entities based on where expression filter
        /// </summary>
        /// <param name="where">filter expression</param>
        /// <returns>List of entities</returns>
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _repository.GetMany(where);
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
            return _repository.Get(where);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IValidationDictionary GetValidationDictionary()
        {
            return _validationDictionary;
        }

        public IEnumerable<T> GetPagedList(int page, int pageSize)
        {
            return _repository.GetPagedList(page, pageSize);
        }

        public bool CheckIfExist(Expression<Func<T, bool>> where)
        {
            return _repository.CheckIfExist(where);
        }

        public int Count(Expression<Func<T, bool>> where = null)
        {
            return _repository.Count(where);
        }
    }
}