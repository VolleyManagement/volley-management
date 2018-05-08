using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Domain.TeamsAggregate;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;

namespace VolleyManagement.Specs.TeamsContext
{
    [Binding]
    [Scope(Feature = "Manage Team Players")]
    public class ManageTeamPlayersSteps
    {
        private readonly ITeamService _teamService;
        private TeamId _teamId;
        private PlayerId _captainId;
        private readonly List<PlayerId> _playerToAdd;
        private readonly ICollection<PlayerId> _playersToRemove;
        private Exception _exception;

        public ManageTeamPlayersSteps()
        {
            _teamService = IocProvider.Get<ITeamService>();
            _playerToAdd = new List<PlayerId>();
            _playersToRemove = new List<PlayerId>();
        }

        [Given(@"Team (.*) exists")]
        public void GivenTeamExists(string name)
        {
            var team = TestDbAdapter.CreateTeamWithCaptain(name, "First", "Last");
            _teamId = new TeamId(team.Id);
            _captainId = new PlayerId(team.CaptainId);
        }

        [Given(@"I have added (.*) as a team player")]
        public void GivenIHavePlayerAsATeamPlayer(string playerName)
        {
            var names = SpecsHelper.SplitFullNameToFirstLastNames(playerName);

            var newPlayer = new PlayerEntity {
                FirstName = names.FirstName,
                LastName = names.LastName
            };

            TestDbAdapter.CreatePlayer(newPlayer);
            _playerToAdd.Add(new PlayerId(newPlayer.Id));
        }

        [Given(@"(.*) is a team player")]
        public void GivenPlayerIsATeamPlayer(string playerName)
        {
            var names = SpecsHelper.SplitFullNameToFirstLastNames(playerName);

            var newPlayer = new PlayerEntity {
                FirstName = names.FirstName,
                LastName = names.LastName
            };

            TestDbAdapter.CreatePlayer(newPlayer);
            TestDbAdapter.AssignPlayerToTeam(newPlayer.Id, _teamId.Id);
        }

        [Given(@"I have removed (.*)")]
        public void GivenIHaveRemovedPlayer(string playerName)
        {
            var names = SpecsHelper.SplitFullNameToFirstLastNames(playerName);

            using (var ctx = TestDbAdapter.Context)
            {
                var playerEntities = ctx.Players
                    .SingleOrDefault(p => p.FirstName == names.FirstName && p.LastName == names.LastName);
                _playersToRemove.Add(new PlayerId(playerEntities.Id));
            }
        }

        [Given(@"(.*) is a team captain")]
        public void GivenJaneDoeIsATeamCaptain(string playerName)
        {
            var names = SpecsHelper.SplitFullNameToFirstLastNames(playerName);

            using (var ctx = TestDbAdapter.Context)
            {
                var playerEntity = ctx.Players
                    .SingleOrDefault(p => p.Id == _captainId.Id);

                if (playerEntity == null)
                {
                    return;
                }

                playerEntity.FirstName = names.FirstName;
                playerEntity.LastName = names.LastName;
                ctx.SaveChanges();
            }
        }

        [When(@"I execute AddPlayersToTeam")]
        public void WhenIExecuteAddPlayersToTeam()
        {
            _teamService.AddPlayers(_teamId, _playerToAdd);
        }

        [When(@"I execute RemovePlayersFromTeam")]
        public void WhenIExecuteRemovePlayersFromTeam()
        {
            try
            {
                _teamService.RemovePlayers(_teamId, _playersToRemove);
            }
            catch (EntityInvariantViolationException exc)
            {
                _exception = exc;
            }
        }

        [Then(@"players are added")]
        public void ThenPlayersAreAdded()
        {
            using (var ctx = TestDbAdapter.Context)
            {
                IEnumerable<PlayerEntity> playerEntities = ctx.Teams.First(team => team.Id == _teamId.Id).Players;
                var entityRosterIds = playerEntities.Select(e => e.Id);
                var playersId = _playerToAdd.Select(e => e.Id);
                entityRosterIds.Should().Contain(playersId, "Players should be added");
            }
        }

        [Then(@"players are removed")]
        public void ThenPlayersAreRemoved()
        {
            using (var ctx = TestDbAdapter.Context)
            {
                IEnumerable<PlayerEntity> playerEntities = ctx.Teams.First(team => team.Id == _teamId.Id).Players;
                var entityRosterIds = playerEntities.Select(e => e.Id);
                if (_playersToRemove != null)
                {
                    entityRosterIds.Should().NotContain(_playersToRemove, "Players should be removed");
                }
            }
        }

        [Then(@"EntityInvariantViolationException is thrown")]
        public void ThenInvalidOperationExceptionIsThrown()
        {
            _exception.Should().BeOfType(typeof(EntityInvariantViolationException), "Should thrown EntityInvariantViolationException");
        }
    }
}
