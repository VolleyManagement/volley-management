using System;
using TechTalk.SpecFlow;

namespace VolleyManagement.Specs.PlayersContext
{
    [Binding]
    public class EditPlayerSteps
    {
        [Given(@"John Smith player exists")]
        public void GivenJohnSmithPlayerExists()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"first name changed to Jack")]
        public void GivenFirstNameChangedToJack()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"first name changed to Very looooooooooooooooooooooooong name which should be more than (.*) symbols")]
        public void GivenFirstNameChangedToVeryLooooooooooooooooooooooooongNameWhichShouldBeMoreThanSymbols(int p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"Ivan Ivanov player does not exist")]
        public void GivenIvanIvanovPlayerDoesNotExist()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I execute EditPlayer")]
        public void WhenIExecuteEditPlayer()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"player is saved with new name")]
        public void ThenPlayerIsSavedWithNewName()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"EntityInvariantViolationException is thrown")]
        public void ThenEntityInvariantViolationExceptionIsThrown()
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
