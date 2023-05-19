using Template.Backend.Model.Audit.Entities;
using Template.Backend.Model.Entities;
using Template.Backend.Service.Audit;
using Template.Backend.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Template.Backend.Api.Models;
using System.Net.Mime;
using AutoMapper;
using Template.Backend.Api.Configuration;
using Template.Backend.Model.Exceptions;
using Template.Backend.Model.Enums;

namespace Template.Backend.Api.Controllers
{
    /// <summary>
    /// Route="api/v1/Companies"
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route(ApiRouteConfiguration.CompanyPrefix)]
    public class CompanyApiController : BaseApiController<Company, CompanyAudit>
    {
        private ICompanyService _CompanyService;
        private ICompanyAuditService _CompanyAuditService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyApiController"/> class.
        /// </summary>
        /// <param name="companyService">The company service.</param>
        /// <param name="companyAuditService">The company audit service.</param>
        public CompanyApiController(ICompanyService companyService, ICompanyAuditService companyAuditService, IMapper mapper, ILogger<Company> logger)
            : base(companyService, companyAuditService, mapper, logger)
        {
            _CompanyService = companyService;
            _CompanyAuditService = companyAuditService;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [HttpGet(ApiRouteConfiguration.CountSuffix)]
        public IActionResult Count()
        {
            return base.Count();
        }

        /// <summary>
        /// Gets All companies
        /// </summary>
        /// <returns>List of companies</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Company>))]
        [HttpGet]
        public IActionResult Get()
        {
            return base.Get();
        }

        /// <summary>
        /// Gets Company by Id.
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>company</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Company))]
        [HttpGet(ApiRouteConfiguration.IdSuffix)]
        public IActionResult Get(int id)
        {
            return base.Get(id);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">Company ID .</param>
        /// <param name="nestedObjectDepth">The maximum level to achieve for navigation properties serialization.</param>
        /// <returns>company</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Company))]
        [HttpGet(ApiRouteConfiguration.IdDepthSuffix)]
        public IActionResult Get(int id, NestedObjectDepth nestedObjectDepth)
        {
            _logger.LogInformation($"Get {typeof(Company).Name} with Id {id} and {nestedObjectDepth}");
            var entity = _CompanyService.FindById(id, nestedObjectDepth);
            if (entity != null)
            {
                return Ok(entity);
            }
            else
            {
                throw new IdNotFoundException($"No element found for this id {id}");
            }
        }

        /// <summary>
        /// Gets the paged list of Companyes.
        /// </summary>
        /// <param name="pageNo">page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>List of Companies</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Company>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(ApiRouteConfiguration.Pagination)]
        public IActionResult GetPagedList([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            return base.GetPagedList(pageNo, pageSize);
        }

        /// <summary>
        /// Deletes company with the specified Id.
        /// </summary>
        /// <param name="id">ID to Delete</param>
        /// <returns>Http Response with statut code (NoContent (204) if is Deleted otherwise error message)</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete(ApiRouteConfiguration.IdSuffix)]
        public IActionResult Delete(int id)
        {
            return base.Delete(id);
        }

        /// <summary>
        /// Insert the specified company.
        /// </summary>
        /// <param name="companyDto">The company dto.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Inserted otherwise error message)
        /// </returns>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Company))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost]
        public IActionResult Post([FromBody] CompanyDto companyDto)
        {
            Company company = _mapper.Map<CompanyDto, Company>(companyDto);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPut(ApiRouteConfiguration.IdSuffix)]
        public IActionResult Put(int id, [FromBody] CompanyDto companyDto)
        {
            Company company = _mapper.Map<CompanyDto, Company>(companyDto);
            return base.Put(id, company);
        }


        /// <summary>
        /// Gets Audit List of company with the specified Id.
        /// its provide All operations performed on this company
        /// </summary>
        /// <param name="id">The Company ID.</param>
        /// <returns>List of company Audits</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyAudit>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(ApiRouteConfiguration.AuditSuffix)]
        public IActionResult Audit(int id)
        {
            return base.Audit(id);
        }

        /// <summary>
        /// Gets Audit of the specified ID.
        /// </summary>
        /// <param name="id">The audit Id.</param>
        /// <returns>Audit line of Company</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompanyAudit))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(ApiRouteConfiguration.AuditIdSuffix)]
        public IActionResult AuditId(int id)
        {
            return base.AuditId(id);
        }

        /// <summary>
        /// Restore audit Values to current Company.
        /// </summary>
        /// <param name="id">Company Id, To restore deleted entity make id=0.</param>
        /// <param name="auditId">Company audit ID.</param>
        /// <param name="content">Empty http content.</param>
        /// <returns>Http Response with statut code (OK if is Restored otherwise error message) </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut(ApiRouteConfiguration.AuditRestoreSuffix)]
        public IActionResult RestoreAudit(int id, int auditId)
        {
            _logger.LogInformation($"RestoreAudit for {typeof(Company).Name} with Id {id} from {typeof(CompanyAudit).Name} with Id {auditId}");
            

            CompanyAudit companyAudit = _CompanyAuditService.GetById(auditId);
            if (companyAudit == null )
                throw new IdNotFoundException($"No element found for this auditId {auditId}");

            Company company = _CompanyService.GetById(id);

            if (id == 0) // restore deleted company
            {
                company = new Company();
                _CompanyService.Restore(company, companyAudit);
                _CompanyService.Add(company);
            }
            else
            {
                if (company == null)
                    throw new IdNotFoundException($"No element found for this Id {id}");
                if (company.ID != companyAudit.ID)
                    throw new BadRequestException($"Invalid Audit for this Id {id} and auditId {auditId}");

                _CompanyService.Restore(company, companyAudit);
                _CompanyService.Update(company);
            }

            // model state test
            if (!_CompanyService.GetValidationDictionary().IsValid() || !ModelState.IsValid)
            {
                AddServiceErrorsToModelState(_CompanyService.GetValidationDictionary());
                throw new ModelStateException("One or more validation errors occurred.", _CompanyService.GetValidationDictionary().ToReadOnlyDictionary());
            }

            _CompanyService.Save();

            return Ok();
        }

        /// <summary>
        /// Gets the snapshot of all entities at the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time in the yyyyMMddTHHmmss format.</param>
        /// <returns>
        /// List of Company as Json response
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyAudit>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(ApiRouteConfiguration.SnapshotSuffix)]
        public IActionResult GetSnapshot(string dateTime)
        {
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Company))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(ApiRouteConfiguration.SnapshotIdSuffix)]
        public IActionResult GetSnapshot(string dateTime, int id)
        {
            return base.GetSnapshot(dateTime, id);
        }

        /// <summary>
        /// Insert the specified company List.
        /// </summary>
        /// <param name="companyDtoList">The company dto list.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Inserted otherwise error message)
        /// </returns>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<Company>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost(ApiRouteConfiguration.ItemsSuffix)]
        public IActionResult PostList([FromBody] IEnumerable<CompanyDto> companyDtoList)
        {
            _logger.LogInformation($"PostList {typeof(Company).Name} count : {companyDtoList?.Count()}");

            IEnumerable<Company> companyList = _mapper.Map<IEnumerable<CompanyDto>, IEnumerable<Company>>(companyDtoList);

            // model state test
            if (!_CompanyService.HugeInsertValidation(companyList) || !ModelState.IsValid)
            {
                AddServiceErrorsToModelState(_CompanyService.GetValidationDictionary());
                throw new ModelStateException("One or more validation errors occurred.", _CompanyService.GetValidationDictionary().ToReadOnlyDictionary());
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Company))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(ApiRouteConfiguration.SearchSuffix)]
        public IActionResult GetByName([FromQuery] string name)
        {
            Company company = _CompanyService.Get(c => c.Name == name);
            if (company != null)
            {
                return Ok(company);
            }
            else
            {
                throw new NoElementFoundException($"No element found for this name {name}");
            }
        }
    }
}