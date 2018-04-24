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
        private Team _team;
        private string NewName;
        private string longLongName;
        private int _captainId = 100;
        private int _newTeamId;

   
        public EditTeamSteps()
        {
            _teamService = IocProvider.Get<ITeamService>();
            _team = new Team(int.MaxValue,
                "Team",
                "Coach",
                "Achievements",
                new PlayerId(_captainId),
                new List<PlayerId>());

        }


        [Given(@"(.*) team exists")]
        public void GivenTeamExists(string testName)
        {
            
            _team.Name = testName;
            var teamDto = Mapper.Map<CreateTeamDto>(_team);
            _team = _teamService.Create(teamDto);
        }
        
        [Given(@"name changed to (.*)")]
        public void GivenNameChangedTo(string newName)
        {
            NewName = newName;
            _team.Name = newName;
        }
        
        [Given(@"name changed to Very looooooooooooooooooooooooong team name which should be more than (.*) symbols")]
        public void GivenNameChangedToVeryLooooooooooooooooooooooooongTeamNameWhichShouldBeMoreThanSymbols(int lenght)
        {
            longLongName = new string('a', lenght + 1);
            _team.Name = longLongName;
        }
        
        [Given(@"Team B team does not exist")]
        public void GivenTeamBTeamDoesNotExist()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"name changed to B-Team")]
        public void GivenNameChangedToB_Team()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"captain is changed to Captain B")]
        public void GivenCaptainIsChangedToCaptainB()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I execute EditTeam")]
        public void WhenIExecuteEditTeam()
        {
            _teamService.Edit(_team);
        }
        
        [When(@"I execute ChangeTeamCaptain")]
        public void WhenIExecuteChangeTeamCaptain()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"team is updated succesfully")]
        public void ThenTeamIsUpdatedSuccesfully()
        {
            _team.Name.Should().BeEquivalentTo(NewName);
        }

        
        [Then(@"EntityInvariantViolationException is thrown")]
        public void ThenEntityInvariantViolationExceptionisthrown()
        {
            _team.Name.Should().BeEquivalentTo(NewName);
        }

    }
}
