using Template.Backend.Data;
using Template.Backend.Data.Repositories;
using Template.Backend.Data.SpecificRepositories;
using Template.Backend.Model.Entities;
using Template.Backend.Service.Services;
using Template.Backend.Service.Validation;

namespace Template.Backend.IntegrationTest
{
    [TestClass]
    public class CompanyIntegrationTest
    {
        ICompanyRepository companyRepository;
        IUnitOfWork unitOfWork;
        IValidationDictionary validationDictionary;
        ICompanyService companyService;

        TestDatabaseFixture Fixture;
        const int SeedCount = 2;


        [TestInitialize]
        public void Initialize()
        {
            Fixture = new TestDatabaseFixture();
            Fixture.Initialize();

            var companies = Helper.CompanyFactory(SeedCount);
            Fixture.Seed(companies);

            StarterDbContext dbContext = Fixture.CreateContext();
            companyRepository = new CompanyRepository(dbContext);
            unitOfWork = new UnitOfWork(dbContext);
            validationDictionary = new ValidationDictionary();
            companyService = new CompanyService(companyRepository, unitOfWork, validationDictionary);
        }

        [TestMethod]
        public void Test_Company_Count()
        {
            var count = companyService.Count();
            Assert.AreEqual(SeedCount, count);
        }

        [TestMethod]
        public void Test_Company_GetById()
        {
            Company company;
            int id = 1;
            company = companyService.GetById(id);
            Assert.AreEqual(id, company.ID);
        }

        [TestMethod]
        public void Test_Company_Add()
        {
            Assert.AreEqual(SeedCount, companyService.Count());
            int countToAdd = 1;
            Company company = Helper.CompanyFactory(countToAdd, "OtherName").First();
            companyService.Add(company);
            companyService.Save();

            Assert.AreEqual(SeedCount + countToAdd, companyService.Count());
        }

        [TestMethod]
        public void Test_Company_AddRange()
        {
            Assert.AreEqual(SeedCount, companyService.Count());

            int countToAdd = 3;
            var companies = Helper.CompanyFactory(countToAdd, "OtherName");
            companyService.AddRange(companies);
            companyService.Save();

            Assert.AreEqual(SeedCount + countToAdd, companyService.Count());
        }

        [TestMethod]
        public void Test_Company_Delete_byId()
        {
            Assert.AreEqual(SeedCount, companyService.Count());

            int idToDelete = 1;
            companyService.Delete(idToDelete);
            companyService.Save();

            Assert.AreEqual(SeedCount - 1, companyService.Count());
        }

        [TestMethod]
        public void Test_Company_Delete_byEntity()
        {
            Assert.AreEqual(SeedCount, companyService.Count());

            Company companyToDelete = companyService.Get(e=>true);
            companyService.Delete(companyToDelete);
            companyService.Save();

            Assert.AreEqual(SeedCount - 1, companyService.Count());
        }

        [TestMethod]
        public void Test_Company_Delete_byWhereExpression()
        {
            Assert.AreEqual(SeedCount, companyService.Count());

            Company companyToDelete = companyService.Get(e => true);
            companyService.Delete(e=>e.Name == companyToDelete.Name);
            companyService.Save();

            Assert.AreEqual(SeedCount - 1, companyService.Count());
        }

        [TestMethod]
        public void Test_Company_AddDuplicateName()
        {
            Assert.AreEqual(SeedCount, companyService.Count());
            int countToAdd = 1;
            Company company = Helper.CompanyFactory(countToAdd, "OtherName").First();
            companyService.Add(company);
            companyService.Save();

            Assert.IsTrue(companyService.GetValidationDictionary().IsValid());
            Assert.AreEqual(SeedCount + countToAdd, companyService.Count());

            Company company2 = Helper.CompanyFactory(countToAdd, "OtherName").First();
            companyService.Add(company2);
            companyService.Save();

            Assert.IsFalse(companyService.GetValidationDictionary().IsValid());
            Assert.AreEqual(SeedCount + countToAdd, companyService.Count());
        }

        [TestMethod]
        public void Test_Company_ValidationDictionary()
        {
            Assert.AreEqual(SeedCount, companyService.Count());
            Assert.IsTrue(companyService.GetValidationDictionary().ToDictionary().Count() == 0);
            Assert.IsTrue(companyService.GetValidationDictionary().IsValid());

            Company companyToDuplicate = companyService.Get(e => true);
            Company newCompany = new Company { Name = companyToDuplicate.Name, CreationDate = DateTime.Now};
            companyService.Add(newCompany);
            companyService.Save();

            Assert.IsTrue(companyService.GetValidationDictionary().ToDictionary().Count() > 0);
            Assert.IsFalse(companyService.GetValidationDictionary().IsValid());
        }

        [TestMethod]
        public void Test_Company_Update()
        {
            Company company  = companyService.GetById(1);
            string newName = "New Name";

            Assert.AreNotEqual(newName, company.Name);

            company.Name = newName;
            companyService.Update(company);
            companyService.Save();

            company = companyService.GetById(1);

            Assert.AreEqual(newName, company.Name);
        }

        [TestMethod]
        public void Test_Company_GetPagedList()
        {
            IEnumerable<Company> companyList = companyService.GetPagedList(1, 3);
            Assert.AreEqual(SeedCount, companyList.Count());

            companyList = companyService.GetPagedList(2, 3);
            Assert.AreEqual(0, companyList.Count());

            companyList = companyService.GetPagedList(1, 1);
            Assert.AreEqual(1, companyList.Count());

            companyList = companyService.GetPagedList(2, 1);
            Assert.AreEqual(1, companyList.Count());
        }

        [TestMethod]
        public void Test_Company_GetAll()
        {
            IEnumerable<Company> companyList = companyService.GetAll();
            Assert.AreEqual(SeedCount, companyList.Count());

            int countToAdd = 4;
            IEnumerable<Company> companies = Helper.CompanyFactory(countToAdd, "OtherName");
            companyService.AddRange(companies);
            companyService.Save();

            companyList = companyService.GetAll();
            Assert.AreEqual(SeedCount + countToAdd, companyList.Count());
        }
    }
}