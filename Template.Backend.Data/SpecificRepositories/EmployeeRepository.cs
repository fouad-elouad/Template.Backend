using Microsoft.EntityFrameworkCore;
using Template.Backend.Data.Repositories;
using Template.Backend.Data.Utilities;
using Template.Backend.Model.Entities;
using Template.Backend.Model.Enums;

namespace Template.Backend.Data.SpecificRepositories
{
    /// <summary>
    /// Specific repository class for Employee entity
    /// </summary>
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbFactory"></param>
        public EmployeeRepository(StarterDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Searches employee.
        /// </summary>
        /// <param name="searchedEmployee">The searched employee.</param>
        /// <param name="startBirthDate">The startBirthDate.</param>
        /// <param name="endBirthDate">The endBirthDate.</param>
        /// <param name="pageNo">The page no.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>
        /// List of Employee
        /// </returns>
        public IEnumerable<Employee> Search(Employee searchedEmployee, DateTime? startBirthDate,
            DateTime? endBirthDate, int? pageNo = null, int? pageSize = null)
        {
            var result = _dbSet.AsQueryable();
            if (searchedEmployee != null)
            {
                if (!string.IsNullOrEmpty(searchedEmployee.Name))
                    result = result.Where(b => b.Name == searchedEmployee.Name);
                if (!string.IsNullOrEmpty(searchedEmployee.Address))
                    result = result.Where(b => b.Address == searchedEmployee.Address);
                if (!string.IsNullOrEmpty(searchedEmployee.Phone))
                    result = result.Where(b => b.Phone == searchedEmployee.Phone);
                
                if (searchedEmployee.CompanyID != null)
                    result = result.Where(b => b.CompanyID == searchedEmployee.CompanyID);
                if (searchedEmployee.DepartmentID != null)
                    result = result.Where(b => b.DepartmentID == searchedEmployee.DepartmentID);
            }

            if (startBirthDate != null)
                result = result.Where(b => b.BirthDate >= startBirthDate);
            if (endBirthDate != null)
                result = result.Where(b => b.BirthDate <= endBirthDate);

            if (pageNo != null && pageSize != null)
                return result.OrderByDescending(a => a.ID).ToPagedList(pageNo.Value, pageSize.Value);
            return result.ToList();
        }

        /// <summary>
        /// Count employee.
        /// </summary>
        /// <param name="searchedEmployee">The searched employee.</param>
        /// <param name="startBirthDate">The startBirthDate.</param>
        /// <param name="endBirthDate">The endBirthDate.</param>
        /// <returns>
        /// Count
        /// </returns>
        public int SearchCount(Employee searchedEmployee, DateTime? startBirthDate, DateTime? endBirthDate)
        {
            var result = _dbSet.AsQueryable();
            if (searchedEmployee != null)
            {
                if (!string.IsNullOrEmpty(searchedEmployee.Name))
                    result = result.Where(b => b.Name == searchedEmployee.Name);
                if (!string.IsNullOrEmpty(searchedEmployee.Address))
                    result = result.Where(b => b.Address == searchedEmployee.Address);
                if (!string.IsNullOrEmpty(searchedEmployee.Phone))
                    result = result.Where(b => b.Phone == searchedEmployee.Phone);

                if (searchedEmployee.CompanyID != null)
                    result = result.Where(b => b.CompanyID == searchedEmployee.CompanyID);
                if (searchedEmployee.DepartmentID != null)
                    result = result.Where(b => b.DepartmentID == searchedEmployee.DepartmentID);
            }

            if (startBirthDate != null)
                result = result.Where(b => b.BirthDate >= startBirthDate);
            if (endBirthDate != null)
                result = result.Where(b => b.BirthDate <= endBirthDate);

            return result.Count();
        }

        public bool CheckIsUnique(string name, int Id)
        {
            return CheckIsUnique(s => s.Name == name && s.ID != Id);
        }

        public Employee? FindById(int id, NestedObjectDepth nestedObjectDepth)
        {
            switch (nestedObjectDepth)
            {
                case NestedObjectDepth.FirstLevel:
                    return _dbSet.Include(c => c.Company).Include(c=>c.Department).FirstOrDefault(c => c.ID == id);
                case NestedObjectDepth.SecondLevel:
                    return _dbSet.Include(c => c.Company).ThenInclude(e=>e.Employees).Include(c => c.Department).ThenInclude(e=>e.Employees).FirstOrDefault(c => c.ID == id);
                default:
                    return _dbSet.Find(id);
            }
        }
    }

    /// <summary>
    /// Specific repository interface for Employee entity
    /// </summary>
    public interface IEmployeeRepository : IRepository<Employee>
    {
        /// <summary>
        /// Checks if is unique.
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        bool CheckIsUnique(string name, int Id);

        /// <summary>
        /// Searches employee.
        /// </summary>
        /// <param name="searchedEmployee">The searched employee.</param>
        /// <param name="startBirthDate">The startBirthDate.</param>
        /// <param name="endBirthDate">The endBirthDate.</param>
        /// <param name="pageNo">The page no.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>
        /// List of Employee
        /// </returns>
        IEnumerable<Employee> Search(Employee searchedEmployee, DateTime? startBirthDate,
            DateTime? endBirthDate, int? pageNo = null, int? pageSize = null);

        /// <summary>
        /// Count employee.
        /// </summary>
        /// <param name="searchedEmployee">The searched employee.</param>
        /// <param name="startBirthDate">The startBirthDate.</param>
        /// <param name="endBirthDate">The endBirthDate.</param>
        /// <returns>
        /// Count
        /// </returns>
        int SearchCount(Employee searchedEmployee, DateTime? startBirthDate,DateTime? endBirthDate);

        Employee? FindById(int id, NestedObjectDepth nestedObjectDepth);
    }
}
