using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using LanguageExt;
using Serilog;
using SimpleInjector;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.Handlers;
using VolleyM.Domain.Players.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests
{
	[Binding]
	[Scope(Feature = "Get All players")]
	public class GetAllPlayersSteps
	{
		private readonly IPlayersTestFixture _testFixture;
		private readonly IAuthFixture _authFixture;
		private readonly Container _container;

		private IRequestHandler<GetAll.Request, List<PlayerDto>> _handler;

		private Either<Error, List<PlayerDto>> _actualResult;

		public GetAllPlayersSteps(IPlayersTestFixture testFixture, IAuthFixture authFixture, Container container)
		{
			_testFixture = testFixture;
			_authFixture = authFixture;
			_container = container;

			// Configure seed to have deterministic results
			Randomizer.Seed = new Random(1116170520);
		}

		[BeforeScenario(Order = Constants.BEFORE_SCENARIO_STEPS_ORDER)]
		public void ScenarioSetup()
		{
			_authFixture.SetTestUserPermission(PlayersConstants.Name, nameof(GetAll));
		}

		[Given(@"several players exist")]
		public async Task GivenSeveralPlayersExist(Table table)
		{
			var inputData = table.CreateSet<TestPlayerDto>();
			await _testFixture.MockSeveralPlayersExist(_testFixture.CurrentTenant, inputData.ToList());
		}

		[When(@"I query all players")]
		public async Task WhenIQueryAllPlayers()
		{
			_handler = _container.GetInstance<IRequestHandler<GetAll.Request, List<PlayerDto>>>();

			_actualResult = await _handler.Handle(new GetAll.Request()).ToEither();
		}

		[Then(@"all players are returned")]
		public void ThenAllPlayersReceived(Table table)
		{
			Log.Warning("ThenAllPlayersReceived invoked; Thread: {ThreadId}", Thread.CurrentThread.ManagedThreadId);
			var expectedResult = table.CreateSet<PlayerDto>();

			_actualResult.ShouldBeEquivalent(expectedResult, "handler should return all available players");
		}
	}
}
