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
    public class ManageTeamPlayersSteps
    {
        private readonly ITeamService _teamService;
        private TeamId _teamId;
        private PlayerId _captainId;
        private readonly List<PlayerId> _playerToAdd;
        private PlayerId _playerToRemove;
        private readonly List<PlayerId> _roster;
        private Exception _exception;

        public ManageTeamPlayersSteps()
        {
            _teamService = IocProvider.Get<ITeamService>();
            _playerToAdd = new List<PlayerId>();
            _roster = new List<PlayerId>();
        }

        [Given(@"Team (.*) exists")]
        [Scope(Feature = "Manage Team Players")]
        public void GivenTeamExists(string name)
        {
            var _player = new PlayerEntity {
                FirstName = "CapitanFirst",
                LastName = "CapitanLast"
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
            _captainId = new PlayerId(_player.Id);
        }

        [Given(@"I have added (.*) as a team player")]
        [Scope(Feature = "Manage Team Players")]
        public void GivenIHavePlayerAsATeamPlayer(string playerName)
        {
            var whitespaceCharIndex = playerName.IndexOf(' ');
            var firstName = playerName.Substring(0, whitespaceCharIndex);
            var lastName = playerName.Substring(whitespaceCharIndex + 1);

            var _newPlayer = new PlayerEntity {
                FirstName = firstName,
                LastName = lastName
            };

            TestDbAdapter.CreatePlayer(_newPlayer);
            _playerToAdd.Add(new PlayerId(_newPlayer.Id));
        }

        [Given(@"(.*) is a team player")]
        [Scope(Feature = "Manage Team Players")]
        public void GivenPlayerIsATeamPlayer(string playerName)
        {
            var whitespaceCharIndex = playerName.IndexOf(' ');
            var firstName = playerName.Substring(0, whitespaceCharIndex);
            var lastName = playerName.Substring(whitespaceCharIndex + 1);

            var _newPlayer = new PlayerEntity {
                FirstName = firstName,
                LastName = lastName
            };

            TestDbAdapter.CreatePlayer(_newPlayer);
            TestDbAdapter.AssignPlayerToTeam(_newPlayer.Id, _teamId.Id);
            _roster.Add(new PlayerId(_newPlayer.Id));
        }

        [Given(@"I have removed (.*)")]
        [Scope(Feature = "Manage Team Players")]
        public void GivenIHaveRemovedPlayer(string playerName)
        {
            var whitespaceCharIndex = playerName.IndexOf(' ');
            var firstName = playerName.Substring(0, whitespaceCharIndex);
            var lastName = playerName.Substring(whitespaceCharIndex + 1);

            using (var ctx = TestDbAdapter.Context)
            {
                var playerEntities = ctx.Players
                    .SingleOrDefault(p => p.FirstName == firstName && p.LastName == lastName);
                _playerToRemove = new PlayerId(playerEntities.Id);
            }
        }

        [Given(@"(.*) is a team captain")]
        [Scope(Feature = "Manage Team Players")]
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
                _teamService.RemovePlayers(_teamId,
                    _playerToRemove != null ? new List<PlayerId> { _playerToRemove } : _roster);
            }
            catch(ArgumentException exc)
            {
                _exception = exc;
            }
        }

        [Then(@"players are added")]
        public void ThenPlayersAreAdded()
        {
            IEnumerable<PlayerEntity> playerEntities;
            using (var ctx = TestDbAdapter.Context)
            {
                playerEntities = ctx.Teams.First(team => team.Id == _teamId.Id).Players;
                var entityRosterIds = playerEntities.Select(e => e.Id);
                var playersId = _playerToAdd.Select(e => e.Id);
                entityRosterIds.Should().Contain(playersId, "Players should be added");
            }
        }

        [Then(@"players are removed")]
        public void ThenPlayersAreRemoved()
        {
            IEnumerable<PlayerEntity> playerEntities;
            using (var ctx = TestDbAdapter.Context)
            {
                playerEntities = ctx.Teams.First(team => team.Id == _teamId.Id).Players;
                var entityRosterIds = playerEntities.Select(e => e.Id);
                var removedPlayers = _roster.Select(p => p.Id);
                entityRosterIds.Should().NotContain(removedPlayers, "Players should be removed");
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
