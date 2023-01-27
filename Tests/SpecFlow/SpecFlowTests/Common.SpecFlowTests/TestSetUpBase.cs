using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Dac;

namespace Common.SpecFlowTests
{
    public class TestSetUpBase
    {
        protected virtual string DbName { get; set; }
        protected virtual IEnumerable<string> DacPacPaths { get; set; }

        private TestcontainerDatabase _container;
        private bool _useLocalTestDatabase;

        public async Task<string> StartDatabase()
        {
            var config = new ConfigurationBuilder()
                    .AddJsonFile("testconfiguration.json", optional: true)
                    .Build();

            _useLocalTestDatabase = Convert.ToBoolean(config["UseLocalTestDatabase"]);

            if (_useLocalTestDatabase)
            {
                return config["LocalTestDatabaseConnectionString"];
            }
            else
            {
                var connectionString = await StartDockerContainer(DbName);

                ApplyDacPacs(DacPacPaths, DbName, connectionString);

                return connectionString;
            }
        }

        public async Task StopDatabase()
        {
            if (!_useLocalTestDatabase)
            {
                await _container.StopAsync();
            }
        }

        private async Task<string> StartDockerContainer(string dbName)
        {
            _container = new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithDatabase(new MsSqlTestcontainerConfiguration
                {
                    Database = dbName,
                    Password = "password#123",
                })
                .Build();

            await _container.StartAsync();

            var sb = new SqlConnectionStringBuilder(_container.ConnectionString);
            sb.TrustServerCertificate = true;
            return sb.ToString();
        }

        private void ApplyDacPacs(IEnumerable<string> dacPacPaths, string dbName, string connectionString)
        {
            var dacpacService = new DacServices(connectionString);

            foreach (var dacPacPath in dacPacPaths)
            {
                var dacpac = DacPackage.Load(dacPacPath);
                dacpacService.Publish(dacpac, dbName, new PublishOptions());
            }
        }
    }
}
