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
    /// Route="api/v1/Departments"
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route(ApiRouteConfiguration.DepartmentPrefix)]
    public class DepartmentApiController : BaseApiController<Department, DepartmentAudit>
    {
        private IDepartmentService _DepartmentService;
        private IDepartmentAuditService _DepartmentAuditService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentApiController"/> class.
        /// </summary>
        /// <param name="departmentService">The department service.</param>
        /// <param name="departmentAuditService">The department audit service.</param>
        public DepartmentApiController(IDepartmentService departmentService, IDepartmentAuditService departmentAuditService, IMapper mapper, ILogger<Department> logger)
            : base(departmentService, departmentAuditService, mapper, logger)
        {
            _DepartmentService = departmentService;
            _DepartmentAuditService = departmentAuditService;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [Route(ApiRouteConfiguration.CountSuffix), HttpGet]
        public override IActionResult Count()
        {
            return base.Count();
        }

        /// <summary>
        /// Gets All departments
        /// </summary>
        /// <returns>List of departments</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Department>))]
        [HttpGet]
        public IActionResult Get()
        {
            return base.Get();
        }

        /// <summary>
        /// Gets Department by Id.
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>department</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Department))]
        [HttpGet(ApiRouteConfiguration.IdSuffix)]
        public IActionResult Get(int id)
        {
            return base.Get(id);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">Department ID .</param>
        /// <param name="nestedObjectDepth">The maximum level to achieve for navigation properties serialization.</param>
        /// <returns>department</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Department))]
        [HttpGet(ApiRouteConfiguration.IdDepthSuffix)]
        public IActionResult Get(int id, NestedObjectDepth nestedObjectDepth)
        {
            _logger.LogInformation($"Get {typeof(Department).Name} with Id {id} and {nestedObjectDepth}");
            var entity = _DepartmentService.FindById(id, nestedObjectDepth);
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
        /// Gets the paged list of Departmentes.
        /// </summary>
        /// <param name="pageNo">page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>List of Departments</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Department>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route(ApiRouteConfiguration.Pagination), HttpGet]
        public IActionResult GetPagedList([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            return base.GetPagedList(pageNo, pageSize);
        }

        /// <summary>
        /// Deletes department with the specified Id.
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
        /// Insert the specified department.
        /// </summary>
        /// <param name="departmentDto">The department dto.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Inserted otherwise error message)
        /// </returns>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Department))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost]
        public IActionResult Post([FromBody] DepartmentDto departmentDto)
        {
            Department department = _mapper.Map<DepartmentDto, Department>(departmentDto);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPut(ApiRouteConfiguration.IdSuffix)]
        public IActionResult Put(int id, [FromBody] DepartmentDto departmentDto)
        {
            Department department = _mapper.Map<DepartmentDto, Department>(departmentDto);
            return base.Put(id, department);
        }


        /// <summary>
        /// Gets Audit List of department with the specified Id.
        /// its provide All operations performed on this department
        /// </summary>
        /// <param name="id">The Department ID.</param>
        /// <returns>List of department Audits</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DepartmentAudit>))]
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
        /// <returns>Audit line of Department</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DepartmentAudit))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(ApiRouteConfiguration.AuditIdSuffix)]
        public IActionResult AuditId(int id)
        {
            return base.AuditId(id);
        }

        /// <summary>
        /// Restore audit Values to current Department.
        /// </summary>
        /// <param name="id">Department Id, To restore deleted entity make id=0.</param>
        /// <param name="auditId">Department audit ID.</param>
        /// <param name="content">Empty http content.</param>
        /// <returns>Http Response with statut code (OK if is Restored otherwise error message) </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut(ApiRouteConfiguration.AuditRestoreSuffix)]
        public IActionResult RestoreAudit(int id, int auditId)
        {
            _logger.LogInformation($"RestoreAudit for {typeof(Department).Name} with Id {id} from {typeof(DepartmentAudit).Name} with Id {auditId}");
            

            DepartmentAudit departmentAudit = _DepartmentAuditService.GetById(auditId);
            if (departmentAudit == null )
                throw new IdNotFoundException($"No element found for this auditId {auditId}");

            Department department = _DepartmentService.GetById(id);

            if (id == 0) // restore deleted department
            {
                department = new Department();
                _DepartmentService.Restore(department, departmentAudit);
                _DepartmentService.Add(department);
            }
            else
            {
                if (department == null)
                    throw new IdNotFoundException($"No element found for this Id {id}");
                if (department.ID != departmentAudit.ID)
                    throw new BadRequestException($"Invalid Audit for this Id {id} and auditId {auditId}");

                _DepartmentService.Restore(department, departmentAudit);
                _DepartmentService.Update(department);
            }

            // model state test
            if (!_DepartmentService.GetValidationDictionary().IsValid() || !ModelState.IsValid)
            {
                AddServiceErrorsToModelState(_DepartmentService.GetValidationDictionary());
                throw new ModelStateException("One or more validation errors occurred.", _DepartmentService.GetValidationDictionary().ToReadOnlyDictionary());
            }

            _DepartmentService.Save();

            return Ok();
        }

        /// <summary>
        /// Gets the snapshot of all entities at the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time in the yyyyMMddTHHmmss format.</param>
        /// <returns>
        /// List of Department as Json response
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DepartmentAudit>))]
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
        /// Department as Json response
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Department))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(ApiRouteConfiguration.SnapshotIdSuffix)]
        public IActionResult GetSnapshot(string dateTime, int id)
        {
            return base.GetSnapshot(dateTime, id);
        }

        /// <summary>
        /// Insert the specified department List.
        /// </summary>
        /// <param name="departmentDtoList">The department dto list.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Inserted otherwise error message)
        /// </returns>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<Department>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost(ApiRouteConfiguration.ItemsSuffix)]
        public IActionResult PostList([FromBody] IEnumerable<DepartmentDto> departmentDtoList)
        {
            _logger.LogInformation($"PostList {typeof(Department).Name} count : {departmentDtoList?.Count()}");

            IEnumerable<Department> departmentList = _mapper.Map<IEnumerable<DepartmentDto>, IEnumerable<Department>>(departmentDtoList);

            // model state test
            if (!_DepartmentService.HugeInsertValidation(departmentList) || !ModelState.IsValid)
            {
                AddServiceErrorsToModelState(_DepartmentService.GetValidationDictionary());
                throw new ModelStateException("One or more validation errors occurred.", _DepartmentService.GetValidationDictionary().ToReadOnlyDictionary());
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Department))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(ApiRouteConfiguration.SearchSuffix)]
        public IActionResult GetByName([FromQuery] string name)
        {
            Department department = _DepartmentService.Get(c => c.Name == name);
            if (department != null)
            {
                return Ok(department);
            }
            else
            {
                throw new NoElementFoundException($"No element found for this name {name}");
            }
        }
    }
}