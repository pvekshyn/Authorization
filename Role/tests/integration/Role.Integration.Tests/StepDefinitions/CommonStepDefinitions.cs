using Common.SDK;
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
    }
}
