using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
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

namespace Template.Backend.UnitTest
{
    [TestClass]
    public class DepartmentUnitTest
    {
        DepartmentApiController departmentApiController;
        IServiceCollection services;
        Mock<IUnitOfWork> _uowMock;
        Mock<IDepartmentRepository> _departmentRepositoryMock;


        [TestInitialize]
        public void Initialize()
        {
            services = new ServiceCollection();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IDepartmentAuditService, DepartmentAuditService>();

            var logger = new Mock<ILogger<Department>>();
            services.AddScoped(typeof(ILogger<Department>), s => logger.Object);

            _uowMock = new Mock<IUnitOfWork>();
            services.AddScoped(typeof(IUnitOfWork), s => _uowMock.Object);

            _departmentRepositoryMock = new Mock<IDepartmentRepository>();
            services.AddScoped(typeof(IDepartmentRepository), s => _departmentRepositoryMock.Object);

            var departmentAuditRepositoryMock = new Mock<IAuditRepository<DepartmentAudit>>();
            services.AddScoped(typeof(IAuditRepository<DepartmentAudit>), s => departmentAuditRepositoryMock.Object);

            services.AddTransient<IValidationDictionary, ValidationDictionary>();
            services.AddTransient<DepartmentApiController, DepartmentApiController>();
            services.AddScoped(typeof(IAuditRepository<>), typeof(AuditRepository<>));

            services.AddAutoMapper(typeof(DepartmentUnitTest), typeof(DepartmentApiController));
        }

        [TestMethod]
        public void Test_Department_GetPagedList()
        {
            var serviceProvider = services.BuildServiceProvider();
            departmentApiController = serviceProvider.GetService<DepartmentApiController>()!;

            _departmentRepositoryMock.Setup(cr => cr.GetPagedList(It.IsAny<int>(), It.IsAny<int>())).Returns(Helper.DepartmentFactory(2)).Verifiable();

            var actionResult = departmentApiController.GetPagedList(1, 2) as ObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.IsInstanceOfType(actionResult.Value, typeof(IEnumerable<Department>));

            var result = actionResult.Value as IEnumerable<Department>;
            Assert.IsTrue(result?.Count() == 2);
            Assert.IsTrue(result.ElementAt(0).ID == 1);
            Assert.IsTrue(result.ElementAt(1).ID == 2);

            _departmentRepositoryMock.Setup(cr => cr.GetPagedList(It.IsAny<int>(), It.IsAny<int>())).Returns(Helper.DepartmentFactory(0)).Verifiable();

            // Action
            void act() => departmentApiController.GetPagedList(1, 2);
            Assert.ThrowsException<NoElementFoundException>(act);

            Mock.Verify(_departmentRepositoryMock);
        }

        [TestMethod]
        public void Test_Department_Delete()
        {
            var serviceProvider = services.BuildServiceProvider();
            departmentApiController = serviceProvider.GetService<DepartmentApiController>()!;

            _departmentRepositoryMock.Setup(cr => cr.Delete(It.IsAny<int>())).Verifiable();

            var actionResult = departmentApiController.Delete(1) as NoContentResult;
            Assert.IsNotNull(actionResult);

            Mock.Verify(_departmentRepositoryMock);
        }

        [TestMethod]
        [ExpectedException(typeof(IdNotFoundException))]
        public void Test_Department_Delete_Exception()
        {
            var serviceProvider = services.BuildServiceProvider();
            departmentApiController = serviceProvider.GetService<DepartmentApiController>()!;

            _departmentRepositoryMock.Setup(cr => cr.Delete(It.IsAny<int>())).Throws(new IdNotFoundException("")).Verifiable();

            var actionResult = departmentApiController.Delete(1);
        }

