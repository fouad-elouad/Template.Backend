using Template.Backend.Data.Audit;
using Template.Backend.Model.Audit.Entities;

namespace Template.Backend.Service.Audit
{
    public class EmployeeAuditService : ServiceAudit<EmployeeAudit>, IEmployeeAuditService
    {
        private readonly IAuditRepository<EmployeeAudit> _EmployeeAuditRepository;
        public EmployeeAuditService(IAuditRepository<EmployeeAudit> EmployeeAuditRepository)
            : base(EmployeeAuditRepository)
        {
            this._EmployeeAuditRepository = EmployeeAuditRepository;
        }

    }

    public interface IEmployeeAuditService : IServiceAudit<EmployeeAudit>
    {
    }
}