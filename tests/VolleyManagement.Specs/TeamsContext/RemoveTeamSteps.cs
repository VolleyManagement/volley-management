using System.Collections.Generic;
using System.Data;
using VolleyManagement.Domain.PlayersAggregate;
using FluentAssertions;
using System;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Domain.TeamsAggregate;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;

namespace VolleyManagement.Specs.TeamsContext
{
    [Binding]
    [Scope(Feature = "Remove Team")]
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
            _teamService = IocProvider.Get<ITeamService>();
        }

        [Given(@"(.*) team exists")]
        public void GivenTeamExists(string name)
        {
            var player = new PlayerEntity {
                FirstName = "First",
                LastName = "Last"
            };
            TestDbAdapter.CreatePlayer(player);

            var team = new TeamEntity {
                Name = name,
                Coach = "coach name",
                Achievements = "Achivements",
                Captain = player,
                Players = new List<PlayerEntity>()
            };

            TestDbAdapter.CreateTeam(team);
            TestDbAdapter.AssignPlayerToTeam(player.Id, team.Id);
            _teamId = new TeamId(team.Id);
        }

        [Given(@"(.*) team does not exist")]
        public void GivenTeamDoesNotExist(string name)
        {
            _teamId = new TeamId(ID_TEAM_DOES_NOT_EXIST);
        }

        [When(@"I execute DeleteTeam")]
        public void WhenIExecuteDeleteTeam()
        {
            try
            {
                using (var context = TestDbAdapter.Context)
                {
                    _teamService.Delete(_teamId);
                }
            }
            catch (Exception exc)
            {
                _exception = exc;
            }

        }

        [Then(@"team is removed")]
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
        public void ThenConcurrencyExceptionIsThrown()
        {
            _exception.Should().NotBe(null, exeptionShouldBeThrown);
            _exception.Should().BeOfType(typeof(DBConcurrencyException), exeptionShouldBeThrown);
        }
    }
}
