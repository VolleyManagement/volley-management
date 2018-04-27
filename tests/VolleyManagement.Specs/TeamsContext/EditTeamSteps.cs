using AutoMapper;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
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

        private TeamEntity _team;
        private PlayerEntity _player;

        private string NewName;
        private string longLongName;
        private int _captainId = 100;
        private int _newTeamId;
        
        public EditTeamSteps()
        {
            _teamService = IocProvider.Get<ITeamService>();
            _playerService = IocProvider.Get<IPlayerService>();

            _player = new PlayerEntity {
                FirstName = "Oleh",
                LastName = "Gerus",
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
            
            TestDbAdapter.AssignPlayerToTeam(_player.Id,_team.Id);
        }
        
        [Given(@"name changed to (.*)")]
        public void GivenNameChangedTo(string newName)
        {
            _team.Name = newName;
        }
        
        [Given(@"name changed to Very looooooooooooooooooooooooong team name which should be more than (.*) symbols")]
        public void GivenNameChangedToNameWhichShouldBeMoreThan(int lenght)
        {
            longLongName = new string('a', lenght + 1);
            _team.Name = longLongName;
        }
        
        [Given(@"Team (.*) team does not exist")]
        [Scope(Feature = "Edit Team")]
        public void GivenTeamDoesNotExist()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"name changed to (.*)")]
        public void GivenNameChangedToAnother()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"captain is changed to (.*)")]
        public void GivenCaptainIsChangedToAnother()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I execute EditTeam")]
        public void WhenIExecuteEditTeam()
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
        
        [When(@"I execute ChangeTeamCaptain")]
        public void WhenIExecuteChangeTeamCaptain()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"team is updated succesfully")]
        public void ThenTeamIsUpdatedSuccesfully()
        {
            TeamEntity teamFromDb;
            using (var context = TestDbAdapter.Context)
            {
                teamFromDb = context.Teams.Find(_team.Id);
            }
            Assert.Equal(teamFromDb.Name, _team.Name);
        }

        [Then(@"EntityInvariantViolationException is thrown")]
        public void ThenEntityInvariantViolationExceptionisthrown()
        {
            _team.Name.Should().BeEquivalentTo(NewName);
        }
    }
}
