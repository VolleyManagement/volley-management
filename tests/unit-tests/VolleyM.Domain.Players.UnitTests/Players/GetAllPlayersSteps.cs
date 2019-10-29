using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.Handlers;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Domain.Players;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.IdentityAndAccess;
using FluentAssertions;
using SimpleInjector;
using System.Threading.Tasks;

namespace VolleyM.Domain.Players.UnitTests
{
    [Binding]
    [Scope(Feature = "GetAllPlayers")]
    public class GetAllPlayersSteps
    {
        private readonly IPlayersTestFixture _testFixture;
        private readonly IAuthFixture _authFixture;
        private readonly Container _container;

        private IRequestHandler<GetAllPlayers.Request, List<PlayerDto>> _handler;

        private Result<List<PlayerDto>> _expectedResult;
        private Result<List<PlayerDto>> _actualResult;

        public GetAllPlayersSteps(IPlayersTestFixture testFixture, IAuthFixture authFixture, Container container)
        {
            _testFixture = testFixture;
            _authFixture = authFixture;
            _container = container;
        }

        [BeforeScenario(Order = Constants.BEFORE_SCENARIO_STEPS_ORDER)]
        public void ScenarioSetup()
        {
            _authFixture.SetTestUserPermission(new Permission(Permissions.Context, Permissions.GetAll));
        }        

        [Given(@"several players exist")]
        public void GivenSeveralPlayersExist()
        {
            _expectedResult = GetMockData();
            _testFixture.MockSeveralPlayersExist(GetMockData());
        }
        
        [When(@"I query all players")]
        public async Task WhenIQueryAllPlayers()
        {
            _handler = _container.GetInstance<IRequestHandler<GetAllPlayers.Request, List<PlayerDto>>>();

            _actualResult = await _handler.Handle(new GetAllPlayers.Request());
        }
        
        [Then(@"all players received")]
        public void ThenAllPlayersReceived()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult, "handler should return all available players");
        }

        private static List<PlayerDto> GetMockData() =>
            new List<PlayerDto>
            {
                new PlayerDto { FirstName = "Name1", LastName = "LastName1", BirthYear = 1980, Height = 180, Weight = 70 },
                new PlayerDto { FirstName = "Name2", LastName = "LastName2", BirthYear = 1985, Height = 180, Weight = 70 }
            };
    }
}
