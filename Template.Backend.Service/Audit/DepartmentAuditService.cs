using Template.Backend.Data.Audit;
using Template.Backend.Model.Audit.Entities;

namespace Template.Backend.Service.Audit
{
    public class DepartmentAuditService : ServiceAudit<DepartmentAudit>, IDepartmentAuditService
    {
        private readonly IAuditRepository<DepartmentAudit> _DepartmentAuditRepository;
        public DepartmentAuditService(IAuditRepository<DepartmentAudit> DepartmentAuditRepository)
            : base(DepartmentAuditRepository)
        {
            this._DepartmentAuditRepository = DepartmentAuditRepository;
        }
    }

    public interface IDepartmentAuditService : IServiceAudit<DepartmentAudit>
    {
    }
}