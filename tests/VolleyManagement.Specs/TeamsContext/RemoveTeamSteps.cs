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
    public class RemoveTeamSteps
    {
        private TeamId _teamId;
        private Exception _exception;
        private readonly ITeamService _teamService;
        private readonly IPlayerService _playerService;

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
            _playerService = IocProvider.Get<IPlayerService>();
        }

        [Given(@"(.*) team exists")]
        [Scope(Feature = "Remove Team")]
        public void GivenTeamExists(string name)
        {
            var _player = new PlayerEntity {
                FirstName = "First",
                LastName = "Last"
            };

            int playerId = _playerService.Create(AutoMapper.Mapper.Map<CreatePlayerDto>(_player)).Id;

            var _team = new TeamEntity {
                Name = name,
                Coach = "coach name",
                Achievements = "Achivements",
                Captain = _player
            };
            var roster = new List<PlayerId> { new PlayerId(playerId) };

            var createTeamDto = new CreateTeamDto {
                Name = _team.Name,
                Achievements = _team.Achievements,
                Captain = new PlayerId(playerId),
                Coach = _team.Coach,
                Roster = roster
            };

            _teamId = new TeamId(_teamService.Create(createTeamDto).Id);
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
            catch (Exception exc)
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
            _exception.Should().BeOfType(typeof(DBConcurrencyException), exeptionShouldBeThrown);
        }
    }
}
