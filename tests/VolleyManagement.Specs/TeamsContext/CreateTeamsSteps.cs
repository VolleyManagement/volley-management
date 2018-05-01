using VolleyManagement.Contracts.Exceptions;

namespace VolleyManagement.Specs.TeamsContext
{
    using AutoMapper;
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TechTalk.SpecFlow;
    using VolleyManagement.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.Properties;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Specs.Infrastructure;
    using VolleyManagement.Specs.Infrastructure.IOC;
    using Xunit;

    using static VolleyManagement.Specs.ExceptionAssertion;
    using static VolleyManagement.Specs.TeamsContext.EntityDomainTeamEqualityAsserter;

    [Binding]
    public class CreateTeamsSteps
    {
        private readonly ITeamService _teamService;
        private Team _team;
        private int _captainId = 100;
        private int _newTeamId;
        private Exception _exception;

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
        [Scope(Feature = "CreateTeams")]
        public void GivenCaptainIs(string fullName)
        {
            RegisterNewPlayerAndSetCaptainId(fullName);

            _team.SetCaptain(new PlayerId(_captainId));
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
            TeamEntity teamEntity;
            using (var context = TestDbAdapter.Context)
            {
                teamEntity = context.Teams.Find(_team.Id);
            }

            teamEntity.Should().NotBe(null, teamShouldBeSavedToDb);
            AssertSimpleDataIsEqual(teamEntity, _team);
        }

        [Then(@"Validation fails")]
        public void ThenEntityInvariantViolationExceptionIsThrown()
        {
            _exception.Should().BeOfType(typeof(MissingEntityException), "Should thrown MissingEntityException");
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
