using System;
using System.Linq;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Domain.TeamsAggregate;
using VolleyManagement.Specs.Infrastructure;

namespace VolleyManagement.Specs.TeamsContext
{
    [Binding]
    public class ManageTeamPlayersSteps
    {
        private Player _player;
        private Team _team;
        private readonly ITeamService _teamService;

        [Given(@"Team (.*) exists")]
        public void GivenTeamAExists(string teamName)
        {
            //_teamService.Get();
            _team = new Team(int.MaxValue, teamName, null, string.Empty, null, null);
            _teamService.Create(new CreateTeamDto {
                Name = _team.Name,
                Achievements = _team.Achievements,
                Roster = _team.Roster,
                Captain = _team.Captain,
                Coach = _team.Coach
            });
        }

        [Given(@"I have added (.*) as a team player")]
        public void GivenIHaveAddedJaneDoeAsATeamPlayer(string playerName)
        {
            
            using (var ctx = TestDbAdapter.Context)
            {
              var res=  ctx.Players.First(x =>
                    x.FirstName == playerName.Split(' ')[0] && 
                    x.LastName == playerName.Split(' ')[1]);
            }
            ScenarioContext.Current.Pending();
        }

        [Given(@"Ivan Ivanov is a team player")]
        public void GivenIvanIvanovIsATeamPlayer()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I have added John Smith as a team player")]
        public void GivenIHaveAddedJohnSmithAsATeamPlayer()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"Jane Doe is a team player")]
        public void GivenJaneDoeIsATeamPlayer()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I have removed Jane Doe")]
        public void GivenIHaveRemovedJaneDoe()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"(.*) is a team player")]
        public void GivenJohnSmithIsATeamPlayer()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"Jane Doe is a team captain")]
        public void GivenJaneDoeIsATeamCaptain()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I execute AddPlayersToTeam")]
        public void WhenIExecuteAddPlayersToTeam()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I execute RemovePlayersFromTeam")]
        public void WhenIExecuteRemovePlayersFromTeam()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"players are added")]
        public void ThenPlayersAreAdded()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"players are removed")]
        public void ThenPlayersAreRemoved()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"InvalidOperationException is thrown")]
        public void ThenInvalidOperationExceptionIsThrown()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
