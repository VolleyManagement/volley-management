using TechTalk.SpecFlow;

namespace VolleyManagement.Specs.TeamsContext
{
    [Binding]
    public class RemoveTeamSteps
    {
        [Given(@"(.*) team exists")]
        [Scope(Feature = "Remove Team")]
        public void GivenTeamExists()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"(.*) team does not exist")]
        [Scope(Feature = "Remove Team")]
        public void GivenTeamDoesNotExist()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I execute DeleteTeam")]
        public void WhenIExecuteDeleteTeam()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"team is removed")]
        [Scope(Feature = "Remove Team")]
        public void ThenTeamIsRemoved()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
