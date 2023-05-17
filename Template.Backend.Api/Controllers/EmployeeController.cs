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
    /// Route="api/v1/Employees"
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route(ApiRouteConfiguration.EmployeePrefix)]
    public class EmployeeController : BaseApiController<Employee, EmployeeAudit>
    {
        private IEmployeeService _EmployeeService;
        private IEmployeeAuditService _EmployeeAuditService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        /// <param name="employeeService">The employee service.</param>
        /// <param name="employeeAuditService">The employee audit service.</param>
        public EmployeeController(IEmployeeService employeeService, IEmployeeAuditService employeeAuditService, IMapper mapper, ILogger<Employee> logger)
            : base(employeeService, employeeAuditService, mapper, logger)
        {
            _EmployeeService = employeeService;
            _EmployeeAuditService = employeeAuditService;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [Route(ApiRouteConfiguration.CountSuffix), HttpGet]
        public override IActionResult Count()
        {
            return base.Count();
        }

        /// <summary>
        /// Gets All employees
        /// </summary>
        /// <returns>List of employees</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Employee>))]
        [HttpGet]
        public IActionResult Get()
        {
            return base.Get();
        }

        /// <summary>
        /// Gets Employee by Id.
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>employee</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
        [HttpGet(ApiRouteConfiguration.IdSuffix)]
        public IActionResult Get(int id)
        {
            return base.Get(id);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">Employee ID .</param>
        /// <param name="nestedObjectDepth">The maximum level to achieve for navigation properties serialization.</param>
        /// <returns>employee</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
        [HttpGet(ApiRouteConfiguration.IdDepthSuffix)]
        public IActionResult Get(int id, NestedObjectDepth nestedObjectDepth)
        {
            _logger.LogInformation($"Get {typeof(Employee).Name} with Id {id} and {nestedObjectDepth}");
            var entity = _EmployeeService.FindById(id, nestedObjectDepth);
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
        /// Gets the paged list of Employeees.
        /// </summary>
        /// <param name="pageNo">page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>List of Employees</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Employee>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route(ApiRouteConfiguration.Pagination), HttpGet]
        public IActionResult GetPagedList([FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            return base.GetPagedList(pageNo, pageSize);
        }

        /// <summary>
        /// Deletes employee with the specified Id.
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
        /// Insert the specified employee.
        /// </summary>
        /// <param name="employeeDto">The employee dto.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Inserted otherwise error message)
        /// </returns>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Employee))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost]
        public IActionResult Post([FromBody] EmployeeDto employeeDto)
        {
            Employee employee = _mapper.Map<EmployeeDto, Employee>(employeeDto);
            return base.Post(employee);
        }

        /// <summary>
        /// Update the employee with the specified ID.
        /// </summary>
        /// <param name="id">the employee ID to Update.</param>
        /// <param name="employeeDto">The employee dto.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Updated otherwise error message)
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPut(ApiRouteConfiguration.IdSuffix)]
        public IActionResult Put(int id, [FromBody] EmployeeDto employeeDto)
        {
            Employee employee = _mapper.Map<EmployeeDto, Employee>(employeeDto);
            return base.Put(id, employee);
        }


        /// <summary>
        /// Gets Audit List of employee with the specified Id.
        /// its provide All operations performed on this employee
        /// </summary>
        /// <param name="id">The Employee ID.</param>
        /// <returns>List of employee Audits</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeAudit>))]
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
        /// <returns>Audit line of Employee</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeAudit))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(ApiRouteConfiguration.AuditIdSuffix)]
        public IActionResult AuditId(int id)
        {
            return base.AuditId(id);
        }

        /// <summary>
        /// Restore audit Values to current Employee.
        /// </summary>
        /// <param name="id">Employee Id, To restore deleted entity make id=0.</param>
        /// <param name="auditId">Employee audit ID.</param>
        /// <param name="content">Empty http content.</param>
        /// <returns>Http Response with statut code (OK if is Restored otherwise error message) </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut(ApiRouteConfiguration.AuditRestoreSuffix)]
        public IActionResult RestoreAudit(int id, int auditId)
        {
            _logger.LogInformation($"RestoreAudit for {typeof(Employee).Name} with Id {id} from {typeof(EmployeeAudit).Name} with Id {auditId}");
            

            EmployeeAudit employeeAudit = _EmployeeAuditService.GetById(auditId);
            if (employeeAudit == null )
                throw new IdNotFoundException($"No element found for this auditId {auditId}");

            Employee employee = _EmployeeService.GetById(id);

            if (id == 0) // restore deleted employee
            {
                employee = new Employee();
                _EmployeeService.Restore(employee, employeeAudit);
                _EmployeeService.Add(employee);
            }
            else
            {
                if (employee == null)
                    throw new IdNotFoundException($"No element found for this Id {id}");
                if (employee.ID != employeeAudit.ID)
                    throw new BadRequestException($"Invalid Audit for this Id {id} and auditId {auditId}");

                _EmployeeService.Restore(employee, employeeAudit);
                _EmployeeService.Update(employee);
            }

            // model state test
            if (!_EmployeeService.GetValidationDictionary().IsValid() || !ModelState.IsValid)
            {
                AddServiceErrorsToModelState(_EmployeeService.GetValidationDictionary());
                throw new ModelStateException("One or more validation errors occurred.", _EmployeeService.GetValidationDictionary().ToReadOnlyDictionary());
            }

            _EmployeeService.Save();

            return Ok();
        }

        /// <summary>
        /// Gets the snapshot of all entities at the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time in the yyyyMMddTHHmmss format.</param>
        /// <returns>
        /// List of Employee as Json response
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeAudit>))]
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
        /// Employee as Json response
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(ApiRouteConfiguration.SnapshotIdSuffix)]
        public IActionResult GetSnapshot(string dateTime, int id)
        {
            return base.GetSnapshot(dateTime, id);
        }

        /// <summary>
        /// Insert the specified employee List.
        /// </summary>
        /// <param name="employeeDtoList">The employee dto list.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Inserted otherwise error message)
        /// </returns>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<Employee>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost(ApiRouteConfiguration.ItemsSuffix)]
        public IActionResult PostList([FromBody] IEnumerable<EmployeeDto> employeeDtoList)
        {
            _logger.LogInformation($"PostList {typeof(Employee).Name} count : {employeeDtoList?.Count()}");

            IEnumerable<Employee> employeeList = _mapper.Map<IEnumerable<EmployeeDto>, IEnumerable<Employee>>(employeeDtoList);

            // model state test
            if (!_EmployeeService.HugeInsertValidation(employeeList) || !ModelState.IsValid)
            {
                AddServiceErrorsToModelState(_EmployeeService.GetValidationDictionary());
                throw new ModelStateException("One or more validation errors occurred.", _EmployeeService.GetValidationDictionary().ToReadOnlyDictionary());
            }

            return base.PostList(employeeList);
        }

        /// <summary>
        /// Gets Employee by Name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Employee
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(ApiRouteConfiguration.SearchSuffix)]
        public IActionResult GetByName([FromQuery] string name)
        {
            Employee employee = _EmployeeService.Get(c => c.Name == name);
            if (employee != null)
            {
                return Ok(employee);
            }
            else
            {
                throw new NoElementFoundException($"No element found for this name {name}");
            }
        }

        /// <summary>
        /// Gets PagedList of Employee with specified Search criteria.
        /// </summary>
        /// <param name="pageNo">The page no.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="searchEmployeeDto">The search employee dto.</param>
        /// <returns>
        /// Employee list
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Employee>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost(ApiRouteConfiguration.SearchSuffix)]
        public IActionResult Search([FromQuery] int pageNo, [FromQuery] int pageSize, [FromBody] SearchEmployeeDto searchEmployeeDto)
        {
            _logger.LogInformation($"Search Paged {typeof(Employee).Name}");
            Employee searchEmployee = _mapper.Map<EmployeeDto, Employee>(searchEmployeeDto);
            IEnumerable<Employee> employeeList = _EmployeeService.Search(searchEmployee,
                        searchEmployeeDto.startBirthDate, searchEmployeeDto.endBirthDate, pageNo, pageSize);

            if (employeeList != null && employeeList.Any())
            {
                int count = _EmployeeService.SearchCount(searchEmployee,
                        searchEmployeeDto.startBirthDate, searchEmployeeDto.endBirthDate);
                AddCountHeaders(employeeList, count);
                return Ok(employeeList);
            }
            else
            {
                throw new NoElementFoundException($"No element found");
            }
        }
    }
}