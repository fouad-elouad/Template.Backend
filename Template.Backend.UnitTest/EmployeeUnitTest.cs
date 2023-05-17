using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Template.Backend.Api.Controllers;
using Template.Backend.Api.Models;
using Template.Backend.Data.Audit;
using Template.Backend.Data.Repositories;
using Template.Backend.Data.SpecificRepositories;
using Template.Backend.Model.Audit.Entities;
using Template.Backend.Model.Entities;
using Template.Backend.Model.Exceptions;
using Template.Backend.Service.Audit;
using Template.Backend.Service.Services;
using Template.Backend.Service.Validation;
using System.Linq.Expressions;

namespace Template.Backend.UnitTest
{
    [TestClass]
    public class EmployeeUnitTest
    {
        EmployeeApiController employeeApiController;
        IServiceCollection services;
        Mock<IUnitOfWork> _uowMock;
        Mock<IEmployeeRepository> _employeeRepositoryMock;


        [TestInitialize]
        public void Initialize()
        {
            services = new ServiceCollection();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IEmployeeAuditService, EmployeeAuditService>();

            var logger = new Mock<ILogger<Employee>>();
            services.AddScoped(typeof(ILogger<Employee>), s => logger.Object);

            _uowMock = new Mock<IUnitOfWork>();
            services.AddScoped(typeof(IUnitOfWork), s => _uowMock.Object);

            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            services.AddScoped(typeof(IEmployeeRepository), s => _employeeRepositoryMock.Object);

            var employeeAuditRepositoryMock = new Mock<IAuditRepository<EmployeeAudit>>();
            services.AddScoped(typeof(IAuditRepository<EmployeeAudit>), s => employeeAuditRepositoryMock.Object);

            services.AddTransient<IValidationDictionary, ValidationDictionary>();
            services.AddTransient<EmployeeApiController, EmployeeApiController>();
            services.AddScoped(typeof(IAuditRepository<>), typeof(AuditRepository<>));

            services.AddAutoMapper(typeof(EmployeeUnitTest), typeof(EmployeeApiController));
        }

        [TestMethod]
        public void Test_Employee_GetPagedList()
        {
            var serviceProvider = services.BuildServiceProvider();
            employeeApiController = serviceProvider.GetService<EmployeeApiController>()!;

            _employeeRepositoryMock.Setup(cr => cr.GetPagedList(It.IsAny<int>(), It.IsAny<int>())).Returns(Helper.EmployeeFactory(2)).Verifiable();

            var actionResult = employeeApiController.GetPagedList(1, 2) as ObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.IsInstanceOfType(actionResult.Value, typeof(IEnumerable<Employee>));

            var result = actionResult.Value as IEnumerable<Employee>;
            Assert.IsTrue(result?.Count() == 2);
            Assert.IsTrue(result.ElementAt(0).ID == 1);
            Assert.IsTrue(result.ElementAt(1).ID == 2);

            _employeeRepositoryMock.Setup(cr => cr.GetPagedList(It.IsAny<int>(), It.IsAny<int>())).Returns(Helper.EmployeeFactory(0)).Verifiable();

            // Action
            Action act = () => employeeApiController.GetPagedList(1, 2);
            Assert.ThrowsException<NoElementFoundException>(act);

            Mock.Verify(_employeeRepositoryMock);
        }

        [TestMethod]
        public void Test_Employee_Delete()
        {
            var serviceProvider = services.BuildServiceProvider();
            employeeApiController = serviceProvider.GetService<EmployeeApiController>()!;

            _employeeRepositoryMock.Setup(cr => cr.Delete(It.IsAny<int>())).Verifiable();

            var actionResult = employeeApiController.Delete(1) as NoContentResult;
            Assert.IsNotNull(actionResult);

            Mock.Verify(_employeeRepositoryMock);
        }

        [TestMethod]
        [ExpectedException(typeof(IdNotFoundException))]
        public void Test_Employee_Delete_Exception()
        {
            var serviceProvider = services.BuildServiceProvider();
            employeeApiController = serviceProvider.GetService<EmployeeApiController>()!;

            _employeeRepositoryMock.Setup(cr => cr.Delete(It.IsAny<int>())).Throws(new IdNotFoundException("")).Verifiable();

            var actionResult = employeeApiController.Delete(1);
        }

