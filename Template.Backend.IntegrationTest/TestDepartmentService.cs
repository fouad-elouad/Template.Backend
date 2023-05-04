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
    /// Test DepartmentService
    /// </summary>
    [TestClass]
    public class TestDepartmentService
    {
        static bool IsDatabaseInitialized = false;
        static int seedCount = 0;

        IDbFactory dbFactory;
        IDepartmentRepository departmentRepository;
        IUnitOfWork unitOfWork;
        IValidationDictionary validationDictionary;
        IDepartmentService departmentService;

        /// <summary>
        /// Initialize 
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new DbFactory();
            departmentRepository = new DepartmentRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
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
                Department department = Helper.DepartmentFactory("TEST"+i);
                departmentService.Add(department);
                departmentService.Save(Helper.UserName);
            }
        }

        /// <summary>
        /// Test Department Count
        /// </summary>
        [TestMethod]
        public void Test_Department_Count()
        {
            Assert.AreEqual(seedCount, departmentService.Count());
        }

        /// <summary>
        /// Test Department Find
        /// </summary>
        [TestMethod]
        public void Test_Department_Find()
        {
            Department department;
            department = departmentService.GetById(1);
            Assert.AreEqual(1, department.ID);

            department = departmentService.GetById(0);
            Assert.IsNull(department);
        }

        /// <summary>
        /// Test Department Add and delete
        /// </summary>
        [TestMethod]
        public void Test_Department_Add_Delete()
        {
            Department department;
            Assert.AreEqual(seedCount, departmentService.Count());

            department = Helper.DepartmentFactory("TEST");
            departmentService.Add(department);
            departmentService.Save(Helper.UserName);

            Assert.AreEqual(seedCount +1, departmentService.Count());

            departmentService.Delete(department.ID);
            departmentService.Save(Helper.UserName);

            Assert.AreEqual(seedCount, departmentService.Count());
        }

        /// <summary>
        /// Test Department Add duplicate Name
        /// </summary>
        [TestMethod]
        public void Test_Department_Add_Duplicate_Name()
        {
            Department department;
            Assert.AreEqual(seedCount, departmentService.Count());

            department = Helper.DepartmentFactory("TEST");
            departmentService.Add(department);
            departmentService.Save(Helper.UserName);

            Assert.AreEqual(seedCount +1, departmentService.Count());

            Department department2 = Helper.DepartmentFactory("TEST");
            departmentService.Add(department2);
            departmentService.Save(Helper.UserName);

            Assert.IsFalse(departmentService.GetValidationDictionary().IsValid());
            Assert.AreEqual(seedCount +1, departmentService.Count());

            departmentService.Delete(department.ID);
            departmentService.Save(Helper.UserName);

            Assert.AreEqual(seedCount, departmentService.Count());
        }

        /// <summary>
        /// Test Department Update
        /// </summary>
        [TestMethod]
        public void Test_Department_Update()
        {
            Department department;

            department = departmentService.GetById(1);
            string previousName = department.Name;
            string newName = "New Name";

            Assert.AreNotEqual(newName, department.Name);

            department.Name = newName;
            departmentService.Update(department);
            departmentService.Save(Helper.UserName);

            Assert.AreEqual(newName, department.Name);

            department.Name = previousName;
            departmentService.Save(Helper.UserName);
        }

        /// <summary>
        /// Test Department get
        /// </summary>
        [TestMethod]
        public void Test_Department_Get()
        {
            Department department;
            department = departmentService.Get(b => b.ID == 3);
            Assert.AreEqual(3, department.ID);  
        }

        /// <summary>
        /// Test Department GetAsNoTraking
        /// </summary>
        public void Test_Department_GetAsNoTraking()
        {
            Department department;
            department = departmentRepository.GetAsNoTraking(b => b.ID == 2);
            Assert.AreEqual(2, department.ID);
        }

        /// <summary>
        /// Test Department GetPagedList
        /// </summary>
        [TestMethod]
        public void Test_Department_GetPagedList()
        {
            IEnumerable<Department> departmentList;
            departmentList = departmentService.GetPagedList(1, 3);
            Assert.AreEqual(3, departmentList.Count());

            departmentList = departmentService.GetPagedList(1, 2);
            Assert.AreEqual(2, departmentList.Count());

            departmentList = departmentService.GetPagedList(2, 2);
            Assert.AreEqual(1, departmentList.Count());
        }

        /// <summary>
        /// Test Department GetAll
        /// </summary>
        [TestMethod]
        public void Test_Department_GetAll()
        {
            IEnumerable<Department> departmentList;
            departmentList = departmentService.GetAll();
            Assert.AreEqual(seedCount, departmentList.Count());
        }

        [TestCleanup]
        public void Test_Department_Cleanup()
        {
            departmentService.Delete(b => b.Name == "TEST");
            departmentService.Save(Helper.UserName);
        }
    }
}