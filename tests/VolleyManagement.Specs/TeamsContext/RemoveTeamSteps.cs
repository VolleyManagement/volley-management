namespace VolleyManagement.Specs.TeamsContext
{
    using FluentAssertions;
    using System;
    using System.Data.Entity.Infrastructure;
    using TechTalk.SpecFlow;
    using VolleyManagement.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Specs.Infrastructure;
    using VolleyManagement.Specs.Infrastructure.IOC;

    [Binding]
    public class RemoveTeamSteps
    {
        private TeamId _teamId;
        private Exception _exception;
        private readonly ITeamService _teamService;
        
        private const int ID_TEAM_DOES_NOT_EXIST = 1;

        private readonly string teamShouldBeDeletedFromDB =
           "Team should been deleted from the database";

        private readonly string exeptionShouldBeThrown =
           "ConcurrencyException should been thown";

        private readonly string exeptionShouldNotBeThrown =
           "Exception should not been thown";

        public RemoveTeamSteps()
        {
            TestDbAdapter.Respawn();
            _teamService = IocProvider.Get<ITeamService>();
            
        }

        [Given(@"(.*) team exists")]
        [Scope(Feature = "Remove Team")]
        public void GivenTeamExists(string name)
        {
            var _player = new PlayerEntity {
                FirstName = "First",
                LastName = "Last"
            };

            var _team = new TeamEntity {
                Name = name,
                Coach = "coach name",
                Achievements = null,
                Captain = _player
            };
            TestDbAdapter.CreateTeam(_team);
            TestDbAdapter.AssignPlayerToTeam(_player.Id, _team.Id);
            _teamId = new TeamId(_team.Id);
        }
        
        [Given(@"(.*) team does not exist")]
        [Scope(Feature = "Remove Team")]
        public void GivenTeamDoesNotExist(string name)
        {
            _teamId = new TeamId(ID_TEAM_DOES_NOT_EXIST);
        }
        
        [When(@"I execute DeleteTeam")]
        public void WhenIExecuteDeleteTeam()
        {
            try
            {
                _teamService.Delete(_teamId);
            }
            catch(Exception exc)
            {
                _exception = exc;
            }
            
        }
        
        [Then(@"team is removed")]
        [Scope(Feature = "Remove Team")]
        public void ThenTeamIsRemoved()
        {
            TeamEntity teamEntity;
            using (var context = TestDbAdapter.Context)
            {
                teamEntity = context.Teams.Find(_teamId.Id);
            }

            teamEntity.Should().Be(null, teamShouldBeDeletedFromDB);
            _exception.Should().Be(null, exeptionShouldNotBeThrown);
        }

        [Then(@"ConcurrencyException is thrown")]
        [Scope(Feature = "Remove Team")]
        public void ThenConcurrencyExceptionIsThrown()
        {
            _exception.Should().NotBe(null, exeptionShouldBeThrown);
            _exception.Should().BeOfType(typeof(DbUpdateConcurrencyException), exeptionShouldBeThrown);
       }
    }
}
