using Common.SDK;
using Dapper;
using Microsoft.Data.SqlClient;
using TechTalk.SpecFlow;

namespace Role.Integration.Tests.StepDefinitions
{
    [Binding]
    public class CommonStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        public CommonStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"Success result")]
        public void Success()
        {
            var result = (Result)_scenarioContext["result"];
            Assert.Equal(200, result.Status);
        }

        [Then(@"Idempotent result")]
        public void Idempotent()
        {
            var result = (Result)_scenarioContext["result"];
            Assert.Equal(204, result.Status);
        }

        [Then(@"Validation error")]
        public void ValidationError()
        {
            var result = (Result)_scenarioContext["result"];
            Assert.Equal(422, result.Status);
        }

        [Then(@"One success result")]
        public void OneSuccess()
        {
            var results = (Result[])_scenarioContext["results"];
            Assert.Single(results.Where(x => x.Status == 200));
        }

        [Then(@"One idempotent result")]
        public void OneIdempotent()
        {
            var results = (Result[])_scenarioContext["results"];
            Assert.Single(results.Where(x => x.Status == 204));
        }

        [Then(@"One validation error")]
        public void OneValidationError()
        {
            var results = (Result[])_scenarioContext["results"];
            Assert.Single(results.Where(x => x.Status == 422));
        }

        [Then(@"Outbox message in database")]
        public void ThenOutboxMessageInDatabase()
        {
            var entityId = (Guid)_scenarioContext["entityId"];

            var sql = @"SELECT TOP 1 1 
                FROM OutboxMessage
                WHERE EntityId = @entityId";

            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                var outboxMessageExists = connection.QueryFirstOrDefault<bool>(sql, new { entityId });

                Assert.True(outboxMessageExists);
            }
        }
    }
}
