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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace Template.Backend.Api.Controllers
{
    /// <summary>
    /// Route="api/v1/Departments"
    /// </summary>
    [ApiVersion("1")]
    [RoutePrefix(ApiRouteConfiguration.DepartmentPrefix)]
    public class DepartmentApiController : BaseApiController<Department, DepartmentAudit>
    {
        private IDepartmentService _DepartmentService;
        private IDepartmentAuditService _DepartmentAuditService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentApiController"/> class.
        /// </summary>
        /// <param name="departmentService">The department service.</param>
        /// <param name="departmentAuditService">The department audit service.</param>
        public DepartmentApiController(IDepartmentService departmentService, IDepartmentAuditService departmentAuditService)
            : base(departmentService, departmentAuditService)
        {
            _DepartmentService = departmentService;
            _DepartmentAuditService = departmentAuditService;
        }

        /// <summary>
        /// Count
        /// </summary>
        /// <returns>Count</returns>
        [ResponseType(typeof(int))]
        [Route(ApiRouteConfiguration.CountSuffix), HttpGet]
        public override IHttpActionResult Count()
        {
            _logger.Info("API: HttpGet Count Department");
            return base.Count();
        }

        /// <summary>
        /// Gets All departments
        /// </summary>
        /// <returns>List of departments</returns>
        [ResponseType(typeof(IEnumerable<Department>))]
        [Route(), HttpGet]
        public override IHttpActionResult Get()
        {
            _logger.Info("API: HttpGet Get Department");
            return base.Get();
        }

        /// <summary>
        /// Gets Department by Id.
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>department</returns>
        [ResponseType(typeof(Department))]
        [Route(ApiRouteConfiguration.IdSuffix), HttpGet]
        public override IHttpActionResult Get(int id)
        {
            _logger.Info("API: HttpGet Get(id) Department id={0}", id);
            return base.Get(id);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">Department ID .</param>
        /// <param name="depth">The maximum level to achieve for navigation properties serialization.</param>
        /// <returns>department</returns>
        [ResponseType(typeof(Department))]
        [Route(ApiRouteConfiguration.IdDepthSuffix), HttpGet]
        public override IHttpActionResult Get(int id, int depth)
        {
            _logger.Info("API: HttpGet Get(id,depth) Department id={0}, depth={1}", id, depth);
            return base.Get(id, depth);
        }

        /// <summary>
        /// Gets the paged list of Departmentes.
        /// </summary>
        /// <param name="pageNo">page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>List of Departments</returns>
        [ResponseType(typeof(IEnumerable<Department>))]
        [Route(), HttpGet]
        public override IHttpActionResult GetPagedList([FromUri] int pageNo, int pageSize)
        {
            _logger.Info("API: HttpGet GetPagedList Department");
            return base.GetPagedList(pageNo, pageSize);
        }

        /// <summary>
        /// Deletes department with the specified Id.
        /// </summary>
        /// <param name="id">ID to Delete</param>
        /// <returns>Http Response with statut code (NoContent (204) if is Deleted otherwise error message)</returns>
        [Route(ApiRouteConfiguration.IdSuffix), HttpDelete]
        public override IHttpActionResult Delete(int id)
        {
            _logger.Info("API: HttpDelete Delete Department id={0}", id);
            return base.Delete(id);
        }

        /// <summary>
        /// Insert the specified department.
        /// </summary>
        /// <param name="departmentDto">The department dto.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Inserted otherwise error message)
        /// </returns>
        [ResponseType(typeof(Department))]
        [Route(), HttpPost]
        public IHttpActionResult Post([FromBody] DepartmentDto departmentDto)
        {
            _logger.Info("API: HttpPost Post Department");
            Department department = Mapper.Map<DepartmentDto, Department>(departmentDto);
            return base.Post(department);
        }

        /// <summary>
        /// Update the department with the specified ID.
        /// </summary>
        /// <param name="id">the department ID to Update.</param>
        /// <param name="departmentDto">The department dto.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Updated otherwise error message)
        /// </returns>
        [Route(ApiRouteConfiguration.IdSuffix), HttpPut]
        public IHttpActionResult Put(int id, DepartmentDto departmentDto)
        {
            _logger.Info("API: HttpPut Put Department id={0}", id);
            if (_DepartmentService.CheckIfExist(o => o.ID == id))
            {
                Department department = Mapper.Map<DepartmentDto, Department>(departmentDto);
                return base.Put(id, department);
            }
            else
            {
                return ResponseMessage(ApiExceptionResponse.Throw(
                    new IdNotFoundException("No element found for this id"), Request));
            }
        }

        /// <summary>
        /// Gets Audit List of department with the specified Id.
        /// its provide All operations performed on this department
        /// </summary>
        /// <param name="id">The Department ID.</param>
        /// <returns>List of department Audits</returns>
        [ResponseType(typeof(IEnumerable<DepartmentAudit>))]
        [Route(ApiRouteConfiguration.AuditSuffix), HttpGet]
        public IHttpActionResult Audit(int id)
        {
            _logger.Info("API: HttpGet Audit Department id={0}", id);

            IEnumerable<DepartmentAudit> departmentAudit = _DepartmentAuditService.GetAuditById(id);

            if (departmentAudit != null && departmentAudit.Any())
            {
                return ResponseMessage(AuditToJsonResponse(departmentAudit, 2));
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
        /// <returns>Audit line of Department</returns>
        [ResponseType(typeof(DepartmentAudit))]
        [Route(ApiRouteConfiguration.AuditIdSuffix), HttpGet]
        public override IHttpActionResult AuditId(int id)
        {
            _logger.Info("API: HttpGet AuditId Department id={0}", id);
            return base.AuditId(id);
        }

        /// <summary>
        /// Restore audit Values to current Department.
        /// </summary>
        /// <param name="id">Department Id.</param>
        /// <param name="auditId">Department audit ID.</param>
        /// <param name="content">Empty http content.</param>
        /// <returns>Http Response with statut code (OK if is Restored otherwise error message) </returns>
        [Route(ApiRouteConfiguration.AuditRestoreSuffix), HttpPut]
        public IHttpActionResult PutRestoreAudit(int id, int auditId, [FromBody] string content)
        {
            _logger.Info("API: HttpPut PutRestoreAudit Department id={0}, auditId={1}", id, auditId);
            Department department = _DepartmentService.GetById(id);

            DepartmentAudit departmentAudit = _DepartmentAuditService.GetById(auditId);
            if (department == null)
            {
                department = new Department();
                _DepartmentService.Restore(department, departmentAudit);
                _DepartmentService.Add(department);
            }
            else
            {
                _DepartmentService.Restore(department, departmentAudit);
            }

            _DepartmentService.Save(User.Identity.Name);

            return Ok();
        }

        /// <summary>
        /// Gets the snapshot of all entities at the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time in the yyyyMMddTHHmmss format.</param>
        /// <returns>
        /// List of Department as Json response
        /// </returns>
        [ResponseType(typeof(IEnumerable<DepartmentAudit>))]
        [Route(ApiRouteConfiguration.SnapshotSuffix), HttpGet]
        public override IHttpActionResult GetSnapshot(string dateTime)
        {
            _logger.Info("API: HttpGet GetSnapshot Department date={0}", dateTime);
            return base.GetSnapshot(dateTime);
        }

        /// <summary>
        /// Gets the snapshot at the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time in the yyyyMMddTHHmmss format.</param>
        /// <param name="id">The ID.</param>
        /// <returns>
        /// Department as Json response
        /// </returns>
        [ResponseType(typeof(Department))]
        [Route(ApiRouteConfiguration.SnapshotIdSuffix), HttpGet]
        public override IHttpActionResult GetSnapshot(string dateTime, int id)
        {
            _logger.Info("API: HttpGet GetSnapshot Department dateTime={0}, id={1}", dateTime, id);
            return base.GetSnapshot(dateTime, id);
        }

        /// <summary>
        /// Insert the specified department List.
        /// </summary>
        /// <param name="departmentDtoList">The department dto list.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Inserted otherwise error message)
        /// </returns>
        [ResponseType(typeof(IEnumerable<Department>))]
        [Route(ApiRouteConfiguration.ItemsSuffix), HttpPost]
        public IHttpActionResult PostList([FromBody] IEnumerable<DepartmentDto> departmentDtoList)
        {
            _logger.Info("API: HttpPost Post DepartmentList");
            IEnumerable<Department> departmentList = Mapper.Map<IEnumerable<DepartmentDto>, IEnumerable<Department>>(departmentDtoList);
            if (!_DepartmentService.HugeInsertValidation(departmentList))
            {
                AddServiceErrorsToModelState(_DepartmentService.GetValidationDictionary());
                return ResponseMessage(InvalidModelStateToJsonResponse(_DepartmentService.GetValidationDictionary().ToDictionary()));
            }

            return base.PostList(departmentList);
        }

        /// <summary>
        /// Gets Department by Name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Department
        /// </returns>
        [ResponseType(typeof(Department))]
        [Route(), HttpGet]
        public IHttpActionResult GetByName([FromUri] string name)
        {
            _logger.Info("API: HttpGet GetByName Department name={0}", name);
            Department department = _DepartmentService.Get(c => c.Name == name);
            if (department != null)
            {
                return ResponseMessage(ToJsonResponse(department, HttpStatusCode.OK, _detailsDepth));
            }
            else
            {
                return ResponseMessage(ApiExceptionResponse.Throw(new NoElementFoundException("No element found for this shorName"), Request));
            }
        }
    }
}