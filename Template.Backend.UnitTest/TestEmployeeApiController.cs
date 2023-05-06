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
    /// Test EmployeeApiController
    /// </summary>
    [TestClass]
    public class TestEmployeeApiController
    {
        IDbFactory dbFactory;
        IEmployeeRepository employeeRepository;
        IUnitOfWork unitOfWork;
        IValidationDictionary validationDictionary;
        IEmployeeService employeeService;
        EmployeeApiController employeeApiController;
        AuditRepository<EmployeeAudit> employeeAuditRepository;
        IEmployeeAuditService employeeAuditService;
        IMapper mapper;

        /// <summary>
        /// Initialize Mocks and services
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new FakeDbFactory();
            employeeRepository = new EmployeeRepository(dbFactory);
            unitOfWork = new UnitOfWork(dbFactory);
            validationDictionary = new ValidationDictionary();
            employeeService = new EmployeeService(employeeRepository, unitOfWork, validationDictionary);
            employeeAuditRepository = new AuditRepository<EmployeeAudit>(dbFactory);
            employeeAuditService = new EmployeeAuditService(employeeAuditRepository);

            mapper = AutoMapperConfig.Initialize();

            employeeApiController = new EmployeeApiController(employeeService, employeeAuditService, mapper);

            employeeApiController.Request = new HttpRequestMessage();
            employeeApiController.Configuration = new HttpConfiguration();
        }

        /// <summary>
        /// GetPagedList when Employee
        /// </summary>
        [TestMethod]
        public void Test_Employee_GetPagedList()
        {
            IList<string> shortNameList = new[] { "TEST1", "TEST2" , "TEST3","TEST4","TEST5" ,
                                                    "TEST6","TEST7" , "TEST8"};

            foreach (var name in shortNameList)
            {
                EmployeeDto employeeDto = Helper.EmployeeDtoFactory(name, 1);

                var actionResult2 = employeeApiController.Post(employeeDto) as ResponseMessageResult;
                Assert.IsTrue(actionResult2.Response.IsSuccessStatusCode);
            }

            var actionResult = employeeApiController.Get() as ResponseMessageResult;
            var list = Helper.DeserializeObject<IEnumerable<Employee>>(actionResult);

            Assert.IsTrue(actionResult.Response.IsSuccessStatusCode);
            Assert.AreEqual(8, list.Count());

            actionResult = employeeApiController.GetPagedList(2, 3) as ResponseMessageResult;
            list = Helper.DeserializeObject<IEnumerable<Employee>>(actionResult);

            Assert.IsTrue(actionResult.Response.IsSuccessStatusCode);
            Assert.AreEqual(3, list.Count());

            actionResult = employeeApiController.GetPagedList(3, 3) as ResponseMessageResult;
            list = Helper.DeserializeObject<IEnumerable<Employee>>(actionResult);

            Assert.IsTrue(actionResult.Response.IsSuccessStatusCode);
            Assert.AreEqual(2, list.Count());
        }

        /// <summary>
        /// Test Employee add and delete
        /// </summary>
        [TestMethod]
        public void Test_Employee_Add_Delete()
        {
            ResponseMessageResult actionResult = (ResponseMessageResult)employeeApiController.Get();
            var contentResult = actionResult.Response.Content as ObjectContent<HttpError>;

            Assert.AreEqual("NoElementFoundException", actionResult.Response.ReasonPhrase);
            Assert.IsFalse(actionResult.Response.IsSuccessStatusCode);
            Assert.IsNotNull(contentResult);

            EmployeeDto employeeDto = Helper.EmployeeDtoFactory("TEST", 1);

            var actionResult2 = employeeApiController.Post(employeeDto) as ResponseMessageResult;
            Assert.IsTrue(actionResult2.Response.IsSuccessStatusCode);

            var actionResult3 = employeeApiController.Get() as ResponseMessageResult;
            var list = Helper.DeserializeObject<IEnumerable<Employee>>(actionResult3);

            Assert.IsTrue(actionResult3.Response.IsSuccessStatusCode);
            Assert.AreEqual(1, list.Count());

            list.FirstOrDefault().ID = 1;
            employeeRepository.Update(list.FirstOrDefault());

            var actionResult4 = employeeApiController.Delete(list.FirstOrDefault().ID);
            var contentResult4 = actionResult4 as StatusCodeResult;

            Assert.IsNotNull(contentResult4);
            Assert.AreEqual(HttpStatusCode.NoContent, contentResult4.StatusCode);
        }

        /// <summary>
        /// Test Employee update
        /// </summary>
        [TestMethod]
        public void Test_Employee_Update()
        {
            EmployeeDto employeeDto = Helper.EmployeeDtoFactory("TEST", 1);
            var actionResult = employeeApiController.Post(employeeDto) as ResponseMessageResult;

            Assert.IsTrue(actionResult.Response.IsSuccessStatusCode);

            var actionResult2 = employeeApiController.Get() as ResponseMessageResult;
            var list = Helper.DeserializeObject<IEnumerable<Employee>>(actionResult2);

            Assert.IsTrue(actionResult2.Response.IsSuccessStatusCode);
            Assert.AreEqual(1, list.Count());

            employeeDto.Name = "NEW_NAME";
            var actionResult3 = employeeApiController.Put(1, employeeDto) as OkResult;

            Assert.IsNotNull(actionResult3);
        }

        [TestMethod]
        public void Test_Employee_Get()
        {
            var actionResult = employeeApiController.Get(0) as ResponseMessageResult;
            var contentResult = actionResult.Response.Content as ObjectContent<HttpError>;

            Assert.AreEqual("IdNotFoundException", actionResult.Response.ReasonPhrase);
            Assert.IsFalse(actionResult.Response.IsSuccessStatusCode);
            Assert.IsNotNull(contentResult);

            EmployeeDto employeeDto = Helper.EmployeeDtoFactory("TEST", 1);

            var actionResult2 = employeeApiController.Post(employeeDto) as ResponseMessageResult;
            Assert.IsTrue(actionResult2.Response.IsSuccessStatusCode);

            var actionResult3 = employeeApiController.Get(1) as ResponseMessageResult;
            var employee = Helper.DeserializeObject<Employee>(actionResult3);

            Assert.IsNotNull(actionResult3);
            Assert.IsNotNull(employee);
            Assert.AreEqual(1, employee.ID);
        }
    }//CL
}//NS