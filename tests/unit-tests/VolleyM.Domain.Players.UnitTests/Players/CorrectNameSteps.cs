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
		private SpecFlowTransform _transform;

		private PlayerId _existingPlayerId;
		private CorrectName.Request _request;
		private Either<Error, Unit> _actualResult;
		private TestPlayerDto _originalPlayer;

		public CorrectNameSteps(IPlayersTestFixture testFixture, IAuthFixture authFixture, Container container)
		{
			_testFixture = testFixture;
			_authFixture = authFixture;
			_container = container;
		}

		[BeforeScenario(Order = Constants.BEFORE_SCENARIO_STEPS_ORDER)]
		public void ScenarioSetup()
		{
			_transform = _container.GetInstance<SpecFlowTransform>();
			_authFixture.SetTestUserPermission(PlayersConstants.Name, nameof(CorrectName));
		}

		[Given(@"player exists")]
		public async Task GivenPlayerExists(Table table)
		{
			var player = _transform.GetInstance<TestPlayerDto>(table);
			_existingPlayerId = player.Id;
			_testFixture.MockNextRandomId(_existingPlayerId.ToString());
			_originalPlayer = player;

			await _testFixture.MockPlayerExists(player);
		}

		[Given(@"I have CorrectNameRequest")]
		public void GivenIHaveCorrectNameRequest(Table table)
		{
			_request = GetPlayer(table);
		}

		[When(@"I execute CorrectName")]
		public async Task WhenIExecuteCorrectName()
		{
			var handler = _container.GetInstance<IRequestHandler<CorrectName.Request, Unit>>();
			_actualResult = await handler.Handle(_request).ToEither();
		}

		[Then(@"success result is returned")]
		public void ThenSuccessResultIsReturned()
		{
			_actualResult.ShouldBeEquivalent(Unit.Default);
		}

		[Then(@"player name is")]
		public async Task ThenPlayerNameIs(Table table)
		{
			var expectedPlayer = table.CreateInstance<CorrectPlayerNameDto>();

			var playerRepository = _container.GetInstance<IPlayersRepository>();
			var actualPlayer = await playerRepository.Get(_testFixture.CurrentTenant, _existingPlayerId).ToEither();

			actualPlayer.ShouldBeEquivalent(expectedPlayer);
		}

		[Then(@"player is not changed")]
		public async Task ThenPlayerIsNotChanged()
		{
			var playerRepository = _container.GetInstance<IPlayersRepository>();
			var actualPlayer = await playerRepository.Get(_testFixture.CurrentTenant, _existingPlayerId).ToEither();

			actualPlayer.ShouldBeEquivalent(_originalPlayer);
		}

		[Then(@"ValidationError is returned")]
		public void ThenValidationErrorIsReturned()
		{
			_actualResult.ShouldBeError(ErrorType.ValidationFailed);
		}

		private CorrectName.Request GetPlayer(Table table)
		{
			var player = _transform.GetInstance<CorrectName.Request>(table);

			_testFixture.SetupPlayerName(player);

			return player;
		}

		private class CorrectPlayerNameDto
		{
			public string FirstName { get; set; }
			public string LastName { get; set; }
		}
	}
}