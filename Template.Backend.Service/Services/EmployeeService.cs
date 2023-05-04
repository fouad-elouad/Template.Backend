using Template.Backend.Data.Repositories;
using Template.Backend.Data.SpecificRepositories;
using Template.Backend.Model.Audit.Entities;
using Template.Backend.Model.Entities;
using Template.Backend.Service.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Template.Backend.Service.Services
{
    /// <summary>
    /// EmployeeService interface
    /// </summary>
    public interface IEmployeeService : IService<Employee>
    {
        /// <summary>
        /// Restores the specified Employee audit.
        /// </summary>
        /// <param name="employee">The Employee.</param>
        /// <param name="employeeAudit">The Employee audit.</param>
        void Restore(Employee employee, EmployeeAudit employeeAudit);

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
        /// <param name="employeeListToValidate">The employee list to validate.</param>
        /// <returns>
        /// Validation state
        /// </returns>
        bool HugeInsertValidation(IEnumerable<Employee> employeeListToValidate);

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
        int SearchCount(Employee searchedEmployee, DateTime? startBirthDate, DateTime? endBirthDate);

    }

    /// <summary>
    /// EmployeeService class
    /// </summary>
    /// <seealso cref="Template.Backend.Service.Service{Template.Backend.Model.Entities.Employee}" />
    /// <seealso cref="Template.Backend.Service.Services.IEmployeeService" />
    public class EmployeeService : Service<Employee>, IEmployeeService
    {
        private readonly IEmployeeRepository _EmployeeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeService"/> class.
        /// </summary>
        /// <param name="employeeRepository">The Employee repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="validatonDictionary">The validaton dictionary.</param>
        public EmployeeService(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork, IValidationDictionary validatonDictionary)
            : base(employeeRepository, unitOfWork, validatonDictionary)
        {
            _EmployeeRepository = employeeRepository;
        }

        /// <summary>
        /// Override Default Add for validation 
        /// Add the given entity to the context
        /// it will be inserted into the database when the Save is called
        /// </summary>
        /// <param name="employee">Entity to add</param>
        public override void Add(Employee employee)
        {
            if (this.ValidateEmployee(employee))
                base.Add(employee);
        }

        /// <summary>
        /// Override Default Update for validation 
        /// Update the given entity in the context
        /// it will be inserted into the database when the Save is called
        /// </summary>
        /// <param name="employee">Entity to Update</param>
        public override void Update(Employee employee)
        {
            if (this.ValidateEmployee(employee))
                base.Update(employee);
        }

        /// <summary>
        /// Checks the is unique.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public bool CheckIsUnique(string name, int Id)
        {
            return _EmployeeRepository.CheckIsUnique(name, Id);
        }

        /// <summary>
        /// Logic and database Validation
        /// </summary>
        /// <param name="employeeToValidate"></param>
        /// <returns>Validation state</returns>
        protected bool ValidateEmployee(Employee employeeToValidate)
        {
            // Database validation
            if (string.IsNullOrWhiteSpace(employeeToValidate.Name))
            {
                GetValidationDictionary().AddError(nameof(employeeToValidate.Name), "Name is required");
            }

            if (employeeToValidate.BirthDate == null)
            {
                GetValidationDictionary().AddError(nameof(employeeToValidate.BirthDate), "BirthDate is required");
            }

            if (employeeToValidate.Address == null)
            {
                GetValidationDictionary().AddError(nameof(employeeToValidate.Address), "Address is required");
            }

            if (employeeToValidate.CompanyID == null)
            {
                GetValidationDictionary().AddError(nameof(employeeToValidate.CompanyID), "CompanyID is required");
            }

            // unique
            if (!_EmployeeRepository.CheckIsUnique(employeeToValidate.Name, employeeToValidate.ID) && !string.IsNullOrWhiteSpace(employeeToValidate.Name))
            {
                GetValidationDictionary().AddError(nameof(employeeToValidate.Name), "the (" + employeeToValidate.Name + ") already exist");
            }

            return GetValidationDictionary().IsValid();
        }

        /// <summary>
        /// Logic and database Validation used for huge insert
        /// Add Error to Validation Dictionnary like "Le Nom est obligatoire lignes 5,10,45,200"
        /// </summary>
        /// <param name="employeeListToValidate">The employee list to validate.</param>
        /// <returns>
        /// Validation state
        /// </returns>
        public bool HugeInsertValidation(IEnumerable<Employee> employeeListToValidate)
        {
            int line = 1;
            bool isRequeredNameMessage = false;
            string requeredNameMessage = "Name is required lignes ";
            bool isRequeredBirthDateMessage = false;
            string requeredBirthDateMessage = "BirthDate is required lignes ";
            bool isRequeredAddressMessage = false;
            string requeredAddressMessage = "Address is required lignes ";
            bool isRequeredCompanyIDMessage = false;
            string requeredCompanyIDMessage = "CompanyID is required lignes ";
            bool isDuplicatedNameMessage = false;
            string duplicatedNameMessage = "Name already exist lignes ";
            foreach (var employee in employeeListToValidate)
            {
                // Database validation
                if (string.IsNullOrWhiteSpace(employee.Name))
                {
                    isRequeredNameMessage = true;
                    requeredNameMessage += line + ",";
                }

                if (employee.BirthDate == null)
                {
                    isRequeredBirthDateMessage = true;
                    requeredBirthDateMessage += line + ",";
                }

                if (employee.Address == null)
                {
                    isRequeredAddressMessage = true;
                    requeredAddressMessage += line + ",";
                }

                if (employee.CompanyID == null)
                {
                    isRequeredCompanyIDMessage = true;
                    requeredCompanyIDMessage += line + ",";
                }

                // unique
                if (!_EmployeeRepository.CheckIsUnique(employee.Name, employee.ID) && !string.IsNullOrWhiteSpace(employee.Name))
                {
                    isDuplicatedNameMessage = true;
                    duplicatedNameMessage += line + ",";
                }

                // unique
                if (employeeListToValidate.Count(c => c.Name == employee.Name) > 1)
                {
                    isDuplicatedNameMessage = true;
                    duplicatedNameMessage += line + ",";
                }
                line++;
            }

            if (isRequeredNameMessage)
                GetValidationDictionary().AddError(nameof(Employee.Name), requeredNameMessage);
            if (isRequeredBirthDateMessage)
                GetValidationDictionary().AddError(nameof(Employee.BirthDate), requeredBirthDateMessage);
            if (isRequeredAddressMessage)
                GetValidationDictionary().AddError(nameof(Employee.Address), requeredAddressMessage);
            if (isRequeredCompanyIDMessage)
                GetValidationDictionary().AddError(nameof(Employee.CompanyID), requeredCompanyIDMessage);
            if (isDuplicatedNameMessage)
                GetValidationDictionary().AddError(nameof(Employee.Name), duplicatedNameMessage);

            return GetValidationDictionary().IsValid();
        }

        public void Restore(Employee Employee, EmployeeAudit EmployeeAudit)
        {
            Employee.Name = EmployeeAudit.Name;
            Employee.Address = EmployeeAudit.Address;
            Employee.BirthDate = EmployeeAudit.BirthDate;
            Employee.CompanyID = EmployeeAudit.CompanyID;
            Employee.DepartmentID = EmployeeAudit.DepartmentID;
            Employee.Phone = EmployeeAudit.Phone;
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
            return _EmployeeRepository.Search(searchedEmployee, startBirthDate, endBirthDate, pageNo, pageSize);
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
            return _EmployeeRepository.SearchCount(searchedEmployee, startBirthDate, endBirthDate);
        }
    }
}