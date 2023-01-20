using Common.SDK;

namespace SpecFlowTests.StepDefinitions
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
    }
}
