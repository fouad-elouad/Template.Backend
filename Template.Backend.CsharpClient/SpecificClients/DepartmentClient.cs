using Template.Backend.CsharpClient.Helpers;
using System.Net.Http.Headers;
using Template.Backend.Model.Entities;
using Template.Backend.Model.Audit.Entities;
using Template.Backend.Model.Exceptions;
using System.Text.Json;
using Template.Backend.Model.Enums;

namespace Template.Backend.CsharpClient.SpecificClients
{
    /// <summary>
    /// DepartmentClient interface
    /// </summary>
    /// <seealso cref="Template.Backend.CsharpClient.IClient{Department, DepartmentAudit}" />
    public interface IDepartmentClient : IClient<Department, DepartmentAudit>
    {
        /// <summary>
        /// Adds the specified Department.
        /// </summary>
        /// <param name="department">The department.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        Department Add(Department department, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Adds the specified Department.
        /// </summary>
        /// <param name="department">The department.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        Task<Department> AddAsync(Department department, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Adds the specified Department List.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// list
        /// </returns>
        IEnumerable<Department> Add(IEnumerable<Department> list, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Adds the specified Department List.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// list
        /// </returns>
        Task<IEnumerable<Department>> AddAsync(IEnumerable<Department> list, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets all Department.
        /// </summary>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Department
        /// </returns>
        IEnumerable<Department> GetAll(AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets all Department.
        /// </summary>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Department
        /// </returns>
        Task<IEnumerable<Department>> GetAllAsync(AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets Department with the specified Id.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Department object
        /// </returns>
        Department Get(int id, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets Department with the specified Id.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Department object
        /// </returns>
        Task<Department> GetAsync(int id, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Count.
        /// </summary>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Count
        /// </returns>
        int Count(AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Count.
        /// </summary>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Count
        /// </returns>
        Task<int> CountAsync(AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets Department with the specified Id.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <param name="nestedObjectDepth">The maximum level to achieve for navigation properties serialization.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>Department object</returns>
        Department Get(int id, NestedObjectDepth nestedObjectDepth, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets Department with the specified Id.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <param name="nestedObjectDepth">The maximum level to achieve for navigation properties serialization.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>Department object</returns>
        Task<Department> GetAsync(int id, NestedObjectDepth nestedObjectDepth, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Deletes Department with the specified Id.
        /// </summary>
        /// <param name="ID">The Department Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        void Delete(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Deletes Department with the specified Id.
        /// </summary>
        /// <param name="ID">The Department Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        Task DeleteAsync(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Updates Department with specified Id.
        /// </summary>
        /// <param name="ID">The Department Id.</param>
        /// <param name="department">The department.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        void Update(int ID, Department department, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Updates Department with specified Id.
        /// </summary>
        /// <param name="ID">The Department Id.</param>
        /// <param name="department">The department.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        Task UpdateAsync(int ID, Department department, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the audit list of Department.
        /// </summary>
        /// <param name="ID">The Department Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        IEnumerable<DepartmentAudit> GetAuditListById(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the audit list of Department.
        /// </summary>
        /// <param name="ID">The Department Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        Task<IEnumerable<DepartmentAudit>> GetAuditListByIdAsync(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the audit row by Id.
        /// </summary>
        /// <param name="ID">The DepartmentAudit Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// DepartmentAudit object
        /// </returns>
        DepartmentAudit GetAuditById(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the audit row by Id.
        /// </summary>
        /// <param name="ID">The DepartmentAudit Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// DepartmentAudit object
        /// </returns>
        Task<DepartmentAudit> GetAuditByIdAsync(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Restores the Department by the spicified DepartmentAudit Id.
        /// </summary>
        /// <param name="id">The Department Id.</param>
        /// <param name="auditId">The DepartmentAudit Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        void Restore(int id, int auditId, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Restores the Department by the spicified DepartmentAudit Id.
        /// </summary>
        /// <param name="id">The Department Id.</param>
        /// <param name="auditId">The DepartmentAudit Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        Task RestoreAsync(int id, int auditId, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the paged list of Department.
        /// </summary>
        /// <param name="pageNo">The page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Department object
        /// </returns>
        IEnumerable<Department> GetPagedList(int pageNo, int pageSize, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the paged list of Department.
        /// </summary>
        /// <param name="pageNo">The page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Department object
        /// </returns>
        Task<IEnumerable<Department>> GetPagedListAsync(int pageNo, int pageSize, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the snapshot of all entities at the specified date time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List snapshot
        /// </returns>
        IEnumerable<Department> GetAllSnapshot(DateTime datetime, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the snapshot of all entities at the specified date time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List snapshot
        /// </returns>
        Task<IEnumerable<Department>> GetAllSnapshotAsync(DateTime datetime, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the snapshot at the specified date time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="id">The entity id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Department snapshot
        /// </returns>
        Department GetIdSnapshot(DateTime datetime, int id, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the snapshot at the specified date time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="id">The entity id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Department snapshot
        /// </returns>
        Task<Department> GetIdSnapshotAsync(DateTime datetime, int id, AuthenticationHeaderValue authHeaderValue = null);
    }

    /// <summary>
    /// DepartmentClient Class
    /// </summary>
    /// <seealso cref="Template.Backend.CsharpClient.Client{Department, DepartmentAudit}" />
    /// <seealso cref="Template.Backend.CsharpClient.SpecificClients.IDepartmentClient" />
    public class DepartmentClient : Client<Department, DepartmentAudit>, IDepartmentClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentClient"/> class.
        /// </summary>
        public DepartmentClient()
        {
        }

        public Department Add(Department department, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string values = JsonSerializer.Serialize(department);
                return Add(ApiConfiguration.DepartmentApiRoute, values, authHeaderValue);
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

        public async Task<Department> AddAsync(Department department, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string values = JsonSerializer.Serialize(department);
                return await AddAsync(ApiConfiguration.DepartmentApiRoute, values, authHeaderValue);
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

        public IEnumerable<Department> Add(IEnumerable<Department> list, AuthenticationHeaderValue authHeaderValue = null)
        {
            string values = JsonSerializer.Serialize(list);
            return AddRange(ApiConfiguration.DepartmentApiItemsRoute, values, authHeaderValue);
        }

        public async Task<IEnumerable<Department>> AddAsync(IEnumerable<Department> list, AuthenticationHeaderValue authHeaderValue = null)
        {
            string values = JsonSerializer.Serialize(list);
            return await AddRangeAsync(ApiConfiguration.DepartmentApiItemsRoute, values, authHeaderValue);
        }

        public int Count(AuthenticationHeaderValue authHeaderValue = null)
        {
            return Count(ApiConfiguration.DepartmentApiCountRoute, authHeaderValue);
        }

        public async Task<int> CountAsync(AuthenticationHeaderValue authHeaderValue = null)
        {
            return await CountAsync(ApiConfiguration.DepartmentApiCountRoute, authHeaderValue);
        }

        public IEnumerable<Department> GetAll(AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetObjects(ApiConfiguration.DepartmentApiRoute, authHeaderValue);
        }

        public async Task<IEnumerable<Department>> GetAllAsync(AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetObjectsAsync(ApiConfiguration.DepartmentApiRoute, authHeaderValue);
        }

        public void Delete(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            Delete(ApiConfiguration.DepartmentApiRoute + ID.ToString(), authHeaderValue);
        }

        public async Task DeleteAsync(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            await DeleteAsync(ApiConfiguration.DepartmentApiRoute + ID.ToString(), authHeaderValue);
        }

        public Department Get(int id, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetAsObject(ApiConfiguration.DepartmentApiRoute + id.ToString(), authHeaderValue);
        }

        public async Task<Department> GetAsync(int id, AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetAsObjectAsync(ApiConfiguration.DepartmentApiRoute + id.ToString(), authHeaderValue);
        }

        public Department Get(int id, NestedObjectDepth nestedObjectDepth, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetAsObject(ApiConfiguration.DepartmentApiRoute + id.ToString() + "/" + nestedObjectDepth.ToString(), authHeaderValue);
        }

        public async Task<Department> GetAsync(int id, NestedObjectDepth nestedObjectDepth, AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetAsObjectAsync(ApiConfiguration.DepartmentApiRoute + id.ToString() + "/" + nestedObjectDepth.ToString(), authHeaderValue);
        }

        public IEnumerable<Department> GetPagedList(int pageNo, int pageSize, AuthenticationHeaderValue authHeaderValue = null)
        {
            string url = string.Empty.AddQuery(nameof(pageNo), pageNo.ToString())
                                     .AddQuery(nameof(pageSize), pageSize.ToString());

            return GetObjects(ApiConfiguration.DepartmentApiPaginationRoute + "?" + url, authHeaderValue);
        }

        public async Task<IEnumerable<Department>> GetPagedListAsync(int pageNo, int pageSize, AuthenticationHeaderValue authHeaderValue = null)
        {
            string url = string.Empty.AddQuery(nameof(pageNo), pageNo.ToString())
                                     .AddQuery(nameof(pageSize), pageSize.ToString());

            return await GetObjectsAsync(ApiConfiguration.DepartmentApiPaginationRoute + "?" + url, authHeaderValue);
        }

        public void Update(int ID, Department department, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string values = JsonSerializer.Serialize(department);
                base.Update(ApiConfiguration.DepartmentApiRoute + ID.ToString(), values, authHeaderValue);
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

        public async Task UpdateAsync(int ID, Department department, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string values = JsonSerializer.Serialize(department);
                await base.UpdateAsync(ApiConfiguration.DepartmentApiRoute + ID.ToString(), values, authHeaderValue);
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

        public IEnumerable<DepartmentAudit> GetAuditListById(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetAuditObjects(ApiConfiguration.DepartmentApiAuditListRoute + ID.ToString(), authHeaderValue);
        }

        public async Task<IEnumerable<DepartmentAudit>> GetAuditListByIdAsync(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetAuditObjectsAsync(ApiConfiguration.DepartmentApiAuditListRoute + ID.ToString(), authHeaderValue);
        }

        public DepartmentAudit GetAuditById(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetAudit(ApiConfiguration.DepartmentApiAuditRoute + ID.ToString(), authHeaderValue);
        }

        public async Task<DepartmentAudit> GetAuditByIdAsync(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetAuditAsync(ApiConfiguration.DepartmentApiAuditRoute + ID.ToString(), authHeaderValue);
        }

        public void Restore(int id, int auditId, AuthenticationHeaderValue authHeaderValue = null)
        {
            Restore(ApiConfiguration.DepartmentApiAuditListRoute + id.ToString() + "/" + auditId.ToString(), authHeaderValue);
        }

        public async Task RestoreAsync(int id, int auditId, AuthenticationHeaderValue authHeaderValue = null)
        {
            await RestoreAsync(ApiConfiguration.DepartmentApiAuditListRoute + id.ToString() + "/" + auditId.ToString(), authHeaderValue);
        }

        public IEnumerable<Department> GetAllSnapshot(DateTime datetime, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string dateFormatted = datetime.ToString(_dateFormat);
                return GetObjects(ApiConfiguration.DepartmentApiSnapshotRoute + dateFormatted, authHeaderValue);
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

        public async Task<IEnumerable<Department>> GetAllSnapshotAsync(DateTime datetime, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string dateFormatted = datetime.ToString(_dateFormat);
                return await GetObjectsAsync(ApiConfiguration.DepartmentApiSnapshotRoute + dateFormatted, authHeaderValue);
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

        public Department GetIdSnapshot(DateTime datetime, int id, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string dateFormatted = datetime.ToString(_dateFormat);
                return GetAsObject(ApiConfiguration.DepartmentApiSnapshotRoute + dateFormatted + "/" + id.ToString(), authHeaderValue);
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

        public async Task<Department> GetIdSnapshotAsync(DateTime datetime, int id, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string dateFormatted = datetime.ToString(_dateFormat);
                return await GetAsObjectAsync(ApiConfiguration.DepartmentApiSnapshotRoute + dateFormatted + "/" + id.ToString(), authHeaderValue);
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