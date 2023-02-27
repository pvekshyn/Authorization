namespace Role.Host.API
{
    public class StartupSettings
    {
        public string ConnectionString { get; init; }
        public string IdentityServerUrl { get; init; }
        public string AuthorizationUrl { get; init; }
        public string KeyVaultName { get; init; }
        public string ManagedIdentityClientId { get; init; }

        private IWebHostEnvironment _env;

        public StartupSettings(ConfigurationManager configuration, IWebHostEnvironment env)
        {
            _env = env;
            ConnectionString = configuration.GetConnectionString("Database");
            IdentityServerUrl = configuration.GetServiceUri("identity-server")?.ToString();
            AuthorizationUrl = configuration.GetServiceUri("authorization-api", "grpc")?.ToString();
            KeyVaultName = configuration.GetSection("KeyVaultName")?.Value;
            ManagedIdentityClientId = configuration.GetSection("ManagedIdentityClientId")?.Value;
        }

        public bool NeedAuth()
        {
            return IdentityServerUrl is not null && AuthorizationUrl is not null;
        }

        public bool NeedKeyVault()
        {
            return KeyVaultName is not null && ManagedIdentityClientId is not null;
        }


        public void Log(ILogger logger)
        {
            logger.LogInformation($"Environment = {_env.EnvironmentName}");
            logger.LogInformation($"{nameof(ConnectionString)} = {ConnectionString}");
            logger.LogInformation($"{nameof(IdentityServerUrl)} = {IdentityServerUrl}");
            logger.LogInformation($"{nameof(AuthorizationUrl)} = {AuthorizationUrl}");
            logger.LogInformation($"{nameof(KeyVaultName)} = {KeyVaultName}");
            logger.LogInformation($"{nameof(ManagedIdentityClientId)} = {ManagedIdentityClientId}");


            if (!_env.IsDevelopment())
            {
                if (!NeedAuth())
                    throw new Exception("No Auth registered");
            }
        }
    }
}
