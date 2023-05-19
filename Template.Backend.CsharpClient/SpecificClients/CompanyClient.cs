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
    /// CompanyClient interface
    /// </summary>
    /// <seealso cref="Template.Backend.CsharpClient.IClient{Company, CompanyAudit}" />
    public interface ICompanyClient : IClient<Company, CompanyAudit>
    {
        /// <summary>
        /// Adds the specified Company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        Company Add(Company company, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Adds the specified Company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        Task<Company> AddAsync(Company company, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Adds the specified Company List.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// list
        /// </returns>
        IEnumerable<Company> Add(IEnumerable<Company> list, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Adds the specified Company List.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// list
        /// </returns>
        Task<IEnumerable<Company>> AddAsync(IEnumerable<Company> list, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets all Company.
        /// </summary>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Company
        /// </returns>
        IEnumerable<Company> GetAll(AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets all Company.
        /// </summary>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Company
        /// </returns>
        Task<IEnumerable<Company>> GetAllAsync(AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets Company with the specified Id.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Company object
        /// </returns>
        Company Get(int id, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets Company with the specified Id.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Company object
        /// </returns>
        Task<Company> GetAsync(int id, AuthenticationHeaderValue authHeaderValue = null);

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
        /// Gets Company with the specified Id.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <param name="nestedObjectDepth">The maximum level to achieve for navigation properties serialization.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>Company object</returns>
        Company Get(int id, NestedObjectDepth nestedObjectDepth, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets Company with the specified Id.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <param name="nestedObjectDepth">The maximum level to achieve for navigation properties serialization.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>Company object</returns>
        Task<Company> GetAsync(int id, NestedObjectDepth nestedObjectDepth, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Deletes Company with the specified Id.
        /// </summary>
        /// <param name="ID">The Company Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        void Delete(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Deletes Company with the specified Id.
        /// </summary>
        /// <param name="ID">The Company Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        Task DeleteAsync(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Updates Company with specified Id.
        /// </summary>
        /// <param name="ID">The Company Id.</param>
        /// <param name="company">The company.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        void Update(int ID, Company company, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Updates Company with specified Id.
        /// </summary>
        /// <param name="ID">The Company Id.</param>
        /// <param name="company">The company.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        Task UpdateAsync(int ID, Company company, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the audit list of Company.
        /// </summary>
        /// <param name="ID">The Company Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        IEnumerable<CompanyAudit> GetAuditListById(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the audit list of Company.
        /// </summary>
        /// <param name="ID">The Company Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// response of type string
        /// </returns>
        Task<IEnumerable<CompanyAudit>> GetAuditListByIdAsync(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the audit row by Id.
        /// </summary>
        /// <param name="ID">The CompanyAudit Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// CompanyAudit object
        /// </returns>
        CompanyAudit GetAuditById(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the audit row by Id.
        /// </summary>
        /// <param name="ID">The CompanyAudit Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// CompanyAudit object
        /// </returns>
        Task<CompanyAudit> GetAuditByIdAsync(int ID, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Restores the Company by the spicified CompanyAudit Id.
        /// </summary>
        /// <param name="id">The Company Id.</param>
        /// <param name="auditId">The CompanyAudit Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        void Restore(int id, int auditId, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Restores the Company by the spicified CompanyAudit Id.
        /// </summary>
        /// <param name="id">The Company Id.</param>
        /// <param name="auditId">The CompanyAudit Id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        Task RestoreAsync(int id, int auditId, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the paged list of Company.
        /// </summary>
        /// <param name="pageNo">The page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Company object
        /// </returns>
        IEnumerable<Company> GetPagedList(int pageNo, int pageSize, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the paged list of Company.
        /// </summary>
        /// <param name="pageNo">The page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List of Company object
        /// </returns>
        Task<IEnumerable<Company>> GetPagedListAsync(int pageNo, int pageSize, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the snapshot of all entities at the specified date time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List snapshot
        /// </returns>
        IEnumerable<Company> GetAllSnapshot(DateTime datetime, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the snapshot of all entities at the specified date time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// List snapshot
        /// </returns>
        Task<IEnumerable<Company>> GetAllSnapshotAsync(DateTime datetime, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the snapshot at the specified date time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="id">The entity id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Company snapshot
        /// </returns>
        Company GetIdSnapshot(DateTime datetime, int id, AuthenticationHeaderValue authHeaderValue = null);

        /// <summary>
        /// Gets the snapshot at the specified date time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="id">The entity id.</param>
        /// <param name="authHeaderValue">The authentication header value.</param>
        /// <returns>
        /// Company snapshot
        /// </returns>
        Task<Company> GetIdSnapshotAsync(DateTime datetime, int id, AuthenticationHeaderValue authHeaderValue = null);
    }

    /// <summary>
    /// CompanyClient Class
    /// </summary>
    /// <seealso cref="Template.Backend.CsharpClient.Client{Company, CompanyAudit}" />
    /// <seealso cref="Template.Backend.CsharpClient.SpecificClients.ICompanyClient" />
    public class CompanyClient : Client<Company, CompanyAudit>, ICompanyClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyClient"/> class.
        /// </summary>
        public CompanyClient()
        {
        }

        public Company Add(Company company, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string values = JsonSerializer.Serialize(company);
                return base.Add(ApiConfiguration.CompanyApiRoute, values, authHeaderValue);
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

        public async Task<Company> AddAsync(Company company, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string values = JsonSerializer.Serialize(company);
                return await AddAsync(ApiConfiguration.CompanyApiRoute, values, authHeaderValue);
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

        public IEnumerable<Company> Add(IEnumerable<Company> list, AuthenticationHeaderValue authHeaderValue = null)
        {
            string values = JsonSerializer.Serialize(list);
            return AddRange(ApiConfiguration.CompanyApiItemsRoute, values, authHeaderValue);
        }

        public async Task<IEnumerable<Company>> AddAsync(IEnumerable<Company> list, AuthenticationHeaderValue authHeaderValue = null)
        {
            string values = JsonSerializer.Serialize(list);
            return await AddRangeAsync(ApiConfiguration.CompanyApiItemsRoute, values, authHeaderValue);
        }

        public int Count(AuthenticationHeaderValue authHeaderValue = null)
        {
            return Count(ApiConfiguration.CompanyApiCountRoute, authHeaderValue);
        }

        public async Task<int> CountAsync(AuthenticationHeaderValue authHeaderValue = null)
        {
            return await CountAsync(ApiConfiguration.CompanyApiCountRoute, authHeaderValue);
        }

        public IEnumerable<Company> GetAll(AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetObjects(ApiConfiguration.CompanyApiRoute, authHeaderValue);
        }

        public async Task<IEnumerable<Company>> GetAllAsync(AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetObjectsAsync(ApiConfiguration.CompanyApiRoute, authHeaderValue);
        }

        public void Delete(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            Delete(ApiConfiguration.CompanyApiRoute + ID.ToString(), authHeaderValue);
        }

        public async Task DeleteAsync(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            await DeleteAsync(ApiConfiguration.CompanyApiRoute + ID.ToString(), authHeaderValue);
        }

        public Company Get(int id, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetAsObject(ApiConfiguration.CompanyApiRoute + id.ToString(), authHeaderValue);
        }

        public async Task<Company> GetAsync(int id, AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetAsObjectAsync(ApiConfiguration.CompanyApiRoute + id.ToString(), authHeaderValue);
        }

        public Company Get(int id, NestedObjectDepth nestedObjectDepth, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetAsObject(ApiConfiguration.CompanyApiRoute + id.ToString() + "/" + nestedObjectDepth.ToString(), authHeaderValue);
        }

        public async Task<Company> GetAsync(int id, NestedObjectDepth nestedObjectDepth, AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetAsObjectAsync(ApiConfiguration.CompanyApiRoute + id.ToString() + "/" + nestedObjectDepth.ToString(), authHeaderValue);
        }

        public IEnumerable<Company> GetPagedList(int pageNo, int pageSize, AuthenticationHeaderValue authHeaderValue = null)
        {
            string url = string.Empty.AddQuery(nameof(pageNo), pageNo.ToString())
                                     .AddQuery(nameof(pageSize), pageSize.ToString());

            return GetObjects(ApiConfiguration.CompanyApiPaginationRoute + "?" + url, authHeaderValue);
        }

        public async Task<IEnumerable<Company>> GetPagedListAsync(int pageNo, int pageSize, AuthenticationHeaderValue authHeaderValue = null)
        {
            string url = string.Empty.AddQuery(nameof(pageNo), pageNo.ToString())
                                     .AddQuery(nameof(pageSize), pageSize.ToString());

            return await GetObjectsAsync(ApiConfiguration.CompanyApiPaginationRoute + "?" + url, authHeaderValue);
        }

        public void Update(int ID, Company company, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string values = JsonSerializer.Serialize(company);
                base.Update(ApiConfiguration.CompanyApiRoute + ID.ToString(), values, authHeaderValue);
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

        public async Task UpdateAsync(int ID, Company company, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string values = JsonSerializer.Serialize(company);
                await base.UpdateAsync(ApiConfiguration.CompanyApiRoute + ID.ToString(), values, authHeaderValue);
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

        public IEnumerable<CompanyAudit> GetAuditListById(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetAuditObjects(ApiConfiguration.CompanyApiAuditListRoute + ID.ToString(), authHeaderValue);
        }

        public async Task<IEnumerable<CompanyAudit>> GetAuditListByIdAsync(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetAuditObjectsAsync(ApiConfiguration.CompanyApiAuditListRoute + ID.ToString(), authHeaderValue);
        }

        public CompanyAudit GetAuditById(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            return GetAudit(ApiConfiguration.CompanyApiAuditRoute + ID.ToString(), authHeaderValue);
        }

        public async Task<CompanyAudit> GetAuditByIdAsync(int ID, AuthenticationHeaderValue authHeaderValue = null)
        {
            return await GetAuditAsync(ApiConfiguration.CompanyApiAuditRoute + ID.ToString(), authHeaderValue);
        }

        public void Restore(int id, int auditId, AuthenticationHeaderValue authHeaderValue = null)
        {
            Restore(ApiConfiguration.CompanyApiAuditListRoute + id.ToString() + "/" + auditId.ToString(), authHeaderValue);
        }

        public async Task RestoreAsync(int id, int auditId, AuthenticationHeaderValue authHeaderValue = null)
        {
            await RestoreAsync(ApiConfiguration.CompanyApiAuditListRoute + id.ToString() + "/" + auditId.ToString(), authHeaderValue);
        }

        public IEnumerable<Company> GetAllSnapshot(DateTime datetime, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string dateFormatted = datetime.ToString(_dateFormat);
                return GetObjects(ApiConfiguration.CompanyApiSnapshotRoute + dateFormatted, authHeaderValue);
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

        public async Task<IEnumerable<Company>> GetAllSnapshotAsync(DateTime datetime, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string dateFormatted = datetime.ToString(_dateFormat);
                return await GetObjectsAsync(ApiConfiguration.CompanyApiSnapshotRoute + dateFormatted, authHeaderValue);
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

        public Company GetIdSnapshot(DateTime datetime, int id, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string dateFormatted = datetime.ToString(_dateFormat);
                return GetAsObject(ApiConfiguration.CompanyApiSnapshotRoute + dateFormatted + "/" + id.ToString(), authHeaderValue);
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

        public async Task<Company> GetIdSnapshotAsync(DateTime datetime, int id, AuthenticationHeaderValue authHeaderValue = null)
        {
            try
            {
                string dateFormatted = datetime.ToString(_dateFormat);
                return await GetAsObjectAsync(ApiConfiguration.CompanyApiSnapshotRoute + dateFormatted + "/" + id.ToString(), authHeaderValue);
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