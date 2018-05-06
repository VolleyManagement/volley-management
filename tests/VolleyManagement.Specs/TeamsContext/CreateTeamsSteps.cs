using System;
using System.Collections.Generic;
using System.Linq;
using VolleyManagement.Contracts.Exceptions;
using AutoMapper;
using FluentAssertions;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Data.Contracts;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Domain.Properties;
using VolleyManagement.Domain.TeamsAggregate;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;
using Xunit;
using VolleyManagement.UnitTests.Services.TeamService;
using Constants = VolleyManagement.Domain.Constants;

namespace VolleyManagement.Specs.TeamsContext
{
    [Binding]
    [Scope(Feature = "CreateTeams")]
    public class CreateTeamsSteps
    {
        private readonly ITeamService _teamService;
        private Team _team;
        private int _captainId = 100;
        private int _newTeamId;
        private Exception _exception;
        private bool _captainExceptionShouldBeSetted;
        private string _teamName = "TeamName";
        private string _coach = "Coach";
        private string _achievements = "_Achievements";
        private PlayerId _captain = new PlayerId(Constants.Player.MIN_ID);
        private IEnumerable<PlayerId> _playerList = new List<PlayerId>();


        private readonly string teamShouldBeSavedToDb =
            "Team should've been saved into the database";

        public CreateTeamsSteps()
        {
            _teamService = IocProvider.Get<ITeamService>();
        }

        [Given(@"team name is (.*)")]
        public void GivenTeamNameIs(string teamName)
        {
            _teamName = teamName;
        }

        [Given(@"captain is (.*)")]
        public void GivenCaptainIs(string fullName)
        {
            RegisterNewPlayerAndSetCaptainId(fullName);
            _captain = new PlayerId(_captainId);
        }

        [Given(@"captain empty")]
        public void GivenCaptainIsEmpty()
        {
            _captain = null;
        }

        [Given(@"coach is (.*)")]
        public void GivenCoachIs(string coach)
        {
            _coach = coach;
        }

        [Given(@"achievements are (.*)")]
        public void GivenAchievementsAre(string achievements)
        {
            _achievements = achievements;
        }

        [When(@"I execute CreateTeam")]
        public void WhenIExecuteCreateTeam()
        {
            try
            {

                var teamDto = new CreateTeamDto {
                    Name = _teamName,
                    Coach = _coach,
                    Achievements = _achievements,
                    Captain = _captain,
                    Roster = _playerList
                };

                _team = _teamService.Create(teamDto);
                _newTeamId = _team.Id;
            }
            catch (Exception ex)
            {
                _exception = ex;
                if (_captain == null)
                {
                    _captainExceptionShouldBeSetted = true;
                }
            }
        }

        [Then(@"new team gets new Id")]
        public void ThenNewTeamGetsNewId()
        {
            Assert.NotEqual(default(int), _newTeamId);
            Assert.NotEqual(int.MaxValue, _newTeamId);
        }

        [Then(@"new team should be succesfully added")]
        public void ThenNewTeamShouldBeSuccesfullyAdded()
        {
            var teamComparer = new TeamComparer();
            TeamEntity teamEntity;
            IEnumerable<PlayerId> roster = new List<PlayerId>();
            using (var context = TestDbAdapter.Context)
            {
                teamEntity = context.Teams.Find(_team.Id);
                if (teamEntity != null)
                {
                    roster = teamEntity.Players.Select(x => new PlayerId(x.Id));
                }
            }

            teamEntity.Should().NotBe(null, teamShouldBeSavedToDb);

            var teamFromDb = new Team(teamEntity.Id, teamEntity.Name, teamEntity.Coach, teamEntity.Achievements, new PlayerId(teamEntity.CaptainId), roster);

            teamComparer.AreEqual(teamFromDb, _team).Should().BeTrue();
        }

        [Then(@"Validation fails")]
        public void ThenValidationFails()
        {
            if (_captainExceptionShouldBeSetted)
            {
                _team.Should().Be(null);
            }
            else
            {
                _exception.Should().BeOfType(typeof(ArgumentException), "Should thrown ArgumentException");
            }
        }

        private void RegisterNewPlayerAndSetCaptainId(string fullName)
        {
            var names = SpecsHelper.SplitFullNameToFirstLastNames(fullName);

            var playerService = IocProvider.Get<IPlayerService>();

            var newPlayer = playerService.Create(
                new CreatePlayerDto {
                    FirstName = names.FirstName,
                    LastName = names.LastName
                });

            _captainId = newPlayer.Id;
        }
    }
}
