using Microsoft.EntityFrameworkCore;
using Template.Backend.Data.Repositories;
using Template.Backend.Model.Entities;
using Template.Backend.Model.Enums;

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
        public DepartmentRepository(StarterDbContext dbContext) : base(dbContext)
        {
        }

        public bool CheckIsUnique(string name, int Id)
        {
            return CheckIsUnique(s => s.Name == name && s.ID != Id);
        }

        public Department? FindById(int id, NestedObjectDepth nestedObjectDepth)
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

        Department? FindById(int id, NestedObjectDepth nestedObjectDepth);
    }
}
