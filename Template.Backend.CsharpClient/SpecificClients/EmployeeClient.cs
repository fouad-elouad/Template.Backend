using Template.Backend.CsharpClient.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Template.Backend.Model.Entities;
using Template.Backend.Model.Audit.Entities;
using Template.Backend.Model.Exceptions;
using System.Threading.Tasks;

namespace Template.Backend.CsharpClient.SpecificClients
{
    /// <summary>
    /// EmployeeClient interface
    /// </summary>
    /// <seealso cref="Template.Backend.CsharpClient.IClient{Employee, EmployeeAudit}" />
    public interface IEmployeeClient : IClient<Employee, EmployeeAudit>
    {
        /// <summary>
        /// Adds the specified Employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        Employee Add(Employee employee, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Adds the specified Employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        Task<Employee> AddAsync(Employee employee, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets all Employee.
        /// </summary>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Employee
        /// </returns>
        IEnumerable<Employee> GetAll(AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets all Employee.
        /// </summary>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Employee
        /// </returns>
        Task<IEnumerable<Employee>> GetAllAsync(AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets Employee with the specified Id.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <param name="depth">The maximum level to achieve for navigation properties serialization.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Employee object
        /// </returns>
        Employee Get(int id, int depth = 1, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets Employee with the specified Id.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <param name="depth">The maximum level to achieve for navigation properties serialization.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Employee object
        /// </returns>
        Task<Employee> GetAsync(int id, int depth = 1, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Deletes Employee with the specified Id.
        /// </summary>
        /// <param name="ID">The Employee Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        void Delete(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Deletes Employee with the specified Id.
        /// </summary>
        /// <param name="ID">The Employee Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        Task DeleteAsync(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Updates Employee with specified Id.
        /// </summary>
        /// <param name="ID">The Employee Id.</param>
        /// <param name="employee">The employee.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        void Update(int ID, Employee employee, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Updates Employee with specified Id.
        /// </summary>
        /// <param name="ID">The Employee Id.</param>
        /// <param name="employee">The employee.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        Task UpdateAsync(int ID, Employee employee, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the audit list of Employee.
        /// </summary>
        /// <param name="ID">The Employee Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        IEnumerable<EmployeeAudit> GetAuditListById(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the audit list of Employee.
        /// </summary>
        /// <param name="ID">The Employee Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        Task<IEnumerable<EmployeeAudit>> GetAuditListByIdAsync(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the audit row by Id.
        /// </summary>
        /// <param name="ID">The EmployeeAudit Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// EmployeeAudit object
        /// </returns>
        EmployeeAudit GetAuditById(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the audit row by Id.
        /// </summary>
        /// <param name="ID">The EmployeeAudit Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// EmployeeAudit object
        /// </returns>
        Task<EmployeeAudit> GetAuditByIdAsync(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Restores the Employee by the spicified EmployeeAudit Id.
        /// </summary>
        /// <param name="id">The Employee Id.</param>
        /// <param name="auditId">The EmployeeAudit Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        void Restore(int id, int auditId, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Restores the Employee by the spicified EmployeeAudit Id.
        /// </summary>
        /// <param name="id">The Employee Id.</param>
        /// <param name="auditId">The EmployeeAudit Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        Task RestoreAsync(int id, int auditId, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the paged list of Employee.
        /// </summary>
        /// <param name="pageNo">The page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Employee object
        /// </returns>
        IEnumerable<Employee> GetPagedList(int pageNo, int pageSize, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the paged list of Employee.
        /// </summary>
        /// <param name="pageNo">The page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Employee object
        /// </returns>
        Task<IEnumerable<Employee>> GetPagedListAsync(int pageNo, int pageSize, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the snapshot of all entities at the specified date time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List snapshot
        /// </returns>
        IEnumerable<Employee> GetAllSnapshot(DateTime datetime, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the snapshot of all entities at the specified date time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List snapshot
        /// </returns>
        Task<IEnumerable<Employee>> GetAllSnapshotAsync(DateTime datetime, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the snapshot at the specified date time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="id">The entity id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Employee snapshot
        /// </returns>
        Employee GetIdSnapshot(DateTime datetime, int id, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the snapshot at the specified date time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="id">The entity id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Employee snapshot
        /// </returns>
        Task<Employee> GetIdSnapshotAsync(DateTime datetime, int id, AuthenticationHeaderValue authHeaderValue = null);
    }

    /// <summary>
    /// EmployeeClient Class
    /// </summary>
    /// <seealso cref="Template.Backend.CsharpClient.Client{Employee, EmployeeAudit}" />
    /// <seealso cref="Template.Backend.CsharpClient.SpecificClients.IEmployeeClient" />
    public class EmployeeClient : Client<Employee, EmployeeAudit>, IEmployeeClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeClient"/> class.
        /// </summary>
        public EmployeeClient()
        {
        }

        public Employee Add(Employee employee, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string values = ToJson(employee);
                return Add(ApiConfiguration.EmployeeApiRoute, values, authHeaderValue);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Employee> AddAsync(Employee employee, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string values = ToJson(employee);
                return await AddAsync(ApiConfiguration.EmployeeApiRoute, values, authHeaderValue);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Employee> GetAll(AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetObjects(ApiConfiguration.EmployeeApiRoute, authHeaderValue);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetObjectsAsync(ApiConfiguration.EmployeeApiRoute, authHeaderValue);
        }

        public void Delete(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
             Delete(ApiConfiguration.EmployeeApiRoute + ID.ToString(), authHeaderValue);
        }

        public async Task DeleteAsync(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
           await DeleteAsync(ApiConfiguration.EmployeeApiRoute + ID.ToString(), authHeaderValue);
        }

        public Employee Get(int id, int depth = 1, AuthenticationHeaderValue authHeaderValue = null)
        {
            if (depth != 1)
                return GetAsObject(ApiConfiguration.EmployeeApiRoute + id.ToString() + "/" + depth.ToString(), authHeaderValue);

            return GetAsObject(ApiConfiguration.EmployeeApiRoute + id.ToString(), authHeaderValue);
        }

        public async Task<Employee> GetAsync(int id, int depth = 1, AuthenticationHeaderValue authHeaderValue = null)
        {
            if (depth != 1)
                return await GetAsObjectAsync(ApiConfiguration.EmployeeApiRoute + id.ToString() + "/" + depth.ToString(), authHeaderValue);

            return await GetAsObjectAsync(ApiConfiguration.EmployeeApiRoute + id.ToString(), authHeaderValue);
        }

        public IEnumerable<Employee> GetPagedList(int pageNo, int pageSize, AuthenticationHeaderValue authHeaderValue = null)
        {
            string url = string.Empty.AddQuery(nameof(pageNo), pageNo.ToString())
                                     .AddQuery(nameof(pageSize), pageSize.ToString());

            return GetObjects(ApiConfiguration.EmployeeApiRoute + "?" + url, authHeaderValue);
        }

        public async Task<IEnumerable<Employee>> GetPagedListAsync(int pageNo, int pageSize, AuthenticationHeaderValue authHeaderValue = null)
        {
            string url = string.Empty.AddQuery(nameof(pageNo), pageNo.ToString())
                                     .AddQuery(nameof(pageSize), pageSize.ToString());

            return await GetObjectsAsync(ApiConfiguration.EmployeeApiRoute + "?" + url, authHeaderValue);
        }

        public void Update(int ID, Employee employee, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string values = ToJson(employee, _listDepth);
                base.Update(ApiConfiguration.EmployeeApiRoute + ID.ToString(), values, authHeaderValue);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(int ID, Employee employee, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string values = ToJson(employee, _listDepth);
                await base.UpdateAsync(ApiConfiguration.EmployeeApiRoute + ID.ToString(), values, authHeaderValue);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<EmployeeAudit> GetAuditListById(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetAuditObjects(ApiConfiguration.EmployeeApiAuditListRoute + ID.ToString(), authHeaderValue);
        }

        public async Task<IEnumerable<EmployeeAudit>> GetAuditListByIdAsync(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetAuditObjectsAsync(ApiConfiguration.EmployeeApiAuditListRoute + ID.ToString(), authHeaderValue);
        }

        public EmployeeAudit GetAuditById(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetAudit(ApiConfiguration.EmployeeApiAuditRoute + ID.ToString(), authHeaderValue);
        }

        public async Task<EmployeeAudit> GetAuditByIdAsync(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetAuditAsync(ApiConfiguration.EmployeeApiAuditRoute + ID.ToString(), authHeaderValue);
        }

        public void Restore(int id, int auditId, AuthenticationHeaderValue authHeaderValue = null)
        {
            Restore(ApiConfiguration.EmployeeApiAuditListRoute + id.ToString() + "/" + auditId.ToString(), authHeaderValue);
        }

        public async Task RestoreAsync(int id, int auditId, AuthenticationHeaderValue authHeaderValue = null)
        {
           await RestoreAsync(ApiConfiguration.EmployeeApiAuditListRoute + id.ToString() + "/" + auditId.ToString(), authHeaderValue);
        }

        public IEnumerable<Employee> GetAllSnapshot(DateTime datetime, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string dateFormatted = datetime.ToString(_dateFormat);
                return GetObjects(ApiConfiguration.EmployeeApiSnapshotRoute + dateFormatted, authHeaderValue);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Employee>> GetAllSnapshotAsync(DateTime datetime, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string dateFormatted = datetime.ToString(_dateFormat);
                return await GetObjectsAsync(ApiConfiguration.EmployeeApiSnapshotRoute + dateFormatted, authHeaderValue);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Employee GetIdSnapshot(DateTime datetime, int id, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string dateFormatted = datetime.ToString(_dateFormat);
                return GetAsObject(ApiConfiguration.EmployeeApiSnapshotRoute + dateFormatted + "/" + id.ToString(), authHeaderValue);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Employee> GetIdSnapshotAsync(DateTime datetime, int id, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string dateFormatted = datetime.ToString(_dateFormat);
                return await GetAsObjectAsync(ApiConfiguration.EmployeeApiSnapshotRoute + dateFormatted + "/" + id.ToString(), authHeaderValue);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}