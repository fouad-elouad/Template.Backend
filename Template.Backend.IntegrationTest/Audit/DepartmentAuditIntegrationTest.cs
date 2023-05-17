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
    public class DepartmentAuditIntegrationTest
    {

        IDepartmentRepository departmentRepository;
        IAuditRepository<DepartmentAudit> departmentAuditRepository;
        IUnitOfWork unitOfWork;
        IValidationDictionary validationDictionary;
        IDepartmentService departmentService;
        IDepartmentAuditService departmentAuditService;

        TestDatabaseFixture Fixture;
        const int SeedCount = 2;


        [TestInitialize]
        public void Initialize()
        {
            Fixture = new TestDatabaseFixture();
            Fixture.Initialize();

            var departments = Helper.DepartmentFactory(SeedCount);
            Fixture.Seed(departments);

            StarterDbContext dbContext = Fixture.CreateContext();
            departmentRepository = new DepartmentRepository(dbContext);
            unitOfWork = new UnitOfWork(dbContext);
            validationDictionary = new ValidationDictionary();
            departmentService = new DepartmentService(departmentRepository, unitOfWork, validationDictionary);
            departmentAuditRepository = new AuditRepository<DepartmentAudit>(dbContext);
            departmentAuditService = new DepartmentAuditService(departmentAuditRepository);
        }

        [TestMethod]
        public void Test_DepartmentAudit_GetAuditById()
        {
            Department department = Helper.DepartmentFactory(1, "OtherName").First();
            departmentService.Add(department);
            departmentService.Save();
            var list = departmentAuditService.GetAuditById(department.ID);
            Assert.AreEqual(1, list.Count());

            department.Name = "NewName";
            departmentService.Update(department);
            departmentService.Save();
            list = departmentAuditService.GetAuditById(department.ID);
            Assert.AreEqual(2, list.Count());

            department.Name = "OtherNewName";
            departmentService.Update(department);
            departmentService.Save();
            list = departmentAuditService.GetAuditById(department.ID);
            Assert.AreEqual(3, list.Count());
        }

        [TestMethod]
        public void Test_DepartmentAudit_GetById()
        {
            Department department = Helper.DepartmentFactory(1, "OtherName").First();
            departmentService.Add(department);
            departmentService.Save();
            var list = departmentAuditService.GetAuditById(department.ID);
            DepartmentAudit departmentAudit = departmentAuditService.GetById(list.FirstOrDefault().DepartmentAuditID);
            Assert.AreEqual(departmentAudit.ID, department.ID);
            Assert.IsNotNull(departmentAudit);
        }
    }
}
