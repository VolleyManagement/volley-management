using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using LanguageExt;
using SimpleInjector;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.Handlers;
using VolleyM.Domain.Players.PlayerAggregate;
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

		private IRequestHandlerOld<GetAll.Request, List<PlayerDto>> _handler;

		private List<PlayerDto> _expectedResult;
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
		public async Task GivenSeveralPlayersExist()
		{
			var inputData = GetSeveralPlayersCaseData();
			await _testFixture.MockSeveralPlayersExist(_testFixture.CurrentTenant, inputData);

			_expectedResult = GetSeveralPlayersCaseExpectedData(inputData);
		}

		[When(@"I query all players")]
		public async Task WhenIQueryAllPlayers()
		{
			_handler = _container.GetInstance<IRequestHandlerOld<GetAll.Request, List<PlayerDto>>>();

			_actualResult = await _handler.Handle(new GetAll.Request());
		}

		[Then(@"all players received")]
		public void ThenAllPlayersReceived()
		{
			_actualResult.ShouldBeEquivalent(_expectedResult, "handler should return all available players");
		}

		private static List<PlayerDto> GetSeveralPlayersCaseExpectedData(List<Player> inputData) =>
			inputData
				.Select(p => new PlayerDto
				{
					Tenant = p.Tenant,
					Id = p.Id,
					FirstName = p.FirstName,
					LastName = p.LastName
				})
				.ToList();

		private List<Player> GetSeveralPlayersCaseData()
		{
			return GetFaker(_testFixture.CurrentTenant).Generate(3);
		}

		private static Faker<Player> GetFaker(TenantId tenant)
		{
			return new Faker<Player>()
				.StrictMode(true)
				.CustomInstantiator(f =>
					new Player(
						tenant,
						new PlayerId(f.Random.Utf16String(10, 20)),
						f.Person.FirstName,
						f.Person.LastName));
		}
	}
}
