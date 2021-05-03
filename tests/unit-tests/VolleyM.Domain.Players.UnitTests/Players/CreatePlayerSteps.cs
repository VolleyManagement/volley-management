﻿using System.Linq;
using System.Threading.Tasks;
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
	[Scope(Feature = "Create Player")]
	public class CreatePlayerSteps
	{
		private readonly IPlayersTestFixture _testFixture;
		private SpecFlowTransform _transform;
		private readonly IAuthFixture _authFixture;
		private readonly Container _container;

		private Create.Request _request;
		private Player _expectedPlayer;

		private Either<Error, Player> _actualResult;

		public CreatePlayerSteps(Container container, IAuthFixture authFixture, IPlayersTestFixture testFixture)
		{
			_container = container;
			_authFixture = authFixture;
			_testFixture = testFixture;
		}

		[BeforeScenario(Order = Constants.BEFORE_SCENARIO_STEPS_ORDER)]
		public void ScenarioSetup()
		{
			_transform = _container.GetInstance<SpecFlowTransform>();
			_authFixture.SetTestUserPermission(PlayersConstants.Name, nameof(Create));
		}

		[Given(@"I have CreateRequest")]
		public void GivenIHaveCreatePlayerRequest(Table table)
		{
			_request = GetPlayer(table);

			var playerId = new PlayerId("player1");
			_testFixture.MockNextRandomId(playerId.ToString());

			_expectedPlayer = new Player(_testFixture.CurrentTenant, new Version("<some-version>"),  playerId, _request.FirstName, _request.LastName);
		}

		[When(@"I execute Create")]
		public async Task WhenIExecuteCreatePlayer()
		{
			var handler = _container.GetInstance<IRequestHandler<Create.Request, Player>>();
			var result = handler.Handle(_request);
			_actualResult = await result.ToEither();
		}

		[Then(@"player is created")]
		public async Task ThenPlayerIsCreated()
		{
			await _testFixture.VerifyPlayerCreated(_expectedPlayer);
		}

		[Then(@"player is returned")]
		public void ThenPlayerIsReturned()
		{
			_actualResult.ShouldBeEquivalent(_expectedPlayer, "created player should be returned");
		}

		[Then(@"player is not created")]
		public async Task ThenPlayerIsNotCreated()
		{
			await _testFixture.VerifyPlayerNotCreated(_expectedPlayer);
		}

		[Then(@"ValidationError is returned")]
		public void ThenValidationErrorIsReturned()
		{
			_actualResult.ShouldBeError(ErrorType.ValidationFailed);
		}

		private Create.Request GetPlayer(Table table)
		{
			var player = _transform.GetInstance<Create.Request>(table);

			_testFixture.SetupPlayerName(player);

			return player;
		}

		private static string SetNameField(string val)
		{
			if (val == "<60+ symbols name>")
			{
				return new string(Enumerable.Repeat('a', 61).ToArray());
			}
			if (val == "<null>")
			{
				return null;
			}

			return val;
		}
	}
}
