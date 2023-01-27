using Common.SDK;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Common.SpecFlowTests
{
    [Binding]
    public class CommonStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        public CommonStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"success result")]
        public void Success()
        {
            var result = (Result)_scenarioContext["result"];
            Assert.AreEqual(200, result.Status);
        }

        [Then(@"forbidden result")]
        public void Forbidden()
        {
            var result = (Result)_scenarioContext["result"];
            Assert.AreEqual(403, result.Status);
        }

        [Then(@"validation error")]
        public void ThenValidationError()
        {
            var result = (Result)_scenarioContext["result"];
            Assert.AreEqual(422, result.Status);

        }

        [Then(@"idempotent result")]
        public void Idempotent()
        {
            var result = (Result)_scenarioContext["result"];
            Assert.AreEqual(204, result.Status);
        }
    }
}
