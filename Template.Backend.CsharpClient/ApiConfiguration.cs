using System.Configuration;
using System.Reflection;
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
        private const string configFileName = "Template.Backend.CsharpClient.dll.config";

        /// <summary>
        /// Gets the Api Routes configuration.
        /// </summary>
        /// <value>
        /// The Api Routes configuration.
        /// </value>
        /// <exception cref="ConfigFileNotFoundException">Configuration file not found in Environment Directory, please contact Administrator</exception>
        private static Configuration ClientConfig
        {
            get
            {
                ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                map.ExeConfigFilename = Path.GetDirectoryName(path) + "\\" + configFileName;
                if (!File.Exists(map.ExeConfigFilename))
                {
                    throw new ConfigFileNotFoundException("Configuration file not found in Environment Directory ("+ configFileName+"), please contact Administrator");
                }
                Configuration libConfig = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                return libConfig;
            }
        }

        /// <summary>
        /// Gets the route value of the specified key.
        /// </summary>
        /// <param name="key">The route key.</param>
        /// <returns>Route value</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException"></exception>
        private static string GetSetting(string key)
        {
            if (ClientConfig.AppSettings.Settings[key] == null)
                throw new ApiKeyNotFoundException(key);

            return ClientConfig.AppSettings.Settings[key].Value;
        }

        /// <summary>
        /// Gets integer setting value of the specified key.
        /// </summary>
        /// <param name="key">The route key.</param>
        /// <returns>Route value</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException"></exception>
        private static int GetSettingInteger(string key)
        {
            if (ClientConfig.AppSettings.Settings[key] == null)
                throw new ApiKeyNotFoundException(key);
            int value;
            if (int.TryParse(ClientConfig.AppSettings.Settings[key].Value, out value))
                return value;
            throw new BusinessException("wrong value for the key " + key);
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