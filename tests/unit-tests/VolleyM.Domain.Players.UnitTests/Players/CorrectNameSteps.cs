using System.Threading.Tasks;
using LanguageExt;
using SimpleInjector;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.Handlers;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.Players.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests.Players
{
	[Binding]
	[Scope(Feature = "Correct Player Name")]
	public class CorrectNameSteps
	{
		private readonly IPlayersTestFixture _testFixture;
		private readonly IAuthFixture _authFixture;
		private readonly Container _container;


		private CorrectName.Request _request;
		private Either<Error, Unit> _actualResult;

		public CorrectNameSteps(IPlayersTestFixture testFixture, IAuthFixture authFixture, Container container)
		{
			_testFixture = testFixture;
			_authFixture = authFixture;
			_container = container;
		}

		[BeforeScenario(Order = Constants.BEFORE_SCENARIO_STEPS_ORDER)]
		public void ScenarioSetup()
		{
			_authFixture.SetTestUserPermission(PlayersConstants.Name, nameof(CorrectName));
		}

		[Given(@"player exists")]
		public async Task GivenPlayerExists(Table table)
		{
			var player = table.CreateInstance<TestPlayerDto>();

			await _testFixture.MockPlayerExists(player);
		}

		[Given(@"I have CorrectNameRequest")]
		public void GivenIHaveCorrectNameRequest(Table table)
		{
			_request = table.CreateInstance<CorrectName.Request>();
		}

		[When(@"I execute CorrectName")]
		public async Task WhenIExecuteCorrectName()
		{
			var handler = _container.GetInstance<IRequestHandler<CorrectName.Request, Unit>>();
			_actualResult = await handler.Handle(_request);
		}

		[Then(@"success result is returned")]
		public void ThenSuccessResultIsReturned()
		{
			_actualResult.ShouldBeEquivalent(Unit.Default);
		}

	}
}