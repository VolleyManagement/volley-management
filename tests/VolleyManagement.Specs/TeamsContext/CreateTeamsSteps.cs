using System;
using System.Collections.Generic;
using System.Linq;
using VolleyManagement.Contracts.Exceptions;
using AutoMapper;
using FluentAssertions;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Domain.Properties;
using VolleyManagement.Domain.TeamsAggregate;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;
using Xunit;
using VolleyManagement.UnitTests.Services.TeamService;

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
        private bool _isSetCaprainException;

        private readonly string teamShouldBeSavedToDb =
            "Team should've been saved into the database";

        public CreateTeamsSteps()
        {
            _teamService = IocProvider.Get<ITeamService>();
            _team = new Team(int.MaxValue,
                "Team",
                "Coach",
                "Achievements",
                new PlayerId(_captainId),
                new List<PlayerId>());
        }

        [Given(@"team name is (.*)")]
        public void GivenTeamNameIs(string teamName)
        {
            _team.Name = teamName;
        }

        [Given(@"captain is (.*)")]
        public void GivenCaptainIs(string fullName)
        {
            var removeNotExistingPlayerFromRosterWhoWasCaptainInTeamCreation =
                new List<PlayerId> { new PlayerId(100) };
            RegisterNewPlayerAndSetCaptainId(fullName);
            _team.SetCaptain(new PlayerId(_captainId));
            _team.RemovePlayers(removeNotExistingPlayerFromRosterWhoWasCaptainInTeamCreation);
        }

        [Given(@"captain empty")]
        public void GivenCaptainIsEmpty()
        {
            try
            {
                _team.SetCaptain(null);
            }
            catch (Exception ex)
            {
                _isSetCaprainException = true;
                _exception = ex;
            }
        }

        [Given(@"coach is (.*)")]
        public void GivenCoachIs(string coach)
        {
            _team.Coach = coach;
        }

        [Given(@"achievements are (.*)")]
        public void GivenAchievementsAre(string achievements)
        {
            _team.Achievements = achievements;
        }

        [When(@"I execute CreateTeam")]
        public void WhenIExecuteCreateTeam()
        {
            try
            {
                var teamDto = Mapper.Map<CreateTeamDto>(_team);

                _team = _teamService.Create(teamDto);

                _newTeamId = _team.Id;
            }
            catch (Exception ex)
            {
                _exception = ex;
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
        public void ThenEntityInvariantViolationExceptionIsThrown()
        {
            if (_isSetCaprainException)
            {
                _exception.Should().BeOfType(typeof(MissingEntityException), "Should thrown MissingEntityException");
            }
            else
            {
                _exception.Should().BeOfType(typeof(ArgumentException), "Should thrown ArgumentException");
            }
        }

        private void RegisterNewPlayerAndSetCaptainId(string fullName)
        {
            var whitespaceCharIndex = fullName.IndexOf(' ');
            var firstName = fullName.Substring(0, whitespaceCharIndex);
            var lastName = fullName.Substring(whitespaceCharIndex + 1);

            var playerService = IocProvider.Get<IPlayerService>();

            var newPlayer = playerService.Create(
                new CreatePlayerDto {
                    FirstName = firstName,
                    LastName = lastName
                });

            _captainId = newPlayer.Id;
        }
    }
}
