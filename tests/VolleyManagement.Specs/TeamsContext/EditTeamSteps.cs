using AutoMapper;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;
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
    [Scope(Feature = "Edit Team")]
    public class EditTeamSteps
    {
        private readonly ITeamService _teamService;

        private readonly TeamEntity _team;
        private readonly PlayerEntity _player;
        private PlayerEntity _captain;
        private Exception _exception;
        private bool _isExceptionThrown;


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

        [Given(@"(.*) team exists")]
        public void GivenTeamExists(string name)
        {
            _team.Name = name;
            TestDbAdapter.CreateTeam(_team);
            TestDbAdapter.AssignPlayerToTeam(_player.Id, _team.Id);
            _team.Players.Add(_player);
        }

        [Given(@"name set to Very Looong team name which should be more than (.*) symbols")]
        public void GivenNameChangedToNameWhichShouldBeMoreThan(int newNameLength)
        {
            var newName = new string('n', newNameLength + 1);
            _team.Name = newName;
        }

        [Given(@"Team (.*) team does not exist")]
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
                _isExceptionThrown = true;
            }
        }

        [When(@"I execute ChangeTeamCaptain")]
        public void WhenIExecuteChangeTeamCaptain()
        {
            _teamService.ChangeCaptain(new TeamId(_team.Id), new PlayerId(_captain.Id));
            _team.Captain = _captain;
            _team.CaptainId = _captain.Id;
        }

        [Then(@"team is updated succesfully")]
        public void ThenTeamIsUpdatedSuccesfully()
        {
            using (var context = TestDbAdapter.Context)
            {

                var teamEntity = context.Teams.Find(_team.Id);      
                teamEntity.Should().BeEquivalentTo(_team,options=>options.Including(x=>x.CaptainId).Including(x=>x.Name));
                teamEntity.Players.Should().BeEquivalentTo(_team.Players, options => options.Including(x => x.Id));                
                teamEntity.Should().NotBe(null);
            }
        }

        [Then(@"(.*) is thrown")]
        public void ThenExceptionIsThrown(string exceptionType)
        {
            _isExceptionThrown.Should().BeTrue();
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