        [TestMethod]
        public void Test_Department_Post()
        {
            var serviceProvider = services.BuildServiceProvider();
            departmentApiController = serviceProvider.GetService<DepartmentApiController>()!;

            Department department = Helper.DepartmentFactory(1).First();
            _departmentRepositoryMock.Setup(cr => cr.Add(It.IsAny<Department>())).Callback<Department>(Helper.IncrementRowVersion).Verifiable();
            _departmentRepositoryMock.Setup(cr => cr.CheckIsUnique(It.IsAny<string>(), It.IsAny<int>())).Returns(true).Verifiable();

            var actionResult = departmentApiController.Post(Helper.DepartmentDtoFactory(1).First()) as ObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(CreatedAtActionResult));
            Assert.IsInstanceOfType(actionResult.Value, typeof(Department));

            var result = actionResult.Value as Department;
            Assert.IsTrue(result?.RowVersion == 1);

            Mock.Verify(_departmentRepositoryMock);
        }

        [TestMethod]
        public void Test_Department_Update()
        {
            var serviceProvider = services.BuildServiceProvider();
            departmentApiController = serviceProvider.GetService<DepartmentApiController>()!;

            _departmentRepositoryMock.Setup(cr => cr.Update(It.IsAny<Department>())).Callback<Department>(Helper.IncrementRowVersion).Verifiable();
            _departmentRepositoryMock.Setup(cr => cr.CheckIfExist(It.IsAny<Expression<Func<Department, bool>>>())).Returns(true).Verifiable();
            _departmentRepositoryMock.Setup(cr => cr.CheckIsUnique(It.IsAny<string>(), It.IsAny<int>())).Returns(true).Verifiable();

            DepartmentDto departmentDto = Helper.DepartmentDtoFactory(1).First();
            var actionResult = departmentApiController.Put(departmentDto.ID, departmentDto) as StatusCodeResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));

            Mock.Verify(_departmentRepositoryMock);
        }

        [TestMethod]
        [ExpectedException(typeof(IdNotFoundException))]
        public void Test_Department_Update_Exception()
        {
            var serviceProvider = services.BuildServiceProvider();
            departmentApiController = serviceProvider.GetService<DepartmentApiController>()!;

            _departmentRepositoryMock.Setup(cr => cr.Update(It.IsAny<Department>())).Callback<Department>(Helper.IncrementRowVersion).Verifiable();
            _departmentRepositoryMock.Setup(cr => cr.CheckIfExist(It.IsAny<Expression<Func<Department, bool>>>())).Returns(false).Verifiable();
            _departmentRepositoryMock.Setup(cr => cr.CheckIsUnique(It.IsAny<string>(), It.IsAny<int>())).Returns(true).Verifiable();

            DepartmentDto departmentDto = Helper.DepartmentDtoFactory(1).First();
            var actionResult = departmentApiController.Put(departmentDto.ID, departmentDto) as StatusCodeResult;

            Mock.Verify(_departmentRepositoryMock);
        }

        [TestMethod]
        public void Test_Department_Get()
        {
            var serviceProvider = services.BuildServiceProvider();
            departmentApiController = serviceProvider.GetService<DepartmentApiController>()!;

            _departmentRepositoryMock.Setup(cr => cr.GetById(It.IsAny<int>())).Returns(Helper.DepartmentFactory(1).First()).Verifiable();

            var actionResult = departmentApiController.Get(1) as ObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.IsInstanceOfType(actionResult.Value, typeof(Department));

            var result = actionResult.Value as Department;
            Assert.IsTrue(result?.ID == 1);

            Mock.Verify(_departmentRepositoryMock);
        }

        [TestMethod]
        [ExpectedException(typeof(IdNotFoundException))]
        public void Test_Department_Get_Exception()
        {
            var serviceProvider = services.BuildServiceProvider();
            departmentApiController = serviceProvider.GetService<DepartmentApiController>()!;

            _departmentRepositoryMock.Setup(cr => cr.GetById(It.IsAny<int>())).Returns(value: null).Verifiable();

            var actionResult = departmentApiController.Get(1) as ObjectResult;
        }
    }
}