using Template.Backend.Data;
using Template.Backend.Data.Repositories;
using Template.Backend.Data.SpecificRepositories;
using Template.Backend.Model.Entities;
using Template.Backend.Service.Services;
using Template.Backend.Service.Validation;

namespace Template.Backend.IntegrationTest
{
    [TestClass]
    public class EmployeeIntegrationTest
    {
        IEmployeeRepository employeeRepository;
        IUnitOfWork unitOfWork;
        IValidationDictionary validationDictionary;
        IEmployeeService employeeService;

        TestDatabaseFixture Fixture;
        const int SeedCount = 2;


        [TestInitialize]
        public void Initialize()
        {
            Fixture = new TestDatabaseFixture();
            Fixture.Initialize();

            // Seed one company for dependency
            var companies = Helper.CompanyFactory(1);
            Fixture.Seed(companies);

            var employees = Helper.EmployeeFactory(SeedCount);
            Fixture.Seed(employees);

           

            StarterDbContext dbContext = Fixture.CreateContext();
            employeeRepository = new EmployeeRepository(dbContext);
            unitOfWork = new UnitOfWork(dbContext);
            validationDictionary = new ValidationDictionary();
            employeeService = new EmployeeService(employeeRepository, unitOfWork, validationDictionary);
        }

        [TestMethod]
        public void Test_Employee_Count()
        {
            var count = employeeService.Count();
            Assert.AreEqual(SeedCount, count);
        }

        [TestMethod]
        public void Test_Employee_GetById()
        {
            Employee employee;
            int id = 1;
            employee = employeeService.GetById(id);
            Assert.AreEqual(id, employee.ID);
        }

        [TestMethod]
        public void Test_Employee_Add()
        {
            Assert.AreEqual(SeedCount, employeeService.Count());
            int countToAdd = 1;
            Employee employee = Helper.EmployeeFactory(countToAdd, "OtherName").First();
            employeeService.Add(employee);
            employeeService.Save();

            Assert.AreEqual(SeedCount + countToAdd, employeeService.Count());
        }

        [TestMethod]
        public void Test_Employee_AddRange()
        {
            Assert.AreEqual(SeedCount, employeeService.Count());

            int countToAdd = 3;
            var employees = Helper.EmployeeFactory(countToAdd, "OtherName");
            employeeService.AddRange(employees);
            employeeService.Save();

            Assert.AreEqual(SeedCount + countToAdd, employeeService.Count());
        }

        [TestMethod]
        public void Test_Employee_Delete_byId()
        {
            Assert.AreEqual(SeedCount, employeeService.Count());

            int idToDelete = 1;
            employeeService.Delete(idToDelete);
            employeeService.Save();

            Assert.AreEqual(SeedCount - 1, employeeService.Count());
        }

        [TestMethod]
        public void Test_Employee_Delete_byEntity()
        {
            Assert.AreEqual(SeedCount, employeeService.Count());

            Employee employeeToDelete = employeeService.Get(e=>true);
            employeeService.Delete(employeeToDelete);
            employeeService.Save();

            Assert.AreEqual(SeedCount - 1, employeeService.Count());
        }

        [TestMethod]
        public void Test_Employee_Delete_byWhereExpression()
        {
            Assert.AreEqual(SeedCount, employeeService.Count());

            Employee employeeToDelete = employeeService.Get(e => true);
            employeeService.Delete(e=>e.Name == employeeToDelete.Name);
            employeeService.Save();

            Assert.AreEqual(SeedCount - 1, employeeService.Count());
        }

        [TestMethod]
        public void Test_Employee_AddDuplicateName()
        {
            Assert.AreEqual(SeedCount, employeeService.Count());
            int countToAdd = 1;
            Employee employee = Helper.EmployeeFactory(countToAdd, "OtherName").First();
            employeeService.Add(employee);
            employeeService.Save();

            Assert.IsTrue(employeeService.GetValidationDictionary().IsValid());
            Assert.AreEqual(SeedCount + countToAdd, employeeService.Count());

            Employee employee2 = Helper.EmployeeFactory(countToAdd, "OtherName").First();
            employeeService.Add(employee2);
            employeeService.Save();

            Assert.IsFalse(employeeService.GetValidationDictionary().IsValid());
            Assert.AreEqual(SeedCount + countToAdd, employeeService.Count());
        }

        [TestMethod]
        public void Test_Employee_ValidationDictionary()
        {
            Assert.AreEqual(SeedCount, employeeService.Count());
            Assert.IsTrue(employeeService.GetValidationDictionary().ToDictionary().Count() == 0);
            Assert.IsTrue(employeeService.GetValidationDictionary().IsValid());

            Employee employeeToDuplicate = employeeService.Get(e => true);
            Employee newEmployee = new Employee { Name = employeeToDuplicate.Name, BirthDate = DateTime.Now, Address = "Address", CompanyID = 1};
            employeeService.Add(newEmployee);
            employeeService.Save();

            Assert.IsTrue(employeeService.GetValidationDictionary().ToDictionary().Count() > 0);
            Assert.IsFalse(employeeService.GetValidationDictionary().IsValid());
        }

        [TestMethod]
        public void Test_Employee_Update()
        {
            Employee employee  = employeeService.GetById(1);
            string newName = "New Name";

            Assert.AreNotEqual(newName, employee.Name);

            employee.Name = newName;
            employeeService.Update(employee);
            employeeService.Save();

            employee = employeeService.GetById(1);

            Assert.AreEqual(newName, employee.Name);
        }

        [TestMethod]
        public void Test_Employee_GetPagedList()
        {
            IEnumerable<Employee> employeeList = employeeService.GetPagedList(1, 3);
            Assert.AreEqual(SeedCount, employeeList.Count());

            employeeList = employeeService.GetPagedList(2, 3);
            Assert.AreEqual(0, employeeList.Count());

            employeeList = employeeService.GetPagedList(1, 1);
            Assert.AreEqual(1, employeeList.Count());

            employeeList = employeeService.GetPagedList(2, 1);
            Assert.AreEqual(1, employeeList.Count());
        }

        [TestMethod]
        public void Test_Employee_GetAll()
        {
            IEnumerable<Employee> employeeList = employeeService.GetAll();
            Assert.AreEqual(SeedCount, employeeList.Count());

            int countToAdd = 4;
            IEnumerable<Employee> employees = Helper.EmployeeFactory(countToAdd, "OtherName");
            employeeService.AddRange(employees);
            employeeService.Save();

            employeeList = employeeService.GetAll();
            Assert.AreEqual(SeedCount + countToAdd, employeeList.Count());
        }
    }
}