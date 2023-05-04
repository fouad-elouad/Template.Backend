using Template.Backend.Data.Repositories;
using Template.Backend.Data.SpecificRepositories;
using Template.Backend.Model.Audit.Entities;
using Template.Backend.Model.Entities;
using Template.Backend.Service.Validation;
using System.Collections.Generic;
using System.Linq;

namespace Template.Backend.Service.Services
{
    /// <summary>
    /// DepartmentService interface
    /// </summary>
    public interface IDepartmentService : IService<Department>
    {
        /// <summary>
        /// Restores the specified Department audit.
        /// </summary>
        /// <param name="department">The Department.</param>
        /// <param name="departmentAudit">The Department audit.</param>
        void Restore(Department department, DepartmentAudit departmentAudit);

        /// <summary>
        /// Checks the is unique.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        bool CheckIsUnique(string name, int Id);

        /// <summary>
        /// Logic and database Validation
        /// </summary>
        /// <param name="departmentListToValidate">The department list to validate.</param>
        /// <returns>
        /// Validation state
        /// </returns>
        bool HugeInsertValidation(IEnumerable<Department> departmentListToValidate);
    }

    /// <summary>
    /// DepartmentService class
    /// </summary>
    /// <seealso cref="Template.Backend.Service.Service{Template.Backend.Model.Entities.Department}" />
    /// <seealso cref="Template.Backend.Service.Services.IDepartmentService" />
    public class DepartmentService : Service<Department>, IDepartmentService
    {
        private readonly IDepartmentRepository _DepartmentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentService"/> class.
        /// </summary>
        /// <param name="departmentRepository">The Department repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="validatonDictionary">The validaton dictionary.</param>
        public DepartmentService(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork, IValidationDictionary validatonDictionary)
            : base(departmentRepository, unitOfWork, validatonDictionary)
        {
            _DepartmentRepository = departmentRepository;
        }

        /// <summary>
        /// Override Default Add for validation 
        /// Add the given entity to the context
        /// it will be inserted into the database when the Save is called
        /// </summary>
        /// <param name="Department">Entity to add</param>
        public override void Add(Department Department)
        {
            if (this.ValidateDepartment(Department))
                base.Add(Department);
        }

        /// <summary>
        /// Override Default Update for validation 
        /// Update the given entity in the context
        /// it will be inserted into the database when the Save is called
        /// </summary>
        /// <param name="entity">Entity to Update</param>
        public override void Update(Department Department)
        {
            if (this.ValidateDepartment(Department))
                base.Update(Department);
        }

        /// <summary>
        /// Checks the is unique.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public bool CheckIsUnique(string name, int Id)
        {
            return _DepartmentRepository.CheckIsUnique(name, Id);
        }

        /// <summary>
        /// Logic and database Validation
        /// </summary>
        /// <param name="departmentToValidate"></param>
        /// <returns>Validation state</returns>
        protected bool ValidateDepartment(Department departmentToValidate)
        {
            // Database validation
            if (string.IsNullOrWhiteSpace(departmentToValidate.Name))
            {
                GetValidationDictionary().AddError(nameof(departmentToValidate.Name), "Name is required");
            }

            // unique
            if (!_DepartmentRepository.CheckIsUnique(departmentToValidate.Name, departmentToValidate.ID) && !string.IsNullOrWhiteSpace(departmentToValidate.Name))
            {
                GetValidationDictionary().AddError(nameof(departmentToValidate.Name), "the (" + departmentToValidate.Name + ") already exist");
            }

            return GetValidationDictionary().IsValid();
        }

        /// <summary>
        /// Logic and database Validation used for huge insert
        /// Add Error to Validation Dictionnary like "Le Nom est obligatoire lignes 5,10,45,200"
        /// </summary>
        /// <param name="departmentListToValidate">The department list to validate.</param>
        /// <returns>
        /// Validation state
        /// </returns>
        public bool HugeInsertValidation(IEnumerable<Department> departmentListToValidate)
        {
            int line = 1;
            bool isRequeredNameMessage = false;
            string requeredNameMessage = "Name is required lignes ";
            bool isDuplicatedNameMessage = false;
            string duplicatedNameMessage = "Name already exist lignes ";
            foreach (var department in departmentListToValidate)
            {
                // Database validation
                if (string.IsNullOrWhiteSpace(department.Name))
                {
                    isRequeredNameMessage = true;
                    requeredNameMessage += line + ",";
                }

                // unique
                if (!_DepartmentRepository.CheckIsUnique(department.Name, department.ID) && !string.IsNullOrWhiteSpace(department.Name))
                {
                    isDuplicatedNameMessage = true;
                    duplicatedNameMessage += line + ",";
                }

                // unique
                if (departmentListToValidate.Count(c => c.Name == department.Name) > 1)
                {
                    isDuplicatedNameMessage = true;
                    duplicatedNameMessage += line + ",";
                }
                line++;
            }

            if (isRequeredNameMessage)
                GetValidationDictionary().AddError(nameof(Department.Name), requeredNameMessage);
            if (isDuplicatedNameMessage)
                GetValidationDictionary().AddError(nameof(Department.Name), duplicatedNameMessage);

            return GetValidationDictionary().IsValid();
        }
        public void Restore(Department Department, DepartmentAudit DepartmentAudit)
        {
            Department.Name = DepartmentAudit.Name;
        }
    }
}