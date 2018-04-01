using System;
using TechTalk.SpecFlow;

namespace VolleyManagement.Specs.TeamsContext
{
    [Binding]
    public class CreateTeamsSteps
    {
        [Given(@"team name is Team A")]
        public void GivenTeamNameIsTeamA()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"captain is Jane Doe")]
        public void GivenCaptainIsJaneDoe()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"coach is ")]
        public void GivenCoachIs()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"achievements are ")]
        public void GivenAchievementsAre()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I execute CreateTeam")]
        public void WhenIExecuteCreateTeam()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"new team gets new Id")]
        public void ThenNewTeamGetsNewId()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"new team should be succesfully added")]
        public void ThenNewTeamShouldBeSuccesfullyAdded()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
