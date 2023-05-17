namespace Template.Backend.Api.Configuration
{
    internal static class ApiRouteConfiguration
    {
        // Api controller prefix
        public const string CompanyPrefix = "api/v{version:apiVersion}/Companies";
        public const string EmployeePrefix = "api/v{version:apiVersion}/Employees";
        public const string DepartmentPrefix = "api/v{version:apiVersion}/Departments";


        // Routes suffix
        public const string IdSuffix = "{id:int}";
        public const string Pagination = "Pagination";
        public const string IdDepthSuffix = "{id:int}/{nestedObjectDepth:alpha}";
        public const string AuditIdSuffix = "AuditId/{id:int}";
        public const string SearchSuffix = "Search";
        public const string AuditSuffix = "Audit/{id:int}";
        public const string AuditRestoreSuffix = "Audit/{id:int}/{auditId:int}";
        public const string SnapshotSuffix = "Snapshot/{dateTime}";
        public const string SnapshotIdSuffix = "Snapshot/{dateTime}/{id:int}";

        public const string CountSuffix = "Count/";
        public const string ItemsSuffix = "Items";
    }
}