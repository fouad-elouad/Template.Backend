using Microsoft.Extensions.Configuration;
using Template.Backend.Model.Exceptions;

namespace Template.Backend.CsharpClient
{
    /// <summary>
    ///  Provides access to Api Route configuration file for client applications.
    /// </summary>
    internal static class ApiConfiguration
    {
        /// <summary>
        /// The configuration file name
        /// </summary>
        private const string configFileName = "Template.Backend.CsharpClient.dll.json";

        private static readonly IConfiguration _configuration;

        static ApiConfiguration()
        {
            var configFilePath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + configFileName;

            if (!File.Exists(configFilePath))
            {
                throw new ConfigFileNotFoundException("Configuration file not found in Environment Directory (" + configFileName + "), please contact Administrator");
            }

            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile(configFilePath, optional: false, reloadOnChange: true);

            _configuration = configBuilder.Build();
        }

        /// <summary>
        /// Gets the route value of the specified key.
        /// </summary>
        /// <param name="key">The route key.</param>
        /// <returns>Route value</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException"></exception>
        private static string GetSetting(string key)
        {
            return _configuration[key] != null ? _configuration[key]! : throw new ApiKeyNotFoundException(key);
        }

        /// <summary>
        /// Gets integer setting value of the specified key.
        /// </summary>
        /// <param name="key">The route key.</param>
        /// <returns>Route value</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException"></exception>
        private static int GetSettingInteger(string key)
        {
            var s_Value = GetSetting(key);
            return int.TryParse(s_Value, out int value) ? value : throw new BusinessException("wrong value for the key " + key);
        }

        /// <summary>
        /// Gets the Api base address.
        /// </summary>
        /// <value>
        /// The base address.
        /// </value>
        public static string BaseAddress { get { return GetSetting("BaseAddress"); } }

        /// <summary>
        /// request timeout
        /// </summary>
        /// <value>
        /// The request timeout.
        /// </value>
        public static int RequestTimeout { get { return GetSettingInteger("RequestTimeout"); } }

        /// <summary>
        /// request timeout
        /// </summary>
        /// <value>
        /// The request timeout.
        /// </value>
        public static int PooledConnectionLifetime { get { return GetSettingInteger("PooledConnectionLifetime"); } }

        // Company
        public static string CompanyApiRoute { get { return GetSetting("CompanyApiRoute"); } }
        public static string CompanyApiCountRoute { get { return GetSetting("CompanyApiCountRoute"); } }
        public static string CompanyApiPaginationRoute { get { return GetSetting("CompanyApiPaginationRoute"); } }
        public static string CompanyApiItemsRoute { get { return GetSetting("CompanyApiItemsRoute"); } }
        public static string CompanyApiSearchRoute { get { return GetSetting("CompanyApiSearchRoute"); } }
        public static string CompanyApiAuditListRoute { get { return GetSetting("CompanyApiAuditListRoute"); } }
        public static string CompanyApiAuditRoute { get { return GetSetting("CompanyApiAuditRoute"); } }
        public static string CompanyApiSnapshotRoute { get { return GetSetting("CompanyApiSnapshotRoute"); } }

        // Employee
        public static string EmployeeApiRoute { get { return GetSetting("EmployeeApiRoute"); } }
        public static string EmployeeApiCountRoute { get { return GetSetting("EmployeeApiCountRoute"); } }
        public static string EmployeeApiPaginationRoute { get { return GetSetting("EmployeeApiPaginationRoute"); } }
        public static string EmployeeApiItemsRoute { get { return GetSetting("EmployeeApiItemsRoute"); } }
        public static string EmployeeApiSearchRoute { get { return GetSetting("EmployeeApiSearchRoute"); } }
        public static string EmployeeApiAuditListRoute { get { return GetSetting("EmployeeApiAuditListRoute"); } }
        public static string EmployeeApiAuditRoute { get { return GetSetting("EmployeeApiAuditRoute"); } }
        public static string EmployeeApiSnapshotRoute { get { return GetSetting("EmployeeApiSnapshotRoute"); } }

        // Department
        public static string DepartmentApiRoute { get { return GetSetting("DepartmentApiRoute"); } }
        public static string DepartmentApiCountRoute { get { return GetSetting("DepartmentApiCountRoute"); } }
        public static string DepartmentApiPaginationRoute { get { return GetSetting("DepartmentApiPaginationRoute"); } }
        public static string DepartmentApiItemsRoute { get { return GetSetting("DepartmentApiItemsRoute"); } }
        public static string DepartmentApiSearchRoute { get { return GetSetting("DepartmentApiSearchRoute"); } }
        public static string DepartmentApiAuditListRoute { get { return GetSetting("DepartmentApiAuditListRoute"); } }
        public static string DepartmentApiAuditRoute { get { return GetSetting("DepartmentApiAuditRoute"); } }
        public static string DepartmentApiSnapshotRoute { get { return GetSetting("DepartmentApiSnapshotRoute"); } }

    }
}