﻿using System.Collections.Generic;
using System.Data;
using VolleyManagement.Domain.PlayersAggregate;
using FluentAssertions;
using System;
using TechTalk.SpecFlow;
using VolleyManagement.Contracts;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Domain.TeamsAggregate;
using VolleyManagement.Specs.Infrastructure;
using VolleyManagement.Specs.Infrastructure.IOC;
using System.Data.Entity.Infrastructure;

namespace VolleyManagement.Specs.TeamsContext
{
    [Binding]
    [Scope(Feature = "Remove Team")]
    public class RemoveTeamSteps
    {
        private TeamId _teamId;
        private Exception _exception;
        private readonly ITeamService _teamService;

        private const int ID_TEAM_DOES_NOT_EXIST = 1;

        public RemoveTeamSteps()
        {
            _teamService = IocProvider.Get<ITeamService>();
        }

        [Given(@"(.*) team exists")]
        public void GivenTeamExists(string name)
        {
            var team = TestDbAdapter.CreateTeamWithCaptain(name, name, "Captain");
            _teamId = new TeamId(team.Id);
        }

        [Given(@"(.*) team does not exist")]
        public void GivenTeamDoesNotExist(string name)
        {
            _teamId = new TeamId(ID_TEAM_DOES_NOT_EXIST);
        }

        [When(@"I execute DeleteTeam")]
        public void WhenIExecuteDeleteTeam()
        {
            try
            {
                _teamService.Delete(_teamId);

            }
            catch (Exception exc)
            {
                _exception = exc;
            }

        }

        [Then(@"team is removed")]
        public void ThenTeamIsRemoved()
        {
            TeamEntity teamEntity;
            using (var context = TestDbAdapter.Context)
            {
                teamEntity = context.Teams.Find(_teamId.Id);
            }

            teamEntity.Should().Be(null, "Team should been deleted from the database");
            _exception.Should().Be(null, "Exception should not been thown");
        }

        [Then(@"ConcurrencyException is thrown")]
        public void ThenConcurrencyExceptionIsThrown()
        {
            _exception.Should().BeOfType(typeof(DbUpdateConcurrencyException), "ConcurrencyException should been thown");
        }
    }
}
