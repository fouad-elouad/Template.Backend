using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Template.Backend.Service.Audit
{
    /// <summary>
    /// IService interface
    /// </summary>
    /// <typeparam name="T">class</typeparam>
    public interface IServiceAudit<T> where T : class
    {

        IEnumerable<T> GetAuditById(int id);
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAllSnapshot(DateTime dateTime);
        T GetByIdSnapshot(DateTime dateTime, int id);
    }
}
