using Microsoft.VisualStudio.TestTools.UnitTesting;
using Template.Backend.Data.Repositories;
using Template.Backend.Data.SpecificRepositories;
using Template.Backend.Model.Entities;
using Template.Backend.Model.Exceptions;
using Template.Backend.Service.Services;
using Template.Backend.Service.Validation;
using System.Collections.Generic;
using System.Linq;

namespace Template.Backend.IntegrationTest
{
    /// <summary>
    /// Test CompanyService
    /// </summary>
    [TestClass]
    public class TestCompanyService
    {
        static bool IsDatabaseInitialized = false;
        static int seedCount = 0;

        IDbFactory dbFactory;
        ICompanyRepository companyRepository;
        IUnitOfWork unitOfWork;
        IValidationDictionary validationDictionary;
        ICompanyService companyService;

        /// <summary>
        /// Initialize 
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new DbFactory();
            companyRepository = new CompanyRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            validationDictionary = new ValidationDictionary();
            companyService = new CompanyService(companyRepository, unitOfWork, validationDictionary);

            if (!IsDatabaseInitialized)
            {
                Helper.InitializeLocalDatabase(dbFactory);
                IsDatabaseInitialized = true;
                Seed(3);
            }

            seedCount = companyService.Count();
        }

        private void Seed(int number)
        {
            for (int i = 0; i < number; i++)
            {
                Company company = Helper.CompanyFactory("TEST"+i);
                companyService.Add(company);
                companyService.Save(Helper.UserName);
            }
        }

        /// <summary>
        /// Test Company Count
        /// </summary>
        [TestMethod]
        public void Test_Company_Count()
        {
            Assert.AreEqual(seedCount, companyService.Count());
        }

        /// <summary>
        /// Test Company Find
        /// </summary>
        [TestMethod]
        public void Test_Company_Find()
        {
            Company company;
            company = companyService.GetById(1);
            Assert.AreEqual(1, company.ID);

            company = companyService.GetById(0);
            Assert.IsNull(company);
        }

        /// <summary>
        /// Test Company Add and delete
        /// </summary>
        [TestMethod]
        public void Test_Company_Add_Delete()
        {
            Company company;
            Assert.AreEqual(seedCount, companyService.Count());

            company = Helper.CompanyFactory("TEST");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            Assert.AreEqual(seedCount +1, companyService.Count());

            companyService.Delete(company.ID);
            companyService.Save(Helper.UserName);

            Assert.AreEqual(seedCount, companyService.Count());
        }

        /// <summary>
        /// Test Company Add duplicate Name
        /// </summary>
        [TestMethod]
        public void Test_Company_Add_Duplicate_Name()
        {
            Company company;
            Assert.AreEqual(seedCount, companyService.Count());

            company = Helper.CompanyFactory("TEST");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            Assert.AreEqual(seedCount +1, companyService.Count());

            Company company2 = Helper.CompanyFactory("TEST");
            companyService.Add(company2);
            companyService.Save(Helper.UserName);

            Assert.IsFalse(companyService.GetValidationDictionary().IsValid());
            Assert.AreEqual(seedCount +1, companyService.Count());

            companyService.Delete(company.ID);
            companyService.Save(Helper.UserName);

            Assert.AreEqual(seedCount, companyService.Count());
        }

        /// <summary>
        /// Test Company Update
        /// </summary>
        [TestMethod]
        public void Test_Company_Update()
        {
            Company company;

            company = companyService.GetById(1);
            string previousName = company.Name;
            string newName = "New Name";

            Assert.AreNotEqual(newName, company.Name);

            company.Name = newName;
            companyService.Update(company);
            companyService.Save(Helper.UserName);

            Assert.AreEqual(newName, company.Name);

            company.Name = previousName;
            companyService.Save(Helper.UserName);
        }

        /// <summary>
        /// Test Company get
        /// </summary>
        [TestMethod]
        public void Test_Company_Get()
        {
            Company company;
            company = companyService.Get(b => b.ID == 3);
            Assert.AreEqual(3, company.ID);  
        }

        /// <summary>
        /// Test Company GetAsNoTraking
        /// </summary>
        public void Test_Company_GetAsNoTraking()
        {
            Company company;
            company = companyRepository.GetAsNoTraking(b => b.ID == 2);
            Assert.AreEqual(2, company.ID);
        }

        /// <summary>
        /// Test Company GetPagedList
        /// </summary>
        [TestMethod]
        public void Test_Company_GetPagedList()
        {
            IEnumerable<Company> companyList;
            companyList = companyService.GetPagedList(1, 3);
            Assert.AreEqual(3, companyList.Count());

            companyList = companyService.GetPagedList(1, 2);
            Assert.AreEqual(2, companyList.Count());

            companyList = companyService.GetPagedList(2, 2);
            Assert.AreEqual(1, companyList.Count());
        }

        /// <summary>
        /// Test Company GetAll
        /// </summary>
        [TestMethod]
        public void Test_Company_GetAll()
        {
            IEnumerable<Company> companyList;
            companyList = companyService.GetAll();
            Assert.AreEqual(seedCount, companyList.Count());
        }

        [TestCleanup]
        public void Test_Company_Cleanup()
        {
            companyService.Delete(b => b.Name == "TEST");
            companyService.Save(Helper.UserName);
        }
    }
}