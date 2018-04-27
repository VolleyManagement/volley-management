using AutoMapper;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Data.Exceptions;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Domain.TeamsAggregate;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;
using Xunit;

namespace VolleyManagement.Specs.TeamsContext
{
    [Binding]
    public class EditTeamSteps
    {
        private readonly ITeamService _teamService;
        private readonly IPlayerService _playerService;

        private readonly TeamEntity _team;
        private readonly PlayerEntity _player;
        private ConcurrencyException _concurrencyException;
        private bool isExceptionThrown = false;


        public EditTeamSteps()
        {
            _teamService = IocProvider.Get<ITeamService>();
            _playerService = IocProvider.Get<IPlayerService>();

            _player = new PlayerEntity {
                FirstName = "FirstName",
                LastName = "LastName",
            };

            _team = new TeamEntity {
                Name = "TestName",
                Coach = "New coach",
                Captain = _player
            };
        }

        [Given(@"(.*) team exists")]
        [Scope(Feature = "Edit Team")]
        public void GivenTeamExists(string name)
        {
            _team.Name = name;
            TestDbAdapter.CreateTeam(_team);
            TestDbAdapter.AssignPlayerToTeam(_player.Id, _team.Id);
        }

        [Given(@"name changed to (.*) team name which should be more than 30 symbols")]
        [Scope(Feature = "Edit Team")]
        public void GivenNameChangedToNameWhichShouldBeMoreThan(string newName)
        {
            _team.Name = newName;
        }

        [Given(@"Team (.*) team does not exist")]
        [Scope(Feature = "Edit Team")]
        public void GivenTeamDoesNotExist(string name)
        {
            _team.Name = name;
        }

        [Given(@"name changed to (.*)")]
        public void GivenNameChangedToAnother(string newName)
        {
            _team.Name = newName;
        }

        [Given(@"captain is changed to (.*)")]
        public void GivenCaptainIsChangedToAnother()
        {
            ScenarioContext.Current.Pending();
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
            catch (ArgumentException e)
            {
                isExceptionThrown = true;
            }
            catch (ConcurrencyException e)
            {
                _concurrencyException = e;
                isExceptionThrown = true;
            }
            catch (Exception)
            {
                isExceptionThrown = true;
            }
        }

        [When(@"I execute ChangeTeamCaptain")]
        public void WhenIExecuteChangeTeamCaptain()
        {
            _teamService.ChangeCaptain(new TeamId(_team.Id), new PlayerId(_player.Id));
        }

        [Then(@"team is updated succesfully")]
        public void ThenTeamIsUpdatedSuccesfully()
        {
            TeamEntity teamFromDb;
            using (var context = TestDbAdapter.Context)
            {
                teamFromDb = context.Teams.Find(_team.Id);
            }
            teamFromDb.Name.Should().BeEquivalentTo(_team.Name);
            teamFromDb.Captain.Should().BeEquivalentTo(_team.Captain);
            teamFromDb.Players.Should().BeEquivalentTo(_team.Players);
        }

        [Then(@"(.*) is thrown")]
        [Scope(Feature = "Edit Team")]
        public void ThenExceptionisthrown(string exceptionType)
        {
            isExceptionThrown.Should().BeTrue();
            if (exceptionType == "ConcurrencyException")
            {
                _concurrencyException.Should().BeOfType(typeof(ConcurrencyException));
            }

            if (exceptionType == "ArgumentException")
            {
                _concurrencyException.Should().BeOfType(typeof(ArgumentException));
            }
        }
    }

}
