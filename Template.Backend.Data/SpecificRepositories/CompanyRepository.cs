using Microsoft.EntityFrameworkCore;
using Template.Backend.Data.Repositories;
using Template.Backend.Model.Entities;
using Template.Backend.Model.Enums;

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
        public CompanyRepository(StarterDbContext dbContext) : base(dbContext)
        {
        }

        public bool CheckIsUnique(string name, int Id)
        {
            return CheckIsUnique(s => s.Name == name && s.ID != Id);
        }

        public Company? FindById(int id, NestedObjectDepth nestedObjectDepth)
        {
            switch (nestedObjectDepth)
            {
                case NestedObjectDepth.FirstLevel:
                    return _dbSet.Include(c => c.Employees).FirstOrDefault(c => c.ID == id);
                case NestedObjectDepth.SecondLevel:
                    return _dbSet.Include(c => c.Employees).ThenInclude(e => e.Department).FirstOrDefault(c => c.ID == id);
                default:
                    return _dbSet.Find(id);
            }
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

        Company? FindById(int id, NestedObjectDepth nestedObjectDepth);

    }
}
