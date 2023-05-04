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
    /// Test EmployeeAuditService class
    /// </summary>
    [TestClass]
    public class TestEmployeeAuditService
    {
        static bool IsDatabaseInitialized = false;
        static int seedCount = 0;

        IDbFactory dbFactory;
        IEmployeeRepository employeeRepository;
        IUnitOfWork unitOfWork;
        IAuditRepository<EmployeeAudit> employeeAuditRepository;
        IEmployeeAuditService employeeAuditService;
        IEmployeeService employeeService;
        IValidationDictionary validationDictionary;

        ICompanyRepository companyRepository;
        ICompanyService companyService;

        /// <summary>
        /// Initialize services
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new DbFactory();
            employeeRepository = new EmployeeRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            employeeAuditRepository = new AuditRepository<EmployeeAudit>(dbFactory);
            employeeAuditService = new EmployeeAuditService(employeeAuditRepository);
            validationDictionary = new ValidationDictionary();
            employeeService = new EmployeeService(employeeRepository, unitOfWork, validationDictionary);

            companyRepository = new CompanyRepository(dbFactory);
            companyService = new CompanyService(companyRepository, unitOfWork, validationDictionary);

            if (!IsDatabaseInitialized)
            {
                Helper.InitializeLocalDatabase(dbFactory);
                IsDatabaseInitialized = true;
                Seed(3);
            }

            seedCount = employeeService.Count();
        }

        private void Seed(int number)
        {
            Company company = Helper.CompanyFactory("TEST");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            for (int i = 0; i < number; i++)
            {
                Employee employee = Helper.EmployeeFactory("TEST" + i, company.ID);
                employeeService.Add(employee);
                employeeService.Save(Helper.UserName);
            }
        }

        /// <summary>
        /// Test EmployeeAudit Find
        /// </summary>
        [TestMethod]
        public void Test_EmployeeAudit_Find()
        {
            Company company = Helper.CompanyFactory("TEST2");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            var audit = employeeAuditService.GetById(0);
            Assert.IsNull(audit);

            EmployeeAudit employeeAudit;
            Employee employee = Helper.EmployeeFactory("TEST", company.ID);
            employeeService.Add(employee);
            employeeService.Save(Helper.UserName);
            IEnumerable<EmployeeAudit> list = employeeAuditService.GetAuditById(employee.ID);
            employeeAudit = employeeAuditService.GetById(list.FirstOrDefault().EmployeeAuditID);
            Assert.IsNotNull(employeeAudit);
        }

        /// <summary>
        /// Test EmployeeAudit GetAuditById
        /// </summary>
        [TestMethod]
        public void Test_EmployeeAudit_GetAuditById()
        {
            Company company = Helper.CompanyFactory("TEST3");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            Employee employee = Helper.EmployeeFactory("TEST", company.ID);
            employeeService.Add(employee);
            employeeService.Save(Helper.UserName);

            IEnumerable<EmployeeAudit> employeeAuditList;
            employeeAuditList = employeeAuditService.GetAuditById(employee.ID);
            Assert.AreEqual(1, employeeAuditList.Count());

            employeeAuditList = employeeAuditService.GetAuditById(10000);
            Assert.AreEqual(0, employeeAuditList.Count());
        }

        /// <summary>
        /// Test EmployeeAudit add and delete
        /// </summary>
        [TestMethod]
        public void Test_EmployeeAudit_Add_Delete()
        {
            Company company = Helper.CompanyFactory("TEST4");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            Employee employee;
            IEnumerable<EmployeeAudit> employeeAuditList;
            int count = employeeAuditService.GetAll().Count();

            employee = Helper.EmployeeFactory("TEST", company.ID);
            employeeService.Add(employee);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(count + 1, employeeAuditService.GetAll().Count());

            employeeAuditList = employeeAuditService.GetAuditById(employee.ID);
            Assert.AreEqual(1, employeeAuditList.Count());
            Assert.AreEqual(Helper.UserName, employeeAuditList.FirstOrDefault().LoggedUserName);
            Assert.AreEqual(employee.ID, employeeAuditList.FirstOrDefault().ID);
            Assert.AreEqual(1, employeeAuditList.FirstOrDefault().RowVersion);
            Assert.AreEqual(AuditOperations.INSERT, employeeAuditList.FirstOrDefault().AuditOperation);

            employeeService.Delete(employee.ID);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(count + 2, employeeAuditService.GetAll().Count());

            employeeAuditList = employeeAuditService.GetAuditById(employee.ID);
            Assert.AreEqual(2, employeeAuditList.Count());
            Assert.AreEqual(employee.ID, employeeAuditList.LastOrDefault().ID);
            Assert.AreEqual(1, employeeAuditList.LastOrDefault().RowVersion);
            Assert.AreEqual(AuditOperations.DELETE, employeeAuditList.LastOrDefault().AuditOperation);

            IsDatabaseInitialized = false;
        }

        /// <summary>
        /// Test EmployeeAudit Update
        /// </summary>
        [TestMethod]
        public void Test_EmployeeAudit_Update()
        {
            Company company = Helper.CompanyFactory("TEST5");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            Employee employee;
            IEnumerable<EmployeeAudit> employeeAuditList;
            int count = employeeAuditService.GetAll().Count();

            employee = Helper.EmployeeFactory("TEST", company.ID);
            employee.Name = "OLD_NAME";
            employeeService.Add(employee);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(count + 1, employeeAuditService.GetAll().Count());
            Assert.AreNotEqual("NEW_NAME", employee.Name);

            employee.Name = "NEW_NAME";
            employeeService.Update(employee);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(count + 2, employeeAuditService.GetAll().Count());

            employeeAuditList = employeeAuditService.GetAuditById(employee.ID);
            Assert.AreEqual(2, employeeAuditList.Count());
            Assert.IsTrue(employeeAuditList.All(b => b.ID == employee.ID));
            Assert.AreEqual(1, employeeAuditList.FirstOrDefault().RowVersion);
            Assert.AreEqual(AuditOperations.INSERT, employeeAuditList.FirstOrDefault().AuditOperation);
            Assert.AreEqual("OLD_NAME", employeeAuditList.FirstOrDefault().Name);
            Assert.AreEqual(2, employeeAuditList.LastOrDefault().RowVersion);
            Assert.AreEqual(AuditOperations.UPDATE, employeeAuditList.LastOrDefault().AuditOperation);
            Assert.AreEqual("NEW_NAME", employeeAuditList.LastOrDefault().Name);

            IsDatabaseInitialized = false;
        }

        /// <summary>
        /// Test EmployeeAudit GetAll
        /// </summary>
        [TestMethod]
        public void Test_EmployeeAudit_GetAll()
        {
            IEnumerable<EmployeeAudit> employeeAuditList;
            employeeAuditList = employeeAuditRepository.GetAll();
            Assert.IsTrue(employeeAuditList.Count()>= seedCount);
        }

        /// <summary>
        /// Test EmployeeAudit GetByIdSnapshot
        /// </summary>
        [TestMethod]
        public void Test_EmployeeAudit_GetByIdSnapshot()
        {
            Company company = Helper.CompanyFactory("TEST6");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            Employee employee;
            int count = employeeAuditService.GetAll().Count();
            Assert.AreEqual(count, employeeAuditService.GetAll().Count());

            employee = Helper.EmployeeFactory("TEST", company.ID);
            employeeService.Add(employee);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(count + 1, employeeAuditService.GetAll().Count());

            DateTime now = DateTime.Now;
            EmployeeAudit oldEmployeeAudit = employeeAuditService.GetByIdSnapshot(now, employee.ID);
            Assert.IsNotNull(oldEmployeeAudit);
            Assert.AreEqual(employee.ID, oldEmployeeAudit.ID);
            Assert.IsTrue(now >= oldEmployeeAudit.CreatedDate);

            employee.Name = "NEW_NAME";
            employeeService.Update(employee);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(count + 2, employeeAuditService.GetAll().Count());

            EmployeeAudit employeeAudit = employeeAuditService.GetByIdSnapshot(now, employee.ID);
            Assert.IsNotNull(employeeAudit);
            Assert.IsTrue(now >= employeeAudit.CreatedDate);
            Assert.AreEqual(oldEmployeeAudit.ID, employeeAudit.ID);
            Assert.AreEqual(oldEmployeeAudit.Name, employeeAudit.Name);
            Assert.AreNotEqual("NEW_NAME", employeeAudit.Name);

            now = DateTime.Now;
            EmployeeAudit newemployeeAudit = employeeAuditService.GetByIdSnapshot(now, employee.ID);
            Assert.IsNotNull(newemployeeAudit);
            Assert.IsTrue(now >= newemployeeAudit.CreatedDate);
            Assert.AreEqual(oldEmployeeAudit.ID, newemployeeAudit.ID);
            Assert.AreNotEqual(oldEmployeeAudit.Name, newemployeeAudit.Name);
            Assert.AreEqual("NEW_NAME", newemployeeAudit.Name);
        }

        /// <summary>
        /// Test EmployeeAudit GetAllSnapshot
        /// </summary>
        [TestMethod]
        public void Test_EmployeeAudit_GetAllSnapshot()
        {
            Company company = Helper.CompanyFactory("TEST7");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            Employee employee;
            DateTime now = DateTime.Now;
            int countNow = employeeAuditService.GetAllSnapshot(DateTime.Now).Count();
            Thread.Sleep(1000);
            employee = Helper.EmployeeFactory("TEST", company.ID);
            employeeService.Add(employee);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(countNow + 1, employeeAuditService.GetAllSnapshot(DateTime.Now).Count());

            Assert.AreEqual(countNow, employeeAuditService.GetAllSnapshot(now).Count());

            employee.Name = "NEW_NAME";
            employeeService.Update(employee);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(countNow, employeeAuditService.GetAllSnapshot(now).Count());
            now = DateTime.Now;
            Assert.AreEqual(countNow+1, employeeAuditService.GetAllSnapshot(now).Count());
        }

        /// <summary>
        /// Test EmployeeAudit Restore
        /// </summary>
        [TestMethod]
        public void Test_EmployeeAudit_Restore()
        {
            Company company = Helper.CompanyFactory("TEST8");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            int count = employeeAuditService.GetAll().Count();
            DateTime now;
            Employee employee = Helper.EmployeeFactory("TEST", company.ID);
            employeeService.Add(employee);
            employeeService.Save(Helper.UserName);
            Thread.Sleep(1000);
            now = DateTime.Now;
            Thread.Sleep(1000);
            employee.Name = "NEW_NAME";
            employeeService.Update(employee);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(count + 2, employeeAuditService.GetAll().Count());

            EmployeeAudit oldEmployeeAudit = employeeAuditService.GetByIdSnapshot(now, employee.ID);

            Assert.AreNotEqual(oldEmployeeAudit.Name, employee.Name);

            employeeService.Restore(employee, oldEmployeeAudit);
            Assert.AreEqual(oldEmployeeAudit.Name, employee.Name);
        }

        [TestCleanup]
        public void Test_EmployeeAudit_Cleanup()
        {
            employeeService.Delete(b => b.Name == "TEST");
            employeeService.Delete(b => b.Name == "NEW_NAME");
            employeeService.Save(Helper.UserName);
        }
    }
}