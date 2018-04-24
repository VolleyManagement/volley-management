using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Domain.TeamsAggregate;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;

namespace VolleyManagement.Specs.TeamsContext
{
    [Binding]
    public class ManageTeamPlayersSteps
    {
        private ITeamService _teamService;
        private IPlayerService _playerService;
        private Team _team;
        private Player _player;
        private Player _captain;

        private int _captainId = 100;

        public ManageTeamPlayersSteps()
        {
            _teamService = IocProvider.Get<ITeamService>();
            _playerService = IocProvider.Get<IPlayerService>();

            _captain = new Player(_captainId, "CaptainFirst", "CaptainLast", null, null, null);

            int capId = _playerService.Create(Mapper.Map<CreatePlayerDto>(_captain)).Id;
            _team = new Team(int.MaxValue,
                "Team",
                "Coach",
                "Achievements",
                new PlayerId(capId),
                new List<PlayerId>());
            
            var team = _teamService.Create(new CreateTeamDto {
                Name = _team.Name,
                Coach = _team.Coach,
                Achievements = _team.Achievements,
                Captain = _team.Captain,
                Roster = _team.Roster
            });

         //   _teamService.ChangeCaptain(new TeamId(team.Id), new PlayerId(capId));
            _player = new Player(int.MaxValue, "First", "Last", null, null, null);
        }

        [Given(@"Team (.*) exists")]
        public void GivenTeamExists(string teamName)
        {
            _team.Name = teamName;
            var teamDto = Mapper.Map<CreateTeamDto>(_team);
            _team = _teamService.Create(teamDto);
        }

        [Given(@"I have added (.*) as a team player")]
        public void GivenIHaveAddedJaneDoeAsATeamPlayer(string playerName)
        {
            var whitespaceCharIndex = playerName.IndexOf(' ');
            var firstName = playerName.Substring(0, whitespaceCharIndex);
            var lastName = playerName.Substring(whitespaceCharIndex + 1);

            _player = _playerService.Create(
                new CreatePlayerDto {
                    FirstName = firstName,
                    LastName = lastName
                });
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
            _teamService.AddPlayers(new TeamId(_team.Id),
                                    new List<PlayerId> {
                                         new PlayerId(_player.Id)
                                    });

        }

        [When(@"I execute RemovePlayersFromTeam")]
        public void WhenIExecuteRemovePlayersFromTeam()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"players are added")]
        public void ThenPlayersAreAdded()
        {
            IEnumerable<PlayerEntity> playerEntities;
            using (var ctx = TestDbAdapter.Context)
            {
                playerEntities = ctx.Teams.First(team => team.Id == _team.Id).Players;
            }
            playerEntities.Should().AllBeEquivalentTo(_team.Roster);
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
