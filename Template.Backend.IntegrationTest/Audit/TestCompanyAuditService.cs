using Microsoft.VisualStudio.TestTools.UnitTesting;
using Template.Backend.Data.Audit;
using Template.Backend.Data.Repositories;
using Template.Backend.Data.SpecificRepositories;
using Template.Backend.Model.Audit;
using Template.Backend.Model.Audit.Entities;
using Template.Backend.Model.Entities;
using Template.Backend.Model.Exceptions;
using Template.Backend.Service.Audit;
using Template.Backend.Service.Services;
using Template.Backend.Service.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Template.Backend.IntegrationTest.Audit
{
    /// <summary>
    /// Test CompanyAuditService class
    /// </summary>
    [TestClass]
    public class TestCompanyAuditService
    {
        static bool IsDatabaseInitialized = false;
        static int seedCount = 0;

        IDbFactory dbFactory;
        ICompanyRepository companyRepository;
        IUnitOfWork unitOfWork;
        IAuditRepository<CompanyAudit> companyAuditRepository;
        ICompanyAuditService companyAuditService;
        ICompanyService companyService;
        IValidationDictionary validationDictionary;

        /// <summary>
        /// Initialize services
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new DbFactory();
            companyRepository = new CompanyRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            companyAuditRepository = new AuditRepository<CompanyAudit>(dbFactory);
            companyAuditService = new CompanyAuditService(companyAuditRepository);
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
                Company company = Helper.CompanyFactory("TEST" + i);
                companyService.Add(company);
                companyService.Save(Helper.UserName);
            }
        }

        /// <summary>
        /// Test CompanyAudit Find
        /// </summary>
        [TestMethod]
        public void Test_CompanyAudit_Find()
        {
            var audit = companyAuditService.GetById(0);
            Assert.IsNull(audit);

            CompanyAudit companyAudit;
            Company company = Helper.CompanyFactory("TEST");
            companyService.Add(company);
            companyService.Save(Helper.UserName);
            var list = companyAuditService.GetAuditById(company.ID);
            companyAudit = companyAuditService.GetById(list.FirstOrDefault().CompanyAuditID);
            Assert.IsNotNull(companyAudit);
        }

        /// <summary>
        /// Test CompanyAudit GetAuditById
        /// </summary>
        [TestMethod]
        public void Test_CompanyAudit_GetAuditById()
        {
            Company company = Helper.CompanyFactory("TEST");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            IEnumerable<CompanyAudit> companyAuditList;
            companyAuditList = companyAuditService.GetAuditById(company.ID);
            Assert.AreEqual(1, companyAuditList.Count());

            companyAuditList = companyAuditService.GetAuditById(10000);
            Assert.AreEqual(0, companyAuditList.Count());
        }

        /// <summary>
        /// Test CompanyAudit add and delete
        /// </summary>
        [TestMethod]
        public void Test_CompanyAudit_Add_Delete()
        {
            Company company;
            IEnumerable<CompanyAudit> companyAuditList;
            int count = companyAuditService.GetAll().Count();

            company = Helper.CompanyFactory("TEST");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            Assert.AreEqual(count + 1, companyAuditService.GetAll().Count());

            companyAuditList = companyAuditService.GetAuditById(company.ID);
            Assert.AreEqual(1, companyAuditList.Count());
            Assert.AreEqual(Helper.UserName, companyAuditList.FirstOrDefault().LoggedUserName);
            Assert.AreEqual(company.ID, companyAuditList.FirstOrDefault().ID);
            Assert.AreEqual(1, companyAuditList.FirstOrDefault().RowVersion);
            Assert.AreEqual(AuditOperations.INSERT, companyAuditList.FirstOrDefault().AuditOperation);

            companyService.Delete(company.ID);
            companyService.Save(Helper.UserName);

            Assert.AreEqual(count + 2, companyAuditService.GetAll().Count());

            companyAuditList = companyAuditService.GetAuditById(company.ID);
            Assert.AreEqual(2, companyAuditList.Count());
            Assert.AreEqual(company.ID, companyAuditList.LastOrDefault().ID);
            Assert.AreEqual(1, companyAuditList.LastOrDefault().RowVersion);
            Assert.AreEqual(AuditOperations.DELETE, companyAuditList.LastOrDefault().AuditOperation);

            IsDatabaseInitialized = false;
        }

        /// <summary>
        /// Test CompanyAudit Update
        /// </summary>
        [TestMethod]
        public void Test_CompanyAudit_Update()
        {
            Company company;
            IEnumerable<CompanyAudit> companyAuditList;
            int count = companyAuditService.GetAll().Count();

            company = Helper.CompanyFactory("TEST");
            company.Name = "OLD_NAME";
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            Assert.AreEqual(count + 1, companyAuditService.GetAll().Count());
            Assert.AreNotEqual("NEW_NAME", company.Name);

            company.Name = "NEW_NAME";
            companyService.Update(company);
            companyService.Save(Helper.UserName);

            Assert.AreEqual(count + 2, companyAuditService.GetAll().Count());

            companyAuditList = companyAuditService.GetAuditById(company.ID);
            Assert.AreEqual(2, companyAuditList.Count());
            Assert.IsTrue(companyAuditList.All(b => b.ID == company.ID));
            Assert.AreEqual(1, companyAuditList.FirstOrDefault().RowVersion);
            Assert.AreEqual(AuditOperations.INSERT, companyAuditList.FirstOrDefault().AuditOperation);
            Assert.AreEqual("OLD_NAME", companyAuditList.FirstOrDefault().Name);
            Assert.AreEqual(2, companyAuditList.LastOrDefault().RowVersion);
            Assert.AreEqual(AuditOperations.UPDATE, companyAuditList.LastOrDefault().AuditOperation);
            Assert.AreEqual("NEW_NAME", companyAuditList.LastOrDefault().Name);

            IsDatabaseInitialized = false;
        }

        /// <summary>
        /// Test CompanyAudit GetAll
        /// </summary>
        [TestMethod]
        public void Test_CompanyAudit_GetAll()
        {
            IEnumerable<CompanyAudit> companyAuditList;
            companyAuditList = companyAuditRepository.GetAll();
            Assert.IsTrue(companyAuditList.Count()>= seedCount);
        }

        /// <summary>
        /// Test CompanyAudit GetByIdSnapshot
        /// </summary>
        [TestMethod]
        public void Test_CompanyAudit_GetByIdSnapshot()
        {
            Company company;
            int count = companyAuditService.GetAll().Count();
            Assert.AreEqual(count, companyAuditService.GetAll().Count());

            company = Helper.CompanyFactory("TEST");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            Assert.AreEqual(count + 1, companyAuditService.GetAll().Count());

            DateTime now = DateTime.Now;
            CompanyAudit oldCompanyAudit = companyAuditService.GetByIdSnapshot(now, company.ID);
            Assert.IsNotNull(oldCompanyAudit);
            Assert.AreEqual(company.ID, oldCompanyAudit.ID);
            Assert.IsTrue(now >= oldCompanyAudit.CreatedDate);

            company.Name = "NEW_NAME";
            companyService.Update(company);
            companyService.Save(Helper.UserName);

            Assert.AreEqual(count + 2, companyAuditService.GetAll().Count());

            CompanyAudit companyAudit = companyAuditService.GetByIdSnapshot(now, company.ID);
            Assert.IsNotNull(companyAudit);
            Assert.IsTrue(now >= companyAudit.CreatedDate);
            Assert.AreEqual(oldCompanyAudit.ID, companyAudit.ID);
            Assert.AreEqual(oldCompanyAudit.Name, companyAudit.Name);
            Assert.AreNotEqual("NEW_NAME", companyAudit.Name);

            now = DateTime.Now;
            CompanyAudit newcompanyAudit = companyAuditService.GetByIdSnapshot(now, company.ID);
            Assert.IsNotNull(newcompanyAudit);
            Assert.IsTrue(now >= newcompanyAudit.CreatedDate);
            Assert.AreEqual(oldCompanyAudit.ID, newcompanyAudit.ID);
            Assert.AreNotEqual(oldCompanyAudit.Name, newcompanyAudit.Name);
            Assert.AreEqual("NEW_NAME", newcompanyAudit.Name);
        }

        /// <summary>
        /// Test CompanyAudit GetAllSnapshot
        /// </summary>
        [TestMethod]
        public void Test_CompanyAudit_GetAllSnapshot()
        {
            Company company;
            DateTime now = DateTime.Now;
            int countNow = companyAuditService.GetAllSnapshot(DateTime.Now).Count();

            company = Helper.CompanyFactory("TEST");
            companyService.Add(company);
            companyService.Save(Helper.UserName);
            Thread.Sleep(1000);
            Assert.AreEqual(countNow + 1, companyAuditService.GetAllSnapshot(DateTime.Now).Count());

            Assert.AreEqual(countNow, companyAuditService.GetAllSnapshot(now).Count());

            company.Name = "NEW_NAME";
            companyService.Update(company);
            companyService.Save(Helper.UserName);

            Assert.AreEqual(countNow, companyAuditService.GetAllSnapshot(now).Count());
            now = DateTime.Now;
            Assert.AreEqual(countNow+1, companyAuditService.GetAllSnapshot(now).Count());
        }

        /// <summary>
        /// Test CompanyAudit Restore
        /// </summary>
        [TestMethod]
        public void Test_CompanyAudit_Restore()
        {
            int count = companyAuditService.GetAll().Count();
            DateTime now;
            Company company = Helper.CompanyFactory("TEST");
            companyService.Add(company);
            companyService.Save(Helper.UserName);
            Thread.Sleep(1000);
            now = DateTime.Now;
            Thread.Sleep(1000);
            company.Name = "NEW_NAME";
            companyService.Update(company);
            companyService.Save(Helper.UserName);

            Assert.AreEqual(count + 2, companyAuditService.GetAll().Count());

            CompanyAudit oldCompanyAudit = companyAuditService.GetByIdSnapshot(now, company.ID);

            Assert.AreNotEqual(oldCompanyAudit.Name, company.Name);

            companyService.Restore(company, oldCompanyAudit);
            Assert.AreEqual(oldCompanyAudit.Name, company.Name);
        }

        [TestCleanup]
        public void Test_CompanyAudit_Cleanup()
        {
            companyService.Delete(b => b.Name == "TEST");
            companyService.Delete(b => b.Name == "NEW_NAME");
            companyService.Save(Helper.UserName);
        }
    }
}