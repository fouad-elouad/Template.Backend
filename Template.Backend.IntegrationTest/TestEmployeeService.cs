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
    /// Test EmployeeService
    /// </summary>
    [TestClass]
    public class TestEmployeeService
    {
        static bool IsDatabaseInitialized = false;
        static int seedCount = 0;

        IDbFactory dbFactory;
        IEmployeeRepository employeeRepository;
        IUnitOfWork unitOfWork;
        IValidationDictionary validationDictionary;
        IEmployeeService employeeService;

        ICompanyRepository companyRepository;
        ICompanyService companyService;

        /// <summary>
        /// Initialize 
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new DbFactory();
            employeeRepository = new EmployeeRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
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
                Employee employee = Helper.EmployeeFactory("TEST"+i, company.ID);
                employeeService.Add(employee);
                employeeService.Save(Helper.UserName);
            }
        }

        /// <summary>
        /// Test Employee Count
        /// </summary>
        [TestMethod]
        public void Test_Employee_Count()
        {
            Assert.AreEqual(seedCount, employeeService.Count());
        }

        /// <summary>
        /// Test Employee Find
        /// </summary>
        [TestMethod]
        public void Test_Employee_Find()
        {
            Employee employee;
            employee = employeeService.GetById(1);
            Assert.AreEqual(1, employee.ID);

            employee = employeeService.GetById(0);
            Assert.IsNull(employee);
        }

        /// <summary>
        /// Test Employee Add and delete
        /// </summary>
        [TestMethod]
        public void Test_Employee_Add_Delete()
        {
            Company company = Helper.CompanyFactory("TEST2");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            Employee employee;
            Assert.AreEqual(seedCount, employeeService.Count());

            employee = Helper.EmployeeFactory("TEST", company.ID);
            employeeService.Add(employee);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(seedCount +1, employeeService.Count());

            employeeService.Delete(employee.ID);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(seedCount, employeeService.Count());
        }

        /// <summary>
        /// Test Employee Add duplicate Name
        /// </summary>
        [TestMethod]
        public void Test_Employee_Add_Duplicate_Name()
        {
            Company company = Helper.CompanyFactory("TEST3");
            companyService.Add(company);
            companyService.Save(Helper.UserName);

            Employee employee;
            Assert.AreEqual(seedCount, employeeService.Count());

            employee = Helper.EmployeeFactory("TEST", company.ID);
            employeeService.Add(employee);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(seedCount +1, employeeService.Count());

            Employee employee2 = Helper.EmployeeFactory("TEST", company.ID);
            employeeService.Add(employee2);
            employeeService.Save(Helper.UserName);

            Assert.IsFalse(employeeService.GetValidationDictionary().IsValid());
            Assert.AreEqual(seedCount +1, employeeService.Count());

            employeeService.Delete(employee.ID);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(seedCount, employeeService.Count());
        }

        /// <summary>
        /// Test Employee Update
        /// </summary>
        [TestMethod]
        public void Test_Employee_Update()
        {
            Employee employee;

            employee = employeeService.GetById(1);
            string previousName = employee.Name;
            string newName = "New Name";

            Assert.AreNotEqual(newName, employee.Name);

            employee.Name = newName;
            employeeService.Update(employee);
            employeeService.Save(Helper.UserName);

            Assert.AreEqual(newName, employee.Name);

            employee.Name = previousName;
            employeeService.Save(Helper.UserName);
        }

        /// <summary>
        /// Test Employee get
        /// </summary>
        [TestMethod]
        public void Test_Employee_Get()
        {
            Employee employee;
            employee = employeeService.Get(b => b.ID == 3);
            Assert.AreEqual(3, employee.ID);  
        }

        /// <summary>
        /// Test Employee GetAsNoTraking
        /// </summary>
        public void Test_Employee_GetAsNoTraking()
        {
            Employee employee;
            employee = employeeRepository.GetAsNoTraking(b => b.ID == 2);
            Assert.AreEqual(2, employee.ID);
        }

        /// <summary>
        /// Test Employee GetPagedList
        /// </summary>
        [TestMethod]
        public void Test_Employee_GetPagedList()
        {
            IEnumerable<Employee> employeeList;
            employeeList = employeeService.GetPagedList(1, 3);
            Assert.AreEqual(3, employeeList.Count());

            employeeList = employeeService.GetPagedList(1, 2);
            Assert.AreEqual(2, employeeList.Count());

            employeeList = employeeService.GetPagedList(2, 2);
            Assert.AreEqual(1, employeeList.Count());
        }

        /// <summary>
        /// Test Employee GetAll
        /// </summary>
        [TestMethod]
        public void Test_Employee_GetAll()
        {
            IEnumerable<Employee> employeeList;
            employeeList = employeeService.GetAll();
            Assert.AreEqual(seedCount, employeeList.Count());
        }

        [TestCleanup]
        public void Test_Employee_Cleanup()
        {
            employeeService.Delete(b => b.Name == "TEST");
            employeeService.Save(Helper.UserName);
        }
    }
}