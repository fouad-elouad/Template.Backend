using Template.Backend.Data;
using Template.Backend.Data.Audit;
using Template.Backend.Data.Repositories;
using Template.Backend.Data.SpecificRepositories;
using Template.Backend.Model.Audit.Entities;
using Template.Backend.Model.Entities;
using Template.Backend.Service.Audit;
using Template.Backend.Service.Services;
using Template.Backend.Service.Validation;

namespace Template.Backend.IntegrationTest.Audit
{

    [TestClass]
    public class EmployeeAuditIntegrationTest
    {

        IEmployeeRepository employeeRepository;
        IAuditRepository<EmployeeAudit> employeeAuditRepository;
        IUnitOfWork unitOfWork;
        IValidationDictionary validationDictionary;
        IEmployeeService employeeService;
        IEmployeeAuditService employeeAuditService;

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
            employeeAuditRepository = new AuditRepository<EmployeeAudit>(dbContext);
            employeeAuditService = new EmployeeAuditService(employeeAuditRepository);
        }

        [TestMethod]
        public void Test_EmployeeAudit_GetAuditById()
        {
            Employee employee = Helper.EmployeeFactory(1, "OtherName").First();
            employeeService.Add(employee);
            employeeService.Save();
            var list = employeeAuditService.GetAuditById(employee.ID);
            Assert.AreEqual(1, list.Count());

            employee.Name = "NewName";
            employeeService.Update(employee);
            employeeService.Save();
            list = employeeAuditService.GetAuditById(employee.ID);
            Assert.AreEqual(2, list.Count());

            employee.Name = "OtherNewName";
            employeeService.Update(employee);
            employeeService.Save();
            list = employeeAuditService.GetAuditById(employee.ID);
            Assert.AreEqual(3, list.Count());
        }

        [TestMethod]
        public void Test_EmployeeAudit_GetById()
        {
            Employee employee = Helper.EmployeeFactory(1, "OtherName").First();
            employeeService.Add(employee);
            employeeService.Save();
            var list = employeeAuditService.GetAuditById(employee.ID);
            EmployeeAudit employeeAudit = employeeAuditService.GetById(list.FirstOrDefault().EmployeeAuditID);
            Assert.AreEqual(employeeAudit.ID, employee.ID);
            Assert.IsNotNull(employeeAudit);
        }
    }
}
