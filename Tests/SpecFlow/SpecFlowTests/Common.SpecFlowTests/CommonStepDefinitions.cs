using Common.SDK;
using TechTalk.SpecFlow;
using Xunit;

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
            Assert.Equal(200, result.Status);
        }

        [Then(@"forbidden result")]
        public void Forbidden()
        {
            var result = (Result)_scenarioContext["result"];
            Assert.Equal(403, result.Status);
        }

        [Then(@"validation error")]
        public void ThenValidationError()
        {
            var result = (Result)_scenarioContext["result"];
            Assert.Equal(422, result.Status);

        }

        [Then(@"idempotent result")]
        public void Idempotent()
        {
            var result = (Result)_scenarioContext["result"];
            Assert.Equal(204, result.Status);
        }
    }
}