        [TestMethod]
        public void Test_Employee_Post()
        {
            var serviceProvider = services.BuildServiceProvider();
            employeeApiController = serviceProvider.GetService<EmployeeApiController>()!;

            Employee employee = Helper.EmployeeFactory(1).First();
            _employeeRepositoryMock.Setup(cr => cr.Add(It.IsAny<Employee>())).Callback<Employee>(Helper.IncrementRowVersion).Verifiable();
            _employeeRepositoryMock.Setup(cr => cr.CheckIsUnique(It.IsAny<string>(), It.IsAny<int>())).Returns(true).Verifiable();

            var actionResult = employeeApiController.Post(Helper.EmployeeDtoFactory(1).First()) as ObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(CreatedAtActionResult));
            Assert.IsInstanceOfType(actionResult.Value, typeof(Employee));

            var result = actionResult.Value as Employee;
            Assert.IsTrue(result?.RowVersion == 1);

            Mock.Verify(_employeeRepositoryMock);
        }

        [TestMethod]
        public void Test_Employee_Update()
        {
            var serviceProvider = services.BuildServiceProvider();
            employeeApiController = serviceProvider.GetService<EmployeeApiController>()!;

            _employeeRepositoryMock.Setup(cr => cr.Update(It.IsAny<Employee>())).Callback<Employee>(Helper.IncrementRowVersion).Verifiable();
            _employeeRepositoryMock.Setup(cr => cr.CheckIfExist(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(true).Verifiable();
            _employeeRepositoryMock.Setup(cr => cr.CheckIsUnique(It.IsAny<string>(), It.IsAny<int>())).Returns(true).Verifiable();

            EmployeeDto employeeDto = Helper.EmployeeDtoFactory(1).First();
            var actionResult = employeeApiController.Put(employeeDto.ID, employeeDto) as StatusCodeResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));

            Mock.Verify(_employeeRepositoryMock);
        }

        [TestMethod]
        [ExpectedException(typeof(IdNotFoundException))]
        public void Test_Employee_Update_Exception()
        {
            var serviceProvider = services.BuildServiceProvider();
            employeeApiController = serviceProvider.GetService<EmployeeApiController>()!;

            _employeeRepositoryMock.Setup(cr => cr.Update(It.IsAny<Employee>())).Callback<Employee>(Helper.IncrementRowVersion).Verifiable();
            _employeeRepositoryMock.Setup(cr => cr.CheckIfExist(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(false).Verifiable();
            _employeeRepositoryMock.Setup(cr => cr.CheckIsUnique(It.IsAny<string>(), It.IsAny<int>())).Returns(true).Verifiable();

            EmployeeDto employeeDto = Helper.EmployeeDtoFactory(1).First();
            var actionResult = employeeApiController.Put(employeeDto.ID, employeeDto) as StatusCodeResult;

            Mock.Verify(_employeeRepositoryMock);
        }

        [TestMethod]
        public void Test_Employee_Get()
        {
            var serviceProvider = services.BuildServiceProvider();
            employeeApiController = serviceProvider.GetService<EmployeeApiController>()!;

            _employeeRepositoryMock.Setup(cr => cr.GetById(It.IsAny<int>())).Returns(Helper.EmployeeFactory(1).First()).Verifiable();

            var actionResult = employeeApiController.Get(1) as ObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.IsInstanceOfType(actionResult.Value, typeof(Employee));

            var result = actionResult.Value as Employee;
            Assert.IsTrue(result?.ID == 1);

            Mock.Verify(_employeeRepositoryMock);
        }

        [TestMethod]
        [ExpectedException(typeof(IdNotFoundException))]
        public void Test_Employee_Get_Exception()
        {
            var serviceProvider = services.BuildServiceProvider();
            employeeApiController = serviceProvider.GetService<EmployeeApiController>()!;

            _employeeRepositoryMock.Setup(cr => cr.GetById(It.IsAny<int>())).Returns(value: null).Verifiable();

            var actionResult = employeeApiController.Get(1) as ObjectResult;
        }
    }
}