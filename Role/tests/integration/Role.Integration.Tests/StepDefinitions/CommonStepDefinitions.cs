using Common.SDK;
using Dapper;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
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
            Assert.AreEqual(200, result.Status);
        }

        [Then(@"Idempotent result")]
        public void Idempotent()
        {
            var result = (Result)_scenarioContext["result"];
            Assert.AreEqual(204, result.Status);
        }

        [Then(@"Validation error")]
        public void ValidationError()
        {
            var result = (Result)_scenarioContext["result"];
            Assert.AreEqual(422, result.Status);
        }

        [Then(@"One success result")]
        public void OneSuccess()
        {
            var results = (Result[])_scenarioContext["results"];
            Assert.AreEqual(1, results.Where(x => x.Status == 200).Count());
        }

        [Then(@"One idempotent result")]
        public void OneIdempotent()
        {
            var results = (Result[])_scenarioContext["results"];
            Assert.AreEqual(1, results.Where(x => x.Status == 204).Count());
        }

        [Then(@"One validation error")]
        public void OneValidationError()
        {
            var results = (Result[])_scenarioContext["results"];
            Assert.AreEqual(1, results.Where(x => x.Status == 422).Count());
        }

        [Then(@"Outbox message in database")]
        public void ThenOutboxMessageInDatabase()
        {
            var entityId = (Guid)_scenarioContext["entityId"];

            var sql = @"SELECT TOP 1 1 
                FROM OutboxMessage
                WHERE EntityId = @entityId";

            using (var connection = new SqlConnection(TestSetUp.ConnectionString))
            {
                var outboxMessageExists = connection.QueryFirstOrDefault<bool>(sql, new { entityId });

                Assert.True(outboxMessageExists);
            }
        }
    }
}
