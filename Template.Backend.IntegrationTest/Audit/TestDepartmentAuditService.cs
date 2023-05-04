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
    /// Test DepartmentAuditService class
    /// </summary>
    [TestClass]
    public class TestDepartmentAuditService
    {
        static bool IsDatabaseInitialized = false;
        static int seedCount = 0;

        IDbFactory dbFactory;
        IDepartmentRepository departmentRepository;
        IUnitOfWork unitOfWork;
        IAuditRepository<DepartmentAudit> departmentAuditRepository;
        IDepartmentAuditService departmentAuditService;
        IDepartmentService departmentService;
        IValidationDictionary validationDictionary;

        /// <summary>
        /// Initialize services
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new DbFactory();
            departmentRepository = new DepartmentRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            departmentAuditRepository = new AuditRepository<DepartmentAudit>(dbFactory);
            departmentAuditService = new DepartmentAuditService(departmentAuditRepository);
            validationDictionary = new ValidationDictionary();
            departmentService = new DepartmentService(departmentRepository, unitOfWork, validationDictionary);

            if (!IsDatabaseInitialized)
            {
                Helper.InitializeLocalDatabase(dbFactory);
                IsDatabaseInitialized = true;
                Seed(3);
            }

            seedCount = departmentService.Count();
        }

        private void Seed(int number)
        {
            for (int i = 0; i < number; i++)
            {
                Department department = Helper.DepartmentFactory("TEST" + i);
                departmentService.Add(department);
                departmentService.Save(Helper.UserName);
            }
        }

        /// <summary>
        /// Test DepartmentAudit Find
        /// </summary>
        [TestMethod]
        public void Test_DepartmentAudit_Find()
        {
            var audit = departmentAuditService.GetById(0);
            Assert.IsNull(audit);

            DepartmentAudit departmentAudit;
            Department department = Helper.DepartmentFactory("TEST");
            departmentService.Add(department);
            departmentService.Save(Helper.UserName);
            var list = departmentAuditService.GetAuditById(department.ID);
            departmentAudit = departmentAuditService.GetById(list.FirstOrDefault().DepartmentAuditID);
            Assert.IsNotNull(departmentAudit);
        }

        /// <summary>
        /// Test DepartmentAudit GetAuditById
        /// </summary>
        [TestMethod]
        public void Test_DepartmentAudit_GetAuditById()
        {
            Department department = Helper.DepartmentFactory("TEST");
            departmentService.Add(department);
            departmentService.Save(Helper.UserName);

            IEnumerable<DepartmentAudit> departmentAuditList;
            departmentAuditList = departmentAuditService.GetAuditById(department.ID);
            Assert.AreEqual(1, departmentAuditList.Count());

            departmentAuditList = departmentAuditService.GetAuditById(10000);
            Assert.AreEqual(0, departmentAuditList.Count());
        }

        /// <summary>
        /// Test DepartmentAudit add and delete
        /// </summary>
        [TestMethod]
        public void Test_DepartmentAudit_Add_Delete()
        {
            Department department;
            IEnumerable<DepartmentAudit> departmentAuditList;
            int count = departmentAuditService.GetAll().Count();

            department = Helper.DepartmentFactory("TEST");
            departmentService.Add(department);
            departmentService.Save(Helper.UserName);

            Assert.AreEqual(count + 1, departmentAuditService.GetAll().Count());

            departmentAuditList = departmentAuditService.GetAuditById(department.ID);
            Assert.AreEqual(1, departmentAuditList.Count());
            Assert.AreEqual(Helper.UserName, departmentAuditList.FirstOrDefault().LoggedUserName);
            Assert.AreEqual(department.ID, departmentAuditList.FirstOrDefault().ID);
            Assert.AreEqual(1, departmentAuditList.FirstOrDefault().RowVersion);
            Assert.AreEqual(AuditOperations.INSERT, departmentAuditList.FirstOrDefault().AuditOperation);

            departmentService.Delete(department.ID);
            departmentService.Save(Helper.UserName);

            Assert.AreEqual(count + 2, departmentAuditService.GetAll().Count());

            departmentAuditList = departmentAuditService.GetAuditById(department.ID);
            Assert.AreEqual(2, departmentAuditList.Count());
            Assert.AreEqual(department.ID, departmentAuditList.LastOrDefault().ID);
            Assert.AreEqual(1, departmentAuditList.LastOrDefault().RowVersion);
            Assert.AreEqual(AuditOperations.DELETE, departmentAuditList.LastOrDefault().AuditOperation);

            IsDatabaseInitialized = false;
        }

        /// <summary>
        /// Test DepartmentAudit Update
        /// </summary>
        [TestMethod]
        public void Test_DepartmentAudit_Update()
        {
            Department department;
            IEnumerable<DepartmentAudit> departmentAuditList;
            int count = departmentAuditService.GetAll().Count();

            department = Helper.DepartmentFactory("TEST");
            department.Name = "OLD_NAME";
            departmentService.Add(department);
            departmentService.Save(Helper.UserName);

            Assert.AreEqual(count + 1, departmentAuditService.GetAll().Count());
            Assert.AreNotEqual("NEW_NAME", department.Name);

            department.Name = "NEW_NAME";
            departmentService.Update(department);
            departmentService.Save(Helper.UserName);

            Assert.AreEqual(count + 2, departmentAuditService.GetAll().Count());

            departmentAuditList = departmentAuditService.GetAuditById(department.ID);
            Assert.AreEqual(2, departmentAuditList.Count());
            Assert.IsTrue(departmentAuditList.All(b => b.ID == department.ID));
            Assert.AreEqual(1, departmentAuditList.FirstOrDefault().RowVersion);
            Assert.AreEqual(AuditOperations.INSERT, departmentAuditList.FirstOrDefault().AuditOperation);
            Assert.AreEqual("OLD_NAME", departmentAuditList.FirstOrDefault().Name);
            Assert.AreEqual(2, departmentAuditList.LastOrDefault().RowVersion);
            Assert.AreEqual(AuditOperations.UPDATE, departmentAuditList.LastOrDefault().AuditOperation);
            Assert.AreEqual("NEW_NAME", departmentAuditList.LastOrDefault().Name);

            IsDatabaseInitialized = false;
        }

        /// <summary>
        /// Test DepartmentAudit GetAll
        /// </summary>
        [TestMethod]
        public void Test_DepartmentAudit_GetAll()
        {
            IEnumerable<DepartmentAudit> departmentAuditList;
            departmentAuditList = departmentAuditRepository.GetAll();
            Assert.IsTrue(departmentAuditList.Count()>= seedCount);
        }

        /// <summary>
        /// Test DepartmentAudit GetByIdSnapshot
        /// </summary>
        [TestMethod]
        public void Test_DepartmentAudit_GetByIdSnapshot()
        {
            Department department;
            int count = departmentAuditService.GetAll().Count();
            Assert.AreEqual(count, departmentAuditService.GetAll().Count());

            department = Helper.DepartmentFactory("TEST");
            departmentService.Add(department);
            departmentService.Save(Helper.UserName);

            Assert.AreEqual(count + 1, departmentAuditService.GetAll().Count());

            DateTime now = DateTime.Now;
            DepartmentAudit oldDepartmentAudit = departmentAuditService.GetByIdSnapshot(now, department.ID);
            Assert.IsNotNull(oldDepartmentAudit);
            Assert.AreEqual(department.ID, oldDepartmentAudit.ID);
            Assert.IsTrue(now >= oldDepartmentAudit.CreatedDate);

            department.Name = "NEW_NAME";
            departmentService.Update(department);
            departmentService.Save(Helper.UserName);

            Assert.AreEqual(count + 2, departmentAuditService.GetAll().Count());

            DepartmentAudit departmentAudit = departmentAuditService.GetByIdSnapshot(now, department.ID);
            Assert.IsNotNull(departmentAudit);
            Assert.IsTrue(now >= departmentAudit.CreatedDate);
            Assert.AreEqual(oldDepartmentAudit.ID, departmentAudit.ID);
            Assert.AreEqual(oldDepartmentAudit.Name, departmentAudit.Name);
            Assert.AreNotEqual("NEW_NAME", departmentAudit.Name);

            now = DateTime.Now;
            DepartmentAudit newdepartmentAudit = departmentAuditService.GetByIdSnapshot(now, department.ID);
            Assert.IsNotNull(newdepartmentAudit);
            Assert.IsTrue(now >= newdepartmentAudit.CreatedDate);
            Assert.AreEqual(oldDepartmentAudit.ID, newdepartmentAudit.ID);
            Assert.AreNotEqual(oldDepartmentAudit.Name, newdepartmentAudit.Name);
            Assert.AreEqual("NEW_NAME", newdepartmentAudit.Name);
        }

        /// <summary>
        /// Test DepartmentAudit GetAllSnapshot
        /// </summary>
        [TestMethod]
        public void Test_DepartmentAudit_GetAllSnapshot()
        {
            Department department;
            DateTime now = DateTime.Now;
            int countNow = departmentAuditService.GetAllSnapshot(DateTime.Now).Count();

            department = Helper.DepartmentFactory("TEST");
            departmentService.Add(department);
            departmentService.Save(Helper.UserName);
            Thread.Sleep(1000);
            Assert.AreEqual(countNow + 1, departmentAuditService.GetAllSnapshot(DateTime.Now).Count());

            Assert.AreEqual(countNow, departmentAuditService.GetAllSnapshot(now).Count());

            department.Name = "NEW_NAME";
            departmentService.Update(department);
            departmentService.Save(Helper.UserName);

            Assert.AreEqual(countNow, departmentAuditService.GetAllSnapshot(now).Count());
            now = DateTime.Now;
            Assert.AreEqual(countNow+1, departmentAuditService.GetAllSnapshot(now).Count());
        }

        /// <summary>
        /// Test DepartmentAudit Restore
        /// </summary>
        [TestMethod]
        public void Test_DepartmentAudit_Restore()
        {
            int count = departmentAuditService.GetAll().Count();
            DateTime now;
            Department department = Helper.DepartmentFactory("TEST");
            departmentService.Add(department);
            departmentService.Save(Helper.UserName);
            Thread.Sleep(1000);
            now = DateTime.Now;
            Thread.Sleep(1000);
            department.Name = "NEW_NAME";
            departmentService.Update(department);
            departmentService.Save(Helper.UserName);

            Assert.AreEqual(count + 2, departmentAuditService.GetAll().Count());

            DepartmentAudit oldDepartmentAudit = departmentAuditService.GetByIdSnapshot(now, department.ID);

            Assert.AreNotEqual(oldDepartmentAudit.Name, department.Name);

            departmentService.Restore(department, oldDepartmentAudit);
            Assert.AreEqual(oldDepartmentAudit.Name, department.Name);
        }

        [TestCleanup]
        public void Test_DepartmentAudit_Cleanup()
        {
            departmentService.Delete(b => b.Name == "TEST");
            departmentService.Delete(b => b.Name == "NEW_NAME");
            departmentService.Save(Helper.UserName);
        }
    }
}