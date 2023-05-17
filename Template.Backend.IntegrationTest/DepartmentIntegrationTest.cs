using Template.Backend.Data;
using Template.Backend.Data.Repositories;
using Template.Backend.Data.SpecificRepositories;
using Template.Backend.Model.Entities;
using Template.Backend.Service.Services;
using Template.Backend.Service.Validation;

namespace Template.Backend.IntegrationTest
{
    [TestClass]
    public class DepartmentIntegrationTest
    {
        IDepartmentRepository departmentRepository;
        IUnitOfWork unitOfWork;
        IValidationDictionary validationDictionary;
        IDepartmentService departmentService;

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
        }

        [TestMethod]
        public void Test_Department_Count()
        {
            var count = departmentService.Count();
            Assert.AreEqual(SeedCount, count);
        }

        [TestMethod]
        public void Test_Department_GetById()
        {
            Department department;
            int id = 1;
            department = departmentService.GetById(id);
            Assert.AreEqual(id, department.ID);
        }

        [TestMethod]
        public void Test_Department_Add()
        {
            Assert.AreEqual(SeedCount, departmentService.Count());
            int countToAdd = 1;
            Department department = Helper.DepartmentFactory(countToAdd, "OtherName").First();
            departmentService.Add(department);
            departmentService.Save();

            Assert.AreEqual(SeedCount + countToAdd, departmentService.Count());
        }

        [TestMethod]
        public void Test_Department_AddRange()
        {
            Assert.AreEqual(SeedCount, departmentService.Count());

            int countToAdd = 3;
            var departments = Helper.DepartmentFactory(countToAdd, "OtherName");
            departmentService.AddRange(departments);
            departmentService.Save();

            Assert.AreEqual(SeedCount + countToAdd, departmentService.Count());
        }

        [TestMethod]
        public void Test_Department_Delete_byId()
        {
            Assert.AreEqual(SeedCount, departmentService.Count());

            int idToDelete = 1;
            departmentService.Delete(idToDelete);
            departmentService.Save();

            Assert.AreEqual(SeedCount - 1, departmentService.Count());
        }

        [TestMethod]
        public void Test_Department_Delete_byEntity()
        {
            Assert.AreEqual(SeedCount, departmentService.Count());

            Department departmentToDelete = departmentService.Get(e=>true);
            departmentService.Delete(departmentToDelete);
            departmentService.Save();

            Assert.AreEqual(SeedCount - 1, departmentService.Count());
        }

        [TestMethod]
        public void Test_Department_Delete_byWhereExpression()
        {
            Assert.AreEqual(SeedCount, departmentService.Count());

            Department departmentToDelete = departmentService.Get(e => true);
            departmentService.Delete(e=>e.Name == departmentToDelete.Name);
            departmentService.Save();

            Assert.AreEqual(SeedCount - 1, departmentService.Count());
        }

        [TestMethod]
        public void Test_Department_AddDuplicateName()
        {
            Assert.AreEqual(SeedCount, departmentService.Count());
            int countToAdd = 1;
            Department department = Helper.DepartmentFactory(countToAdd, "OtherName").First();
            departmentService.Add(department);
            departmentService.Save();

            Assert.IsTrue(departmentService.GetValidationDictionary().IsValid());
            Assert.AreEqual(SeedCount + countToAdd, departmentService.Count());

            Department department2 = Helper.DepartmentFactory(countToAdd, "OtherName").First();
            departmentService.Add(department2);
            departmentService.Save();

            Assert.IsFalse(departmentService.GetValidationDictionary().IsValid());
            Assert.AreEqual(SeedCount + countToAdd, departmentService.Count());
        }

        [TestMethod]
        public void Test_Department_ValidationDictionary()
        {
            Assert.AreEqual(SeedCount, departmentService.Count());
            Assert.IsTrue(departmentService.GetValidationDictionary().ToDictionary().Count() == 0);
            Assert.IsTrue(departmentService.GetValidationDictionary().IsValid());

            Department departmentToDuplicate = departmentService.Get(e => true);
            Department newDepartment = new Department { Name = departmentToDuplicate.Name};
            departmentService.Add(newDepartment);
            departmentService.Save();

            Assert.IsTrue(departmentService.GetValidationDictionary().ToDictionary().Count() > 0);
            Assert.IsFalse(departmentService.GetValidationDictionary().IsValid());
        }

        [TestMethod]
        public void Test_Department_Update()
        {
            Department department  = departmentService.GetById(1);
            string newName = "New Name";

            Assert.AreNotEqual(newName, department.Name);

            department.Name = newName;
            departmentService.Update(department);
            departmentService.Save();

            department = departmentService.GetById(1);

            Assert.AreEqual(newName, department.Name);
        }

        [TestMethod]
        public void Test_Department_GetPagedList()
        {
            IEnumerable<Department> departmentList = departmentService.GetPagedList(1, 3);
            Assert.AreEqual(SeedCount, departmentList.Count());

            departmentList = departmentService.GetPagedList(2, 3);
            Assert.AreEqual(0, departmentList.Count());

            departmentList = departmentService.GetPagedList(1, 1);
            Assert.AreEqual(1, departmentList.Count());

            departmentList = departmentService.GetPagedList(2, 1);
            Assert.AreEqual(1, departmentList.Count());
        }

        [TestMethod]
        public void Test_Department_GetAll()
        {
            IEnumerable<Department> departmentList = departmentService.GetAll();
            Assert.AreEqual(SeedCount, departmentList.Count());

            int countToAdd = 4;
            IEnumerable<Department> departments = Helper.DepartmentFactory(countToAdd, "OtherName");
            departmentService.AddRange(departments);
            departmentService.Save();

            departmentList = departmentService.GetAll();
            Assert.AreEqual(SeedCount + countToAdd, departmentList.Count());
        }
    }
}