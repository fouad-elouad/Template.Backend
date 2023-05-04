using Template.Backend.Data.Repositories;
using Template.Backend.Model.Entities;

namespace Template.Backend.Data.SpecificRepositories
{
    /// <summary>
    /// Specific repository class for Company entity
    /// </summary>
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbFactory"></param>
        public CompanyRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool CheckIsUnique(string name, int Id)
        {
            return CheckIsUnique(s => s.Name == name && s.ID != Id);
        }
    }

    /// <summary>
    /// Specific repository interface for Company entity
    /// </summary>
    public interface ICompanyRepository : IRepository<Company>
    {
        /// <summary>
        /// Checks if is unique.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        bool CheckIsUnique(string name, int Id);
    }
}
