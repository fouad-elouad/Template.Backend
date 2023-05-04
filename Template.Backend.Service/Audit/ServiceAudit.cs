using Template.Backend.Model.Audit;
using Template.Backend.Data.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Template.Backend.Service.Audit
{
    public abstract class ServiceAudit<T> where T : class, IAuditEntity
    {
        private readonly IAuditRepository<T> _repository;

        public ServiceAudit(IAuditRepository<T> Repository)
        {
            _repository = Repository;
        }

        /// <summary>
        /// Get Audit List by principal entity id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAuditById(int id)
        {
            return _repository.GetMany(m => m.ID == id);
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

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _repository.GetMany(where);
        }

        /// <summary>
        /// Get a Snapshot of all records
        /// </summary>
        /// <param name="dateTime">The date time snapshot.</param>
        /// <returns> List of audit entities</returns>
        /// <exception cref="DateTimeFormatException">DateTime parameter format yyyyMMddThhmmss</exception>
        public virtual IEnumerable<T> GetAllSnapshot(DateTime dateTime)
        {
            Expression<Func<T, bool>> where = a => a.CreatedDate <= dateTime;
            Func<T, DateTime?> orderBy = a => a.CreatedDate;
            Func<T, int> groupBy = a => a.ID;
            Func<IGrouping<int, T>, T> selector = a => a.Last();
            Expression<Func<T, bool>> AuditOperationwhere = a => a.AuditOperation != AuditOperations.DELETE;

            return _repository.GetAllSnapshot(where, orderBy, groupBy, selector, AuditOperationwhere);
        }


        /// <summary>
        /// Gets a Snapshot for the spicified Id.
        /// </summary>
        /// <param name="dateTime">The date time snapshot.</param>
        /// <param name="id">The id.</param>
        /// <returns>audit Entity snapShot or null</returns>
        /// <exception cref="DateTimeFormatException">DateTime parameter format yyyyMMddThhmmss</exception>
        public virtual T GetByIdSnapshot(DateTime dateTime, int id)
        {
            Expression<Func<T, bool>> where = a => a.CreatedDate <= dateTime && a.ID == id;
            Func<T, DateTime?> orderBy = a => a.CreatedDate;
            Expression<Func<T, bool>> AuditOperationwhere = a => a.AuditOperation != AuditOperations.DELETE;

            T auditEntity = _repository.GetByIdSnapshot(where, orderBy);
            if (auditEntity == null || auditEntity.AuditOperation == AuditOperations.DELETE)
                return null;
            return auditEntity;
        }
    }
}