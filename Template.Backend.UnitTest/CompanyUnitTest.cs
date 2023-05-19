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
    public class CompanyUnitTest
    {
        CompanyApiController companyApiController;
        IServiceCollection services;
        Mock<IUnitOfWork> _uowMock;
        Mock<ICompanyRepository> _companyRepositoryMock;


        [TestInitialize]
        public void Initialize()
        {
            services = new ServiceCollection();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<ICompanyAuditService, CompanyAuditService>();

            var logger = new Mock<ILogger<Company>>();
            services.AddScoped(typeof(ILogger<Company>), s => logger.Object);

            _uowMock = new Mock<IUnitOfWork>();
            services.AddScoped(typeof(IUnitOfWork), s => _uowMock.Object);

            _companyRepositoryMock = new Mock<ICompanyRepository>();
            services.AddScoped(typeof(ICompanyRepository), s => _companyRepositoryMock.Object);

            var companyAuditRepositoryMock = new Mock<IAuditRepository<CompanyAudit>>();
            services.AddScoped(typeof(IAuditRepository<CompanyAudit>), s => companyAuditRepositoryMock.Object);

            services.AddTransient<IValidationDictionary, ValidationDictionary>();
            services.AddTransient<CompanyApiController, CompanyApiController>();
            services.AddScoped(typeof(IAuditRepository<>), typeof(AuditRepository<>));

            services.AddAutoMapper(typeof(CompanyUnitTest), typeof(CompanyApiController));
        }

        [TestMethod]
        public void Test_Company_GetPagedList()
        {
            var serviceProvider = services.BuildServiceProvider();
            companyApiController = serviceProvider.GetService<CompanyApiController>()!;

            _companyRepositoryMock.Setup(cr => cr.GetPagedList(It.IsAny<int>(), It.IsAny<int>())).Returns(Helper.CompanyFactory(2)).Verifiable();

            var actionResult = companyApiController.GetPagedList(1, 2) as ObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.IsInstanceOfType(actionResult.Value, typeof(IEnumerable<Company>));

            var result = actionResult.Value as IEnumerable<Company>;
            Assert.IsTrue(result?.Count() == 2);
            Assert.IsTrue(result.ElementAt(0).ID == 1);
            Assert.IsTrue(result.ElementAt(1).ID == 2);

            _companyRepositoryMock.Setup(cr => cr.GetPagedList(It.IsAny<int>(), It.IsAny<int>())).Returns(Helper.CompanyFactory(0)).Verifiable();

            // Action
            void act() => companyApiController.GetPagedList(1, 2);
            Assert.ThrowsException<NoElementFoundException>(act);

            Mock.Verify(_companyRepositoryMock);
        }

        [TestMethod]
        public void Test_Company_Delete()
        {
            var serviceProvider = services.BuildServiceProvider();
            companyApiController = serviceProvider.GetService<CompanyApiController>()!;

            _companyRepositoryMock.Setup(cr => cr.Delete(It.IsAny<int>())).Verifiable();

            var actionResult = companyApiController.Delete(1) as NoContentResult;
            Assert.IsNotNull(actionResult);

            Mock.Verify(_companyRepositoryMock);
        }

        [TestMethod]
        [ExpectedException(typeof(IdNotFoundException))]
        public void Test_Company_Delete_Exception()
        {
            var serviceProvider = services.BuildServiceProvider();
            companyApiController = serviceProvider.GetService<CompanyApiController>()!;

            _companyRepositoryMock.Setup(cr => cr.Delete(It.IsAny<int>())).Throws(new IdNotFoundException("")).Verifiable();

            var actionResult = companyApiController.Delete(1);
        }

        [TestMethod]
        public void Test_Company_Post()
        {
            var serviceProvider = services.BuildServiceProvider();
            companyApiController = serviceProvider.GetService<CompanyApiController>()!;

            Company company = Helper.CompanyFactory(1).First();
            _companyRepositoryMock.Setup(cr => cr.Add(It.IsAny<Company>())).Callback<Company>(Helper.IncrementRowVersion).Verifiable();
            _companyRepositoryMock.Setup(cr => cr.CheckIsUnique(It.IsAny<string>(), It.IsAny<int>())).Returns(true).Verifiable();

            var actionResult = companyApiController.Post(Helper.CompanyDtoFactory(1).First()) as ObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(CreatedAtActionResult));
            Assert.IsInstanceOfType(actionResult.Value, typeof(Company));

            var result = actionResult.Value as Company;
            Assert.IsTrue(result?.RowVersion == 1);

            Mock.Verify(_companyRepositoryMock);
        }

        [TestMethod]
        public void Test_Company_Update()
        {
            var serviceProvider = services.BuildServiceProvider();
            companyApiController = serviceProvider.GetService<CompanyApiController>()!;

            _companyRepositoryMock.Setup(cr => cr.Update(It.IsAny<Company>())).Callback<Company>(Helper.IncrementRowVersion).Verifiable();
            _companyRepositoryMock.Setup(cr => cr.CheckIfExist(It.IsAny<Expression<Func<Company, bool>>>())).Returns(true).Verifiable();
            _companyRepositoryMock.Setup(cr => cr.CheckIsUnique(It.IsAny<string>(), It.IsAny<int>())).Returns(true).Verifiable();

            CompanyDto companyDto = Helper.CompanyDtoFactory(1).First();
            var actionResult = companyApiController.Put(companyDto.ID, companyDto) as StatusCodeResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));

            Mock.Verify(_companyRepositoryMock);
        }

        [TestMethod]
        [ExpectedException(typeof(IdNotFoundException))]
        public void Test_Company_Update_Exception()
        {
            var serviceProvider = services.BuildServiceProvider();
            companyApiController = serviceProvider.GetService<CompanyApiController>()!;

            _companyRepositoryMock.Setup(cr => cr.Update(It.IsAny<Company>())).Callback<Company>(Helper.IncrementRowVersion).Verifiable();
            _companyRepositoryMock.Setup(cr => cr.CheckIfExist(It.IsAny<Expression<Func<Company, bool>>>())).Returns(false).Verifiable();
            _companyRepositoryMock.Setup(cr => cr.CheckIsUnique(It.IsAny<string>(), It.IsAny<int>())).Returns(true).Verifiable();

            CompanyDto companyDto = Helper.CompanyDtoFactory(1).First();
            var actionResult = companyApiController.Put(companyDto.ID, companyDto) as StatusCodeResult;

            Mock.Verify(_companyRepositoryMock);
        }

        [TestMethod]
        public void Test_Company_Get()
        {
            var serviceProvider = services.BuildServiceProvider();
            companyApiController = serviceProvider.GetService<CompanyApiController>()!;

            _companyRepositoryMock.Setup(cr => cr.GetById(It.IsAny<int>())).Returns(Helper.CompanyFactory(1).First()).Verifiable();

            var actionResult = companyApiController.Get(1) as ObjectResult;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
            Assert.IsInstanceOfType(actionResult.Value, typeof(Company));

            var result = actionResult.Value as Company;
            Assert.IsTrue(result?.ID == 1);

            Mock.Verify(_companyRepositoryMock);
        }

        [TestMethod]
        [ExpectedException(typeof(IdNotFoundException))]
        public void Test_Company_Get_Exception()
        {
            var serviceProvider = services.BuildServiceProvider();
            companyApiController = serviceProvider.GetService<CompanyApiController>()!;

            _companyRepositoryMock.Setup(cr => cr.GetById(It.IsAny<int>())).Returns(value: null).Verifiable();

            var actionResult = companyApiController.Get(1) as ObjectResult;
        }
    }
}