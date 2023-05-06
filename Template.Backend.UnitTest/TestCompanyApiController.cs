using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Collections.Generic;
using System.Net;
using Template.Backend.Data.Repositories;
using Template.Backend.Data.SpecificRepositories;
using Template.Backend.Service.Validation;
using Template.Backend.Service.Services;
using Template.Backend.Api.Controllers;
using Template.Backend.Model.Audit.Entities;
using Template.Backend.Data.Audit;
using Template.Backend.Service.Audit;
using Template.Backend.UnitTest.Configuration;
using Template.Backend.Api.Models;
using Template.Backend.Model.Entities;
using Template.Backend.Api;
using AutoMapper;

namespace Template.Backend.UnitTest
{
    /// <summary>
    /// Test CompanyApiController
    /// </summary>
    [TestClass]
    public class TestCompanyApiController
    {
        IDbFactory dbFactory;
        ICompanyRepository companyRepository;
        IUnitOfWork unitOfWork;
        IValidationDictionary validationDictionary;
        ICompanyService companyService;
        CompanyApiController companyApiController;
        AuditRepository<CompanyAudit> companyAuditRepository;
        ICompanyAuditService companyAuditService;
        IMapper mapper;

        /// <summary>
        /// Initialize Mocks and services
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new FakeDbFactory();
            companyRepository = new CompanyRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            validationDictionary = new ValidationDictionary();
            companyService = new CompanyService(companyRepository, unitOfWork, validationDictionary);
            companyAuditRepository = new AuditRepository<CompanyAudit>(dbFactory);
            companyAuditService = new CompanyAuditService(companyAuditRepository);

            // AutoMapper;
            //if (!AutoMapperConfig.IsInitialized)
            //{
                mapper = AutoMapperConfig.Initialize();
            //}

            companyApiController = new CompanyApiController(companyService, companyAuditService, mapper);

            companyApiController.Request = new HttpRequestMessage();
            companyApiController.Configuration = new HttpConfiguration();

            
        }

        /// <summary>
        /// GetPagedList when Company
        /// </summary>
        [TestMethod]
        public void Test_Company_GetPagedList()
        {
            IList<string> shortNameList = new[] { "TEST1", "TEST2" , "TEST3","TEST4","TEST5" ,
                                                    "TEST6","TEST7" , "TEST8"};

            foreach (var name in shortNameList)
            {
                CompanyDto companyDto = Helper.CompanyDtoFactory(name);

                var actionResult2 = companyApiController.Post(companyDto) as ResponseMessageResult;
                Assert.IsTrue(actionResult2.Response.IsSuccessStatusCode);
            }

            var actionResult = companyApiController.Get() as ResponseMessageResult;
            var list = Helper.DeserializeObject<IEnumerable<Company>>(actionResult);

            Assert.IsTrue(actionResult.Response.IsSuccessStatusCode);
            Assert.AreEqual(8, list.Count());

            actionResult = companyApiController.GetPagedList(2, 3) as ResponseMessageResult;
            list = Helper.DeserializeObject<IEnumerable<Company>>(actionResult);

            Assert.IsTrue(actionResult.Response.IsSuccessStatusCode);
            Assert.AreEqual(3, list.Count());

            actionResult = companyApiController.GetPagedList(3, 3) as ResponseMessageResult;
            list = Helper.DeserializeObject<IEnumerable<Company>>(actionResult);

            Assert.IsTrue(actionResult.Response.IsSuccessStatusCode);
            Assert.AreEqual(2, list.Count());
        }

        /// <summary>
        /// Test Company add and delete
        /// </summary>
        [TestMethod]
        public void Test_Company_Add_Delete()
        {
            ResponseMessageResult actionResult = (ResponseMessageResult)companyApiController.Get();
            var contentResult = actionResult.Response.Content as ObjectContent<HttpError>;

            Assert.AreEqual("NoElementFoundException", actionResult.Response.ReasonPhrase);
            Assert.IsFalse(actionResult.Response.IsSuccessStatusCode);
            Assert.IsNotNull(contentResult);

            CompanyDto companyDto = Helper.CompanyDtoFactory("TEST");

            var actionResult2 = companyApiController.Post(companyDto) as ResponseMessageResult;
            Assert.IsTrue(actionResult2.Response.IsSuccessStatusCode);

            var actionResult3 = companyApiController.Get() as ResponseMessageResult;
            var list = Helper.DeserializeObject<IEnumerable<Company>>(actionResult3);

            Assert.IsTrue(actionResult3.Response.IsSuccessStatusCode);
            Assert.AreEqual(1, list.Count());

            list.FirstOrDefault().ID = 1;
            companyRepository.Update(list.FirstOrDefault());

            var actionResult4 = companyApiController.Delete(list.FirstOrDefault().ID);
            var contentResult4 = actionResult4 as StatusCodeResult;

            Assert.IsNotNull(contentResult4);
            Assert.AreEqual(HttpStatusCode.NoContent, contentResult4.StatusCode);
        }

        /// <summary>
        /// Test Company update
        /// </summary>
        [TestMethod]
        public void Test_Company_Update()
        {
            CompanyDto companyDto = Helper.CompanyDtoFactory("TEST");
            var actionResult = companyApiController.Post(companyDto) as ResponseMessageResult;

            Assert.IsTrue(actionResult.Response.IsSuccessStatusCode);

            var actionResult2 = companyApiController.Get() as ResponseMessageResult;
            var list = Helper.DeserializeObject<IEnumerable<Company>>(actionResult2);

            Assert.IsTrue(actionResult2.Response.IsSuccessStatusCode);
            Assert.AreEqual(1, list.Count());

            companyDto.Name = "NEW_NAME";
            var actionResult3 = companyApiController.Put(1, companyDto) as OkResult;

            Assert.IsNotNull(actionResult3);
        }

        [TestMethod]
        public void Test_Company_Get()
        {
            var actionResult = companyApiController.Get(0) as ResponseMessageResult;
            var contentResult = actionResult.Response.Content as ObjectContent<HttpError>;

            Assert.AreEqual("IdNotFoundException", actionResult.Response.ReasonPhrase);
            Assert.IsFalse(actionResult.Response.IsSuccessStatusCode);
            Assert.IsNotNull(contentResult);

            CompanyDto companyDto = Helper.CompanyDtoFactory("TEST");

            var actionResult2 = companyApiController.Post(companyDto) as ResponseMessageResult;
            Assert.IsTrue(actionResult2.Response.IsSuccessStatusCode);

            var actionResult3 = companyApiController.Get(1) as ResponseMessageResult;
            var company = Helper.DeserializeObject<Company>(actionResult3);

            Assert.IsNotNull(actionResult3);
            Assert.IsNotNull(company);
            Assert.AreEqual(1,company.ID);
        }
    }//CL
}//NS