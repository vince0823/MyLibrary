namespace MyLibrary.WebApi.PolicyHandlers
{
    public static class ApplicationClaimPolicy
    {
        public const string SuperAdminOnly = "SuperAdminOnly";
        public const string AdminOnly = "AdminOnly";
        public const string ReportOnly = "ReportOnly";
        public const string NormalOnly = "NormalOnly";
        public const string IsSpecificAdminOnly = "IsSpecificAdminOnly";
    }
}
