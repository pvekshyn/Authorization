namespace Role.Integration.Tests
{
    public static class Constants
    {
        public static string ConnectionString = "Data Source=localhost\\SQLEXPRESS;User Id=role_service;Password=role_password;Initial Catalog=Test_Role;TrustServerCertificate=True";

        //from postdeploy, need to setup/teardown test data
        public static readonly Guid AssignPermissionId = new Guid("D4F358B3-E9CD-466B-AED9-F31A77E6A8D3");
        public static readonly Guid DeassignPermissionId = new Guid("BC6B2FAC-6A85-4956-81C3-4A2429FE2298");

    }
}
