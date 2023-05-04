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

namespace Template.Backend.UnitTest
{
    /// <summary>
    /// Test DepartmentApiController
    /// </summary>
    [TestClass]
    public class TestDepartmentApiController
    {
        IDbFactory dbFactory;
        IDepartmentRepository departmentRepository;
        IUnitOfWork unitOfWork;
        IValidationDictionary validationDictionary;
        IDepartmentService departmentService;
        DepartmentApiController departmentApiController;
        AuditRepository<DepartmentAudit> departmentAuditRepository;
        IDepartmentAuditService departmentAuditService;

        /// <summary>
        /// Initialize Mocks and services
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new FakeDbFactory();
            departmentRepository = new DepartmentRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            validationDictionary = new ValidationDictionary();
            departmentService = new DepartmentService(departmentRepository, unitOfWork, validationDictionary);
            departmentAuditRepository = new AuditRepository<DepartmentAudit>(dbFactory);
            departmentAuditService = new DepartmentAuditService(departmentAuditRepository);

            departmentApiController = new DepartmentApiController(departmentService, departmentAuditService);

            departmentApiController.Request = new HttpRequestMessage();
            departmentApiController.Configuration = new HttpConfiguration();

            // AutoMapper;
            if (!AutoMapperConfig.IsInitialized)
            {
                AutoMapperConfig.Configure();
            }
        }

        /// <summary>
        /// GetPagedList when Department
        /// </summary>
        [TestMethod]
        public void Test_Department_GetPagedList()
        {
            IList<string> shortNameList = new[] { "TEST1", "TEST2" , "TEST3","TEST4","TEST5" ,
                                                    "TEST6","TEST7" , "TEST8"};

            foreach (var name in shortNameList)
            {
                DepartmentDto departmentDto = Helper.DepartmentDtoFactory(name);

                var actionResult2 = departmentApiController.Post(departmentDto) as ResponseMessageResult;
                Assert.IsTrue(actionResult2.Response.IsSuccessStatusCode);
            }

            var actionResult = departmentApiController.Get() as ResponseMessageResult;
            var list = Helper.DeserializeObject<IEnumerable<Department>>(actionResult);

            Assert.IsTrue(actionResult.Response.IsSuccessStatusCode);
            Assert.AreEqual(8, list.Count());

            actionResult = departmentApiController.GetPagedList(2, 3) as ResponseMessageResult;
            list = Helper.DeserializeObject<IEnumerable<Department>>(actionResult);

            Assert.IsTrue(actionResult.Response.IsSuccessStatusCode);
            Assert.AreEqual(3, list.Count());

            actionResult = departmentApiController.GetPagedList(3, 3) as ResponseMessageResult;
            list = Helper.DeserializeObject<IEnumerable<Department>>(actionResult);

            Assert.IsTrue(actionResult.Response.IsSuccessStatusCode);
            Assert.AreEqual(2, list.Count());
        }

        /// <summary>
        /// Test Department add and delete
        /// </summary>
        [TestMethod]
        public void Test_Department_Add_Delete()
        {
            ResponseMessageResult actionResult = (ResponseMessageResult)departmentApiController.Get();
            var contentResult = actionResult.Response.Content as ObjectContent<HttpError>;

            Assert.AreEqual("NoElementFoundException", actionResult.Response.ReasonPhrase);
            Assert.IsFalse(actionResult.Response.IsSuccessStatusCode);
            Assert.IsNotNull(contentResult);

            DepartmentDto departmentDto = Helper.DepartmentDtoFactory("TEST");

            var actionResult2 = departmentApiController.Post(departmentDto) as ResponseMessageResult;
            Assert.IsTrue(actionResult2.Response.IsSuccessStatusCode);

            var actionResult3 = departmentApiController.Get() as ResponseMessageResult;
            var list = Helper.DeserializeObject<IEnumerable<Department>>(actionResult3);

            Assert.IsTrue(actionResult3.Response.IsSuccessStatusCode);
            Assert.AreEqual(1, list.Count());

            list.FirstOrDefault().ID = 1;
            departmentRepository.Update(list.FirstOrDefault());

            var actionResult4 = departmentApiController.Delete(list.FirstOrDefault().ID);
            var contentResult4 = actionResult4 as StatusCodeResult;

            Assert.IsNotNull(contentResult4);
            Assert.AreEqual(HttpStatusCode.NoContent, contentResult4.StatusCode);
        }

        /// <summary>
        /// Test Department update
        /// </summary>
        [TestMethod]
        public void Test_Department_Update()
        {
            DepartmentDto departmentDto = Helper.DepartmentDtoFactory("TEST");
            var actionResult = departmentApiController.Post(departmentDto) as ResponseMessageResult;

            Assert.IsTrue(actionResult.Response.IsSuccessStatusCode);

            var actionResult2 = departmentApiController.Get() as ResponseMessageResult;
            var list = Helper.DeserializeObject<IEnumerable<Department>>(actionResult2);

            Assert.IsTrue(actionResult2.Response.IsSuccessStatusCode);
            Assert.AreEqual(1, list.Count());

            departmentDto.Name = "NEW_NAME";
            var actionResult3 = departmentApiController.Put(1, departmentDto) as OkResult;

            Assert.IsNotNull(actionResult3);
        }

        [TestMethod]
        public void Test_Department_Get()
        {
            var actionResult = departmentApiController.Get(0) as ResponseMessageResult;
            var contentResult = actionResult.Response.Content as ObjectContent<HttpError>;

            Assert.AreEqual("IdNotFoundException", actionResult.Response.ReasonPhrase);
            Assert.IsFalse(actionResult.Response.IsSuccessStatusCode);
            Assert.IsNotNull(contentResult);

            DepartmentDto departmentDto = Helper.DepartmentDtoFactory("TEST");

            var actionResult2 = departmentApiController.Post(departmentDto) as ResponseMessageResult;
            Assert.IsTrue(actionResult2.Response.IsSuccessStatusCode);

            var actionResult3 = departmentApiController.Get(1) as ResponseMessageResult;
            var department = Helper.DeserializeObject<Department>(actionResult3);

            Assert.IsNotNull(actionResult3);
            Assert.IsNotNull(department);
            Assert.AreEqual(1,department.ID);
        }
    }//CL
}//NS