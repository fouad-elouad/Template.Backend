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
    /// Route="api/v1/Employees"
    /// </summary>
    [ApiVersion("1")]
    [RoutePrefix(ApiRouteConfiguration.EmployeePrefix)]
    public class EmployeeApiController : BaseApiController<Employee, EmployeeAudit>
    {
        private IEmployeeService _EmployeeService;
        private IEmployeeAuditService _EmployeeAuditService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeApiController"/> class.
        /// </summary>
        /// <param name="employeeService">The employee service.</param>
        /// <param name="employeeAuditService">The employee audit service.</param>
        /// <param name="mapper">The mapper.</param>
        public EmployeeApiController(IEmployeeService employeeService, IEmployeeAuditService employeeAuditService, IMapper mapper)
            : base(employeeService, employeeAuditService, mapper)
        {
            _EmployeeService = employeeService;
            _EmployeeAuditService = employeeAuditService;
        }

        /// <summary>
        /// Count
        /// </summary>
        /// <returns>Count</returns>
        [ResponseType(typeof(int))]
        [Route(ApiRouteConfiguration.CountSuffix), HttpGet]
        public override IHttpActionResult Count()
        {
            _logger.Info("API: HttpGet Count Employee");
            return base.Count();
        }

        /// <summary>
        /// Gets All employees
        /// </summary>
        /// <returns>List of employees</returns>
        [ResponseType(typeof(IEnumerable<Employee>))]
        [Route(), HttpGet]
        public override IHttpActionResult Get()
        {
            _logger.Info("API: HttpGet Get Employee");
            return base.Get();
        }

        /// <summary>
        /// Gets Employee by Id.
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>employee</returns>
        [ResponseType(typeof(Employee))]
        [Route(ApiRouteConfiguration.IdSuffix), HttpGet]
        public override IHttpActionResult Get(int id)
        {
            _logger.Info("API: HttpGet Get(id) Employee id={0}", id);
            return base.Get(id);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">Employee ID .</param>
        /// <param name="depth">The maximum level to achieve for navigation properties serialization.</param>
        /// <returns>employee</returns>
        [ResponseType(typeof(Employee))]
        [Route(ApiRouteConfiguration.IdDepthSuffix), HttpGet]
        public override IHttpActionResult Get(int id, int depth)
        {
            _logger.Info("API: HttpGet Get(id,depth) Employee id={0}, depth={1}", id, depth);
            return base.Get(id, depth);
        }

        /// <summary>
        /// Gets the paged list of Employeees.
        /// </summary>
        /// <param name="pageNo">page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>List of Employees</returns>
        [ResponseType(typeof(IEnumerable<Employee>))]
        [Route(), HttpGet]
        public override IHttpActionResult GetPagedList([FromUri] int pageNo, int pageSize)
        {
            _logger.Info("API: HttpGet GetPagedList Employee");
            return base.GetPagedList(pageNo, pageSize);
        }

        /// <summary>
        /// Deletes employee with the specified Id.
        /// </summary>
        /// <param name="id">ID to Delete</param>
        /// <returns>Http Response with statut code (NoContent (204) if is Deleted otherwise error message)</returns>
        [Route(ApiRouteConfiguration.IdSuffix), HttpDelete]
        public override IHttpActionResult Delete(int id)
        {
            _logger.Info("API: HttpDelete Delete Employee id={0}", id);
            return base.Delete(id);
        }

        /// <summary>
        /// Insert the specified employee.
        /// </summary>
        /// <param name="employeeDto">The employee dto.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Inserted otherwise error message)
        /// </returns>
        [ResponseType(typeof(Employee))]
        [Route(), HttpPost]
        public IHttpActionResult Post([FromBody] EmployeeDto employeeDto)
        {
            _logger.Info("API: HttpPost Post Employee");
            Employee employee = Mapper.Map<EmployeeDto, Employee>(employeeDto);
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
        [Route(ApiRouteConfiguration.IdSuffix), HttpPut]
        public IHttpActionResult Put(int id, EmployeeDto employeeDto)
        {
            _logger.Info("API: HttpPut Put Employee id={0}", id);
            if (_EmployeeService.CheckIfExist(o => o.ID == id))
            {
                Employee employee = Mapper.Map<EmployeeDto, Employee>(employeeDto);
                return base.Put(id, employee);
            }
            else
            {
                return ResponseMessage(ApiExceptionResponse.Throw(
                    new IdNotFoundException("No element found for this id"), Request));
            }
        }

        /// <summary>
        /// Gets Audit List of employee with the specified Id.
        /// its provide All operations performed on this employee
        /// </summary>
        /// <param name="id">The Employee ID.</param>
        /// <returns>List of employee Audits</returns>
        [ResponseType(typeof(IEnumerable<EmployeeAudit>))]
        [Route(ApiRouteConfiguration.AuditSuffix), HttpGet]
        public IHttpActionResult Audit(int id)
        {
            _logger.Info("API: HttpGet Audit Employee id={0}", id);

            IEnumerable<EmployeeAudit> employeeAudit = _EmployeeAuditService.GetAuditById(id);

            if (employeeAudit != null && employeeAudit.Any())
            {
                return ResponseMessage(AuditToJsonResponse(employeeAudit, 2));
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
        /// <returns>Audit line of Employee</returns>
        [ResponseType(typeof(EmployeeAudit))]
        [Route(ApiRouteConfiguration.AuditIdSuffix), HttpGet]
        public override IHttpActionResult AuditId(int id)
        {
            _logger.Info("API: HttpGet AuditId Employee id={0}", id);
            return base.AuditId(id);
        }

        /// <summary>
        /// Restore audit Values to current Employee.
        /// </summary>
        /// <param name="id">Employee Id.</param>
        /// <param name="auditId">Employee audit ID.</param>
        /// <param name="content">Empty http content.</param>
        /// <returns>Http Response with statut code (OK if is Restored otherwise error message) </returns>
        [Route(ApiRouteConfiguration.AuditRestoreSuffix), HttpPut]
        public IHttpActionResult PutRestoreAudit(int id, int auditId, [FromBody] string content)
        {
            _logger.Info("API: HttpPut PutRestoreAudit Employee id={0}, auditId={1}", id, auditId);
            Employee employee = _EmployeeService.GetById(id);

            EmployeeAudit employeeAudit = _EmployeeAuditService.GetById(auditId);
            if (employee == null)
            {
                employee = new Employee();
                _EmployeeService.Restore(employee, employeeAudit);
                _EmployeeService.Add(employee);
            }
            else
            {
                _EmployeeService.Restore(employee, employeeAudit);
            }

            _EmployeeService.Save(User.Identity.Name);

            return Ok();
        }

        /// <summary>
        /// Gets the snapshot of all entities at the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time in the yyyyMMddTHHmmss format.</param>
        /// <returns>
        /// List of Employee as Json response
        /// </returns>
        [ResponseType(typeof(IEnumerable<EmployeeAudit>))]
        [Route(ApiRouteConfiguration.SnapshotSuffix), HttpGet]
        public override IHttpActionResult GetSnapshot(string dateTime)
        {
            _logger.Info("API: HttpGet GetSnapshot Employee date={0}", dateTime);
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
        [ResponseType(typeof(Employee))]
        [Route(ApiRouteConfiguration.SnapshotIdSuffix), HttpGet]
        public override IHttpActionResult GetSnapshot(string dateTime, int id)
        {
            _logger.Info("API: HttpGet GetSnapshot Employee dateTime={0}, id={1}", dateTime, id);
            return base.GetSnapshot(dateTime, id);
        }

        /// <summary>
        /// Insert the specified employee List.
        /// </summary>
        /// <param name="employeeDtoList">The employee dto list.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Inserted otherwise error message)
        /// </returns>
        [ResponseType(typeof(IEnumerable<Employee>))]
        [Route(ApiRouteConfiguration.ItemsSuffix), HttpPost]
        public IHttpActionResult PostList([FromBody] IEnumerable<EmployeeDto> employeeDtoList)
        {
            _logger.Info("API: HttpPost Post EmployeeList");
            IEnumerable<Employee> employeeList = Mapper.Map<IEnumerable<EmployeeDto>, IEnumerable<Employee>>(employeeDtoList);
            if (!_EmployeeService.HugeInsertValidation(employeeList))
            {
                AddServiceErrorsToModelState(_EmployeeService.GetValidationDictionary());
                return ResponseMessage(InvalidModelStateToJsonResponse(_EmployeeService.GetValidationDictionary().ToDictionary()));
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
        [ResponseType(typeof(Employee))]
        [Route(), HttpGet]
        public IHttpActionResult GetByName([FromUri] string name)
        {
            _logger.Info("API: HttpGet GetByName Employee name={0}", name);
            Employee employee = _EmployeeService.Get(c => c.Name == name);
            if (employee != null)
            {
                return ResponseMessage(ToJsonResponse(employee, HttpStatusCode.OK, _detailsDepth));
            }
            return ResponseMessage(ApiExceptionResponse.Throw(new NoElementFoundException("No element found for this shorName"), Request));
        }

        /// <summary>
        /// Gets Employee list by companyID.
        /// </summary>
        /// <param name="companyID">The companyID.</param>
        /// <returns>
        /// Employee list
        /// </returns>
        [ResponseType(typeof(IEnumerable<Employee>))]
        [Route(), HttpGet]
        public IHttpActionResult GetByCompany([FromUri] int companyID)
        {
            _logger.Info("API: HttpGet GetByCompany Employee companyID={0}", companyID);
            IEnumerable<Employee> employeeList = _EmployeeService.GetMany(c => c.CompanyID == companyID);
            if (employeeList != null && employeeList.Any())
            {
                return ResponseMessage(ToJsonResponse(employeeList, HttpStatusCode.OK, _detailsDepth));
            }
            return ResponseMessage(ApiExceptionResponse.Throw(new NoElementFoundException("No element found for this companyID"), Request));
        }

        /// <summary>
        /// Gets Employee list by departmentID.
        /// </summary>
        /// <param name="departmentID">The departmentID.</param>
        /// <returns>
        /// Employee list
        /// </returns>
        [ResponseType(typeof(IEnumerable<Employee>))]
        [Route(), HttpGet]
        public IHttpActionResult GetByDepartment([FromUri] int departmentID)
        {
            _logger.Info("API: HttpGet GetByDepartment Employee departmentID={0}", departmentID);
            IEnumerable<Employee> employeeList = _EmployeeService.GetMany(c => c.DepartmentID == departmentID);
            if (employeeList != null && employeeList.Any())
            {
                return ResponseMessage(ToJsonResponse(employeeList, HttpStatusCode.OK, _detailsDepth));
            }
            return ResponseMessage(ApiExceptionResponse.Throw(new NoElementFoundException("No element found for this departmentID"), Request));
        }

        /// <summary>
        /// Gets List of Employee with specified Search criteria.
        /// </summary>
        /// <param name="searchEmployeeDto">The search employee dto.</param>
        /// <returns>
        /// Employee list
        /// </returns>
        [ResponseType(typeof(IEnumerable<Employee>))]
        [Route(ApiRouteConfiguration.SearchSuffix), HttpPost]
        public IHttpActionResult Search([FromBody] SearchEmployeeDto searchEmployeeDto)
        {
            _logger.Info("API: HttpPost Search Employee");
            Employee searchEmployee = Mapper.Map<EmployeeDto, Employee>(searchEmployeeDto);
            IEnumerable<Employee> employeeList = _EmployeeService.Search(searchEmployee,
                                            searchEmployeeDto.startBirthDate, searchEmployeeDto.endBirthDate);

            if (employeeList != null && employeeList.Any())
            {
                return ResponseMessage(ToJsonResponse(employeeList, HttpStatusCode.OK, _detailsDepth));
            }
            return ResponseMessage(ApiExceptionResponse.Throw(new NoElementFoundException("No element found"), Request));
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
        [ResponseType(typeof(IEnumerable<Employee>))]
        [Route(ApiRouteConfiguration.SearchSuffix), HttpPost]
        public IHttpActionResult Search([FromUri] int pageNo, int pageSize, [FromBody] SearchEmployeeDto searchEmployeeDto)
        {
            _logger.Info("API: HttpPost Search Employee");
            Employee searchEmployee = Mapper.Map<EmployeeDto, Employee>(searchEmployeeDto);
            IEnumerable<Employee> employeeList = _EmployeeService.Search(searchEmployee,
                        searchEmployeeDto.startBirthDate, searchEmployeeDto.endBirthDate, pageNo, pageSize);

            if (employeeList != null && employeeList.Any())
            {
                int count = _EmployeeService.SearchCount(searchEmployee,
                        searchEmployeeDto.startBirthDate, searchEmployeeDto.endBirthDate);
                return ResponseMessage(ToJsonResponse(employeeList, HttpStatusCode.OK, _detailsDepth, count));
            }
            return ResponseMessage(ApiExceptionResponse.Throw(new NoElementFoundException("No element found"), Request));
        }
    }
}