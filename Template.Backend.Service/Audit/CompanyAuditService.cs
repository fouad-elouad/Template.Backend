using Template.Backend.Data.Audit;
using Template.Backend.Model.Audit.Entities;

namespace Template.Backend.Service.Audit
{
    public class CompanyAuditService : ServiceAudit<CompanyAudit>, ICompanyAuditService
    {
        private readonly IAuditRepository<CompanyAudit> _CompanyAuditRepository;
        public CompanyAuditService(IAuditRepository<CompanyAudit> CompanyAuditRepository)
            : base(CompanyAuditRepository)
        {
            this._CompanyAuditRepository = CompanyAuditRepository;
        }

    }

    public interface ICompanyAuditService : IServiceAudit<CompanyAudit>
    {
    }
}