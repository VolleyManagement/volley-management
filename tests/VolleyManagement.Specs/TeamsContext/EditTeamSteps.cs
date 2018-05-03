using AutoMapper;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Contracts.Exceptions;
using VolleyManagement.Data.Exceptions;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Domain.TeamsAggregate;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;
using VolleyManagement.UnitTests.Services.TeamService;
using Xunit;

namespace VolleyManagement.Specs.TeamsContext
{
    [Binding]
    public class EditTeamSteps
    {
        private readonly ITeamService _teamService;

        private readonly TeamEntity _team;
        private readonly PlayerEntity _player;
        private PlayerEntity _captain;
        private Exception _exception;
        private bool isExceptionThrown;


        public EditTeamSteps()
        {
            _teamService = IocProvider.Get<ITeamService>();

            _player = new PlayerEntity {
                FirstName = "FirstName",
                LastName = "LastName",
            };

            _team = new TeamEntity {
                Name = "TestName",
                Coach = "New coach",
                Achievements = "Achive",
                Captain = _player
            };
        }
        #region Given

        [Given(@"(.*) team exists")]
        [Scope(Feature = "Edit Team")]
        public void GivenTeamExists(string name)
        {
            _team.Name = name;
            TestDbAdapter.CreateTeam(_team);
            TestDbAdapter.AssignPlayerToTeam(_player.Id, _team.Id);
            _team.Players.Add(_player);
        }

        [Given(@"name set to Very Looong team name which should be more than (.*) symbols")]
        [Scope(Feature = "Edit Team")]
        public void GivenNameChangedToNameWhichShouldBeMoreThan(int newNameLength)
        {
            var newName = new string('n', newNameLength + 1);
            _team.Name = newName;
        }

        [Given(@"Team (.*) team does not exist")]
        [Scope(Feature = "Edit Team")]
        public void GivenTeamDoesNotExist(string name)
        {
            _team.Name = name;
            _team.Captain = _player;
        }

        [Given(@"name changed to (.*)")]
        public void GivenNameChangedToAnother(string newName)
        {
            _team.Name = newName;
        }

        [Given(@"captain is changed to (.*)")]
        public void GivenCaptainIsChangedToAnother(string captainName)
        {
            var whitespaceCharIndex = captainName.IndexOf(' ');
            var firstName = captainName.Substring(0, whitespaceCharIndex);
            var lastName = captainName.Substring(whitespaceCharIndex + 1);

            _captain = new PlayerEntity {
                FirstName = firstName,
                LastName = lastName,
            };

            TestDbAdapter.CreatePlayer(_captain);
            _team.Players.Add(_captain);
        }

        #endregion

        #region When
        [When(@"I execute EditTeam")]
        public void WhenIExecuteEditTeam()
        {
            try
            {
                var team = new Team(_team.Id,
                    _team.Name,
                    _team.Coach,
                    _team.Achievements,
                    new PlayerId(_team.Captain.Id),
                    new List<PlayerId>()
                );

                _teamService.Edit(team);
            }
            catch (Exception exception)
            {
                _exception = exception;
                isExceptionThrown = true;
            }
        }

        [When(@"I execute ChangeTeamCaptain")]
        public void WhenIExecuteChangeTeamCaptain()
        {
            _teamService.ChangeCaptain(new TeamId(_team.Id), new PlayerId(_captain.Id));
            _team.Captain = _captain;
            _team.CaptainId = _captain.Id;
        }

        #endregion
        [Then(@"team is updated succesfully")]
        public void ThenTeamIsUpdatedSuccesfully()
        {
            using (var context = TestDbAdapter.Context)
            {
                var teamEntity = context.Teams.Find(_team.Id);

                var isUpdatedSuccesfully = true;
                isUpdatedSuccesfully = teamEntity.Id == _team.Id &&
                    teamEntity.Name.Equals(_team.Name) &&
                    teamEntity.Coach.Equals(_team.Coach) &&
                    teamEntity.Achievements.Equals(_team.Achievements) &&
                    teamEntity.Captain.Id == _team.Captain.Id;

                if (isUpdatedSuccesfully)
                {
                    var xRosterIds = teamEntity.Players.OrderByDescending(p => p.Id).Select(p => p.Id);
                    var yRosterIds = _team.Players.OrderByDescending(p => p.Id).Select(p => p.Id);

                    isUpdatedSuccesfully = xRosterIds.SequenceEqual(yRosterIds);
                }

                teamEntity.Should().NotBe(null);
                isUpdatedSuccesfully.Should().BeTrue("Expected and actual should be equal");
            }
        }

        [Then(@"(.*) is thrown")]
        [Scope(Feature = "Edit Team")]
        public void ThenExceptionIsThrown(string exceptionType)
        {
            isExceptionThrown.Should().BeTrue();
            if (exceptionType == "MissingEntityException")
            {
                _exception.Should().BeOfType(typeof(MissingEntityException));
            }

            if (exceptionType == "EntityInvariantViolationException")
            {
                _exception.Should().BeOfType(typeof(EntityInvariantViolationException));
            }
        }
    }

}
