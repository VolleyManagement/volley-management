using TechTalk.SpecFlow;

namespace VolleyManagement.Specs.TeamsContext
{
    [Binding]
    public class RemoveTeamSteps
    {
        [Given(@"Volley\.org\.ua team exists")]
        public void GivenVolley_Org_UaTeamExists()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"football\.org\.ua team does not exist")]
        public void GivenFootball_Org_UaTeamDoesNotExist()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I execute DeleteTeam")]
        public void WhenIExecuteDeleteTeam()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"team is removed")]
        public void ThenTeamIsRemoved()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
