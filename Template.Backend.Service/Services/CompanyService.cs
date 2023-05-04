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
    /// CompanyService interface
    /// </summary>
    public interface ICompanyService : IService<Company>
    {
        /// <summary>
        /// Restores the specified Company audit.
        /// </summary>
        /// <param name="company">The Company.</param>
        /// <param name="companyAudit">The Company audit.</param>
        void Restore(Company company, CompanyAudit companyAudit);

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
        /// <param name="companyListToValidate">The company list to validate.</param>
        /// <returns>
        /// Validation state
        /// </returns>
        bool HugeInsertValidation(IEnumerable<Company> companyListToValidate);
    }

    /// <summary>
    /// CompanyService class
    /// </summary>
    /// <seealso cref="Template.Backend.Service.Service{Template.Backend.Model.Entities.Company}" />
    /// <seealso cref="Template.Backend.Service.Services.ICompanyService" />
    public class CompanyService : Service<Company>, ICompanyService
    {
        private readonly ICompanyRepository _CompanyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyService"/> class.
        /// </summary>
        /// <param name="companyRepository">The Company repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="validatonDictionary">The validaton dictionary.</param>
        public CompanyService(ICompanyRepository companyRepository, IUnitOfWork unitOfWork, IValidationDictionary validatonDictionary)
            : base(companyRepository, unitOfWork, validatonDictionary)
        {
            _CompanyRepository = companyRepository;
        }

        /// <summary>
        /// Override Default Add for validation 
        /// Add the given entity to the context
        /// it will be inserted into the database when the Save is called
        /// </summary>
        /// <param name="Company">Entity to add</param>
        public override void Add(Company Company)
        {
            if (this.ValidateCompany(Company))
                base.Add(Company);
        }

        /// <summary>
        /// Override Default Update for validation 
        /// Update the given entity in the context
        /// it will be inserted into the database when the Save is called
        /// </summary>
        /// <param name="entity">Entity to Update</param>
        public override void Update(Company Company)
        {
            if (this.ValidateCompany(Company))
                base.Update(Company);
        }

        /// <summary>
        /// Checks the is unique.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public bool CheckIsUnique(string name, int Id)
        {
            return _CompanyRepository.CheckIsUnique(name, Id);
        }

        /// <summary>
        /// Logic and database Validation
        /// </summary>
        /// <param name="companyToValidate"></param>
        /// <returns>Validation state</returns>
        protected bool ValidateCompany(Company companyToValidate)
        {
            // Database validation
            if (string.IsNullOrWhiteSpace(companyToValidate.Name))
            {
                GetValidationDictionary().AddError(nameof(companyToValidate.Name), "Name is required");
            }

            if (companyToValidate.CreationDate == null)
            {
                GetValidationDictionary().AddError(nameof(companyToValidate.CreationDate), "CreationDate Name is required");
            }

            // unique
            if (!_CompanyRepository.CheckIsUnique(companyToValidate.Name, companyToValidate.ID) && !string.IsNullOrWhiteSpace(companyToValidate.Name))
            {
                GetValidationDictionary().AddError(nameof(companyToValidate.Name), "the (" + companyToValidate.Name + ") already exist");
            }

            return GetValidationDictionary().IsValid();
        }

        /// <summary>
        /// Logic and database Validation used for huge insert
        /// Add Error to Validation Dictionnary like "Le Nom est obligatoire lignes 5,10,45,200"
        /// </summary>
        /// <param name="companyListToValidate">The company list to validate.</param>
        /// <returns>
        /// Validation state
        /// </returns>
        public bool HugeInsertValidation(IEnumerable<Company> companyListToValidate)
        {
            int line = 1;
            bool isRequeredNameMessage = false;
            string requeredNameMessage = "Name is required lignes ";
            bool isRequeredCreationDateMessage = false;
            string requeredCreationDateMessage = "CreationDate Name is required lignes ";
            bool isDuplicatedNameMessage = false;
            string duplicatedNameMessage = "Name already exist lignes ";
            foreach (var company in companyListToValidate)
            {
                // Database validation
                if (string.IsNullOrWhiteSpace(company.Name))
                {
                    isRequeredNameMessage = true;
                    requeredNameMessage += line + ",";
                }

                // Database validation
                if (company.CreationDate == null)
                {
                    isRequeredCreationDateMessage = true;
                    requeredCreationDateMessage += line + ",";
                }

                // unique
                if (!_CompanyRepository.CheckIsUnique(company.Name, company.ID) && !string.IsNullOrWhiteSpace(company.Name))
                {
                    isDuplicatedNameMessage = true;
                    duplicatedNameMessage += line + ",";
                }

                // unique
                if (companyListToValidate.Count(c => c.Name == company.Name) > 1)
                {
                    isDuplicatedNameMessage = true;
                    duplicatedNameMessage += line + ",";
                }
                line++;
            }

            if (isRequeredNameMessage)
                GetValidationDictionary().AddError(nameof(Company.Name), requeredNameMessage);
            if (isRequeredCreationDateMessage)
                GetValidationDictionary().AddError(nameof(Company.CreationDate), requeredCreationDateMessage);
            if (isDuplicatedNameMessage)
                GetValidationDictionary().AddError(nameof(Company.Name), duplicatedNameMessage);

            return GetValidationDictionary().IsValid();
        }

        public void Restore(Company Company, CompanyAudit CompanyAudit)
        {
            Company.Name = CompanyAudit.Name;
            Company.CreationDate = CompanyAudit.CreationDate;
        }
    }
}