using Template.Backend.Data.Repositories;
using Template.Backend.Model.Entities;

namespace Template.Backend.Data.SpecificRepositories
{
    /// <summary>
    /// Specific repository class for Department entity
    /// </summary>
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbFactory"></param>
        public DepartmentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool CheckIsUnique(string name, int Id)
        {
            return CheckIsUnique(s => s.Name == name && s.ID != Id);
        }
    }

    /// <summary>
    /// Specific repository interface for Department entity
    /// </summary>
    public interface IDepartmentRepository : IRepository<Department>
    {
        /// <summary>
        /// Checks if is unique.
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        bool CheckIsUnique(string name, int Id);
    }
}
