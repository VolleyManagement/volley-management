using TechTalk.SpecFlow;

namespace VolleyManagement.Specs.PlayersContext
{
    [Binding]
    public class RemovePlayerSteps
    {
        [When(@"I execute DeletePlayer")]
        public void WhenIExecuteDeletePlayer()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"player is removed")]
        public void ThenPlayerIsRemoved()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"ConcurrencyException is thrown")]
        public void ThenConcurrencyExceptionIsThrown()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
