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
            var player = new PlayerEntity {
                FirstName = "CapitanFirst",
                LastName = "CapitanLast"
            };

            var team = new TeamEntity {
                Name = name,
                Coach = "coach name",
                Achievements = null,
                Captain = player
            };
            TestDbAdapter.CreateTeam(team);
            TestDbAdapter.AssignPlayerToTeam(player.Id, team.Id);
            _teamId = new TeamId(team.Id);
            _captainId = new PlayerId(player.Id);
        }

        [Given(@"I have added (.*) as a team player")]
        public void GivenIHavePlayerAsATeamPlayer(string playerName)
        {
            var whitespaceCharIndex = playerName.IndexOf(' ');
            var firstName = playerName.Substring(0, whitespaceCharIndex);
            var lastName = playerName.Substring(whitespaceCharIndex + 1);

            var newPlayer = new PlayerEntity {
                FirstName = firstName,
                LastName = lastName
            };

            TestDbAdapter.CreatePlayer(newPlayer);
            _playerToAdd.Add(new PlayerId(newPlayer.Id));
        }

        [Given(@"(.*) is a team player")]
        public void GivenPlayerIsATeamPlayer(string playerName)
        {
            var whitespaceCharIndex = playerName.IndexOf(' ');
            var firstName = playerName.Substring(0, whitespaceCharIndex);
            var lastName = playerName.Substring(whitespaceCharIndex + 1);

            var newPlayer = new PlayerEntity {
                FirstName = firstName,
                LastName = lastName
            };

            TestDbAdapter.CreatePlayer(newPlayer);
            TestDbAdapter.AssignPlayerToTeam(newPlayer.Id, _teamId.Id);
        }

        [Given(@"I have removed (.*)")]
        public void GivenIHaveRemovedPlayer(string playerName)
        {
            var whitespaceCharIndex = playerName.IndexOf(' ');
            var firstName = playerName.Substring(0, whitespaceCharIndex);
            var lastName = playerName.Substring(whitespaceCharIndex + 1);

            using (var ctx = TestDbAdapter.Context)
            {
                var playerEntities = ctx.Players
                    .SingleOrDefault(p => p.FirstName == firstName && p.LastName == lastName);
                _playersToRemove.Add(new PlayerId(playerEntities.Id));
            }
        }

        [Given(@"(.*) is a team captain")]
        public void GivenJaneDoeIsATeamCaptain(string playerName)
        {
            var whitespaceCharIndex = playerName.IndexOf(' ');
            var firstName = playerName.Substring(0, whitespaceCharIndex);
            var lastName = playerName.Substring(whitespaceCharIndex + 1);

            using (var ctx = TestDbAdapter.Context)
            {
                var playerEntity = ctx.Players
                    .SingleOrDefault(p => p.Id == _captainId.Id);

                if (playerEntity == null)
                {
                    return;
                }

                playerEntity.FirstName = firstName;
                playerEntity.LastName = lastName;
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
            catch (ArgumentException exc)
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
                _exception.Should().BeNull("Should not thrown exception");
            }
        }

        [Then(@"ArgumentException is thrown")]
        public void ThenInvalidOperationExceptionIsThrown()
        {
            _exception.Should().BeOfType(typeof(ArgumentException), "Should thrown ArgumentException");
        }
    }
}
