using System;
using TechTalk.SpecFlow;

namespace VolleyManagement.Specs
{
    [Binding]
    public class EditTeamSteps
    {
        [Given(@"Team A team exists")]
        public void GivenTeamATeamExists()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"name changed to A-Team")]
        public void GivenNameChangedToA_Team()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"name changed to Very looooooooooooooooooooooooong team name which should be more than (.*) symbols")]
        public void GivenNameChangedToVeryLooooooooooooooooooooooooongTeamNameWhichShouldBeMoreThanSymbols(int p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"Team B team does not exist")]
        public void GivenTeamBTeamDoesNotExist()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"name changed to B-Team")]
        public void GivenNameChangedToB_Team()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"captain is changed to Captain B")]
        public void GivenCaptainIsChangedToCaptainB()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I execute EditTeam")]
        public void WhenIExecuteEditTeam()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I execute ChangeTeamCaptain")]
        public void WhenIExecuteChangeTeamCaptain()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"team is updated succesfully")]
        public void ThenTeamIsUpdatedSuccesfully()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
