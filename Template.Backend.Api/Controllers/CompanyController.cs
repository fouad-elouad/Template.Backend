using AutoMapper;
using Template.Backend.Api.Configuration;
using Template.Backend.Api.Exceptions;
using Template.Backend.Api.Models;
using Template.Backend.Model.Audit.Entities;
using Template.Backend.Model.Entities;
using Template.Backend.Model.Exceptions;
using Template.Backend.Service.Audit;
using Template.Backend.Service.Services;
using Microsoft.Web.Http;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace Template.Backend.Api.Controllers
{
    /// <summary>
    /// Route="api/v1/Companies"
    /// </summary>
    [ApiVersion("1")]
    [RoutePrefix(ApiRouteConfiguration.CompanyPrefix)]
    public class CompanyApiController : BaseApiController<Company, CompanyAudit>
    {
        private ICompanyService _CompanyService;
        private ICompanyAuditService _CompanyAuditService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyApiController"/> class.
        /// </summary>
        /// <param name="companyService">The company service.</param>
        /// <param name="companyAuditService">The company audit service.</param>
        /// <param name="mapper">The mapper.</param>
        public CompanyApiController(ICompanyService companyService, ICompanyAuditService companyAuditService, IMapper mapper)
            : base(companyService, companyAuditService, mapper)
        {
            _CompanyService = companyService;
            _CompanyAuditService = companyAuditService;
        }

        /// <summary>
        /// Count
        /// </summary>
        /// <returns>Count</returns>
        [ResponseType(typeof(int))]
        [Route(ApiRouteConfiguration.CountSuffix), HttpGet]
        public override IHttpActionResult Count()
        {
            _logger.Info("API: HttpGet Count Company");
            return base.Count();
        }

        /// <summary>
        /// Gets All companies
        /// </summary>
        /// <returns>List of companies</returns>
        [ResponseType(typeof(IEnumerable<Company>))]
        [Route(), HttpGet]
        public override IHttpActionResult Get()
        {
            _logger.Info("API: HttpGet Get Company");
            return base.Get();
        }

        /// <summary>
        /// Gets Company by Id.
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>company</returns>
        [ResponseType(typeof(Company))]
        [Route(ApiRouteConfiguration.IdSuffix), HttpGet]
        public override IHttpActionResult Get(int id)
        {
            _logger.Info("API: HttpGet Get(id) Company id={0}", id);
            return base.Get(id);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">Company ID .</param>
        /// <param name="depth">The maximum level to achieve for navigation properties serialization.</param>
        /// <returns>company</returns>
        [ResponseType(typeof(Company))]
        [Route(ApiRouteConfiguration.IdDepthSuffix), HttpGet]
        public override IHttpActionResult Get(int id, int depth)
        {
            _logger.Info("API: HttpGet Get(id,depth) Company id={0}, depth={1}", id, depth);
            return base.Get(id, depth);
        }

        /// <summary>
        /// Gets the paged list of Companyes.
        /// </summary>
        /// <param name="pageNo">page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>List of Companies</returns>
        [ResponseType(typeof(IEnumerable<Company>))]
        [Route(), HttpGet]
        public override IHttpActionResult GetPagedList([FromUri] int pageNo, int pageSize)
        {
            _logger.Info("API: HttpGet GetPagedList Company");
            return base.GetPagedList(pageNo, pageSize);
        }

        /// <summary>
        /// Deletes company with the specified Id.
        /// </summary>
        /// <param name="id">ID to Delete</param>
        /// <returns>Http Response with statut code (NoContent (204) if is Deleted otherwise error message)</returns>
        [Route(ApiRouteConfiguration.IdSuffix), HttpDelete]
        public override IHttpActionResult Delete(int id)
        {
            _logger.Info("API: HttpDelete Delete Company id={0}", id);
            return base.Delete(id);
        }

        /// <summary>
        /// Insert the specified company.
        /// </summary>
        /// <param name="companyDto">The company dto.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Inserted otherwise error message)
        /// </returns>
        [ResponseType(typeof(Company))]
        [Route(), HttpPost]
        public IHttpActionResult Post([FromBody] CompanyDto companyDto)
        {
            _logger.Info("API: HttpPost Post Company");
            Company company = Mapper.Map<CompanyDto, Company>(companyDto);
            return base.Post(company);
        }

        /// <summary>
        /// Update the company with the specified ID.
        /// </summary>
        /// <param name="id">the company ID to Update.</param>
        /// <param name="companyDto">The company dto.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Updated otherwise error message)
        /// </returns>
        [Route(ApiRouteConfiguration.IdSuffix), HttpPut]
        public IHttpActionResult Put(int id, CompanyDto companyDto)
        {
            _logger.Info("API: HttpPut Put Company id={0}", id);
            if (_CompanyService.CheckIfExist(o => o.ID == id))
            {
                Company company = Mapper.Map<CompanyDto, Company>(companyDto);
                return base.Put(id, company);
            }
            else
            {
                return ResponseMessage(ApiExceptionResponse.Throw(
                    new IdNotFoundException("No element found for this id"), Request));
            }
        }


        /// <summary>
        /// Gets Audit List of company with the specified Id.
        /// its provide All operations performed on this company
        /// </summary>
        /// <param name="id">The Company ID.</param>
        /// <returns>List of company Audits</returns>
        [ResponseType(typeof(IEnumerable<CompanyAudit>))]
        [Route(ApiRouteConfiguration.AuditSuffix), HttpGet]
        public IHttpActionResult Audit(int id)
        {
            _logger.Info("API: HttpGet Audit Company id={0}", id);

            IEnumerable<CompanyAudit> companyAudit = _CompanyAuditService.GetAuditById(id);

            if (companyAudit != null && companyAudit.Any())
            {
                return ResponseMessage(AuditToJsonResponse(companyAudit, 2));
            }
            else
            {
                return ResponseMessage(ApiExceptionResponse.Throw(new IdNotFoundException("No element found for this id"), Request));
            }
        }

        /// <summary>
        /// Gets Audit of the specified ID.
        /// </summary>
        /// <param name="id">The audit Id.</param>
        /// <returns>Audit line of Company</returns>
        [ResponseType(typeof(CompanyAudit))]
        [Route(ApiRouteConfiguration.AuditIdSuffix), HttpGet]
        public override IHttpActionResult AuditId(int id)
        {
            _logger.Info("API: HttpGet AuditId Company id={0}", id);
            return base.AuditId(id);
        }

        /// <summary>
        /// Restore audit Values to current Company.
        /// </summary>
        /// <param name="id">Company Id.</param>
        /// <param name="auditId">Company audit ID.</param>
        /// <param name="content">Empty http content.</param>
        /// <returns>Http Response with statut code (OK if is Restored otherwise error message) </returns>
        [Route(ApiRouteConfiguration.AuditRestoreSuffix), HttpPut]
        public IHttpActionResult PutRestoreAudit(int id, int auditId, [FromBody] string content)
        {
            _logger.Info("API: HttpPut PutRestoreAudit Company id={0}, auditId={1}", id, auditId);
            Company company = _CompanyService.GetById(id);

            CompanyAudit companyAudit = _CompanyAuditService.GetById(auditId);
            if (company == null)
            {
                company = new Company();
                _CompanyService.Restore(company, companyAudit);
                _CompanyService.Add(company);
            }
            else
            {
                _CompanyService.Restore(company, companyAudit);
            }

            _CompanyService.Save(User.Identity.Name);

            return Ok();
        }

        /// <summary>
        /// Gets the snapshot of all entities at the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time in the yyyyMMddTHHmmss format.</param>
        /// <returns>
        /// List of Company as Json response
        /// </returns>
        [ResponseType(typeof(IEnumerable<CompanyAudit>))]
        [Route(ApiRouteConfiguration.SnapshotSuffix), HttpGet]
        public override IHttpActionResult GetSnapshot(string dateTime)
        {
            _logger.Info("API: HttpGet GetSnapshot Company date={0}", dateTime);
            return base.GetSnapshot(dateTime);
        }

        /// <summary>
        /// Gets the snapshot at the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time in the yyyyMMddTHHmmss format.</param>
        /// <param name="id">The ID.</param>
        /// <returns>
        /// Company as Json response
        /// </returns>
        [ResponseType(typeof(Company))]
        [Route(ApiRouteConfiguration.SnapshotIdSuffix), HttpGet]
        public override IHttpActionResult GetSnapshot(string dateTime, int id)
        {
            _logger.Info("API: HttpGet GetSnapshot Company dateTime={0}, id={1}", dateTime, id);
            return base.GetSnapshot(dateTime, id);
        }

        /// <summary>
        /// Insert the specified company List.
        /// </summary>
        /// <param name="companyDtoList">The company dto list.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Inserted otherwise error message)
        /// </returns>
        [ResponseType(typeof(IEnumerable<Company>))]
        [Route(ApiRouteConfiguration.ItemsSuffix), HttpPost]
        public IHttpActionResult PostList([FromBody] IEnumerable<CompanyDto> companyDtoList)
        {
            _logger.Info("API: HttpPost Post CompanyList");
            IEnumerable<Company> companyList = Mapper.Map<IEnumerable<CompanyDto>, IEnumerable<Company>>(companyDtoList);
            if (!_CompanyService.HugeInsertValidation(companyList))
            {
                AddServiceErrorsToModelState(_CompanyService.GetValidationDictionary());
                return ResponseMessage(InvalidModelStateToJsonResponse(_CompanyService.GetValidationDictionary().ToDictionary()));
            }

            return base.PostList(companyList);
        }

        /// <summary>
        /// Gets Company by Name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Company
        /// </returns>
        [ResponseType(typeof(Company))]
        [Route(), HttpGet]
        public IHttpActionResult GetByName([FromUri] string name)
        {
            _logger.Info("API: HttpGet GetByName Company name={0}", name);
            Company company = _CompanyService.Get(c => c.Name == name);
            if (company != null)
            {
                return ResponseMessage(ToJsonResponse(company, HttpStatusCode.OK, _detailsDepth));
            }
            else
            {
                return ResponseMessage(ApiExceptionResponse.Throw(new NoElementFoundException("No element found for this shorName"), Request));
            }
        }
    }
}