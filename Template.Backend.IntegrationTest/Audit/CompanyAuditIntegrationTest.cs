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
    public class CompanyAuditIntegrationTest
    {

        ICompanyRepository companyRepository;
        IAuditRepository<CompanyAudit> companyAuditRepository;
        IUnitOfWork unitOfWork;
        IValidationDictionary validationDictionary;
        ICompanyService companyService;
        ICompanyAuditService companyAuditService;

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
            companyAuditRepository = new AuditRepository<CompanyAudit>(dbContext);
            companyAuditService = new CompanyAuditService(companyAuditRepository);
        }

        [TestMethod]
        public void Test_CompanyAudit_GetAuditById()
        {
            Company company = Helper.CompanyFactory(1, "OtherName").First();
            companyService.Add(company);
            companyService.Save();
            var list = companyAuditService.GetAuditById(company.ID);
            Assert.AreEqual(1, list.Count());

            company.Name = "NewName";
            companyService.Update(company);
            companyService.Save();
            list = companyAuditService.GetAuditById(company.ID);
            Assert.AreEqual(2, list.Count());

            company.Name = "OtherNewName";
            companyService.Update(company);
            companyService.Save();
            list = companyAuditService.GetAuditById(company.ID);
            Assert.AreEqual(3, list.Count());
        }

        [TestMethod]
        public void Test_CompanyAudit_GetById()
        {
            Company company = Helper.CompanyFactory(1, "OtherName").First();
            companyService.Add(company);
            companyService.Save();
            var list = companyAuditService.GetAuditById(company.ID);
            CompanyAudit companyAudit = companyAuditService.GetById(list.FirstOrDefault().CompanyAuditID);
            Assert.AreEqual(companyAudit.ID, company.ID);
            Assert.IsNotNull(companyAudit);
        }
    }
}
