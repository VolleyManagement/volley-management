﻿using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using SimpleInjector;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.Handlers;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests
{
	[Binding]
	[Scope(Feature = "Create Player Old")]
	public class CreatePlayerStepsOld
	{
		private readonly IPlayersTestFixture _testFixture;
		private readonly IAuthFixture _authFixture;
		private readonly Container _container;

		private CreateOld.Request _request;
		private Player _expectedPlayer;

		private Either<Error, Player> _actualResult;

		public CreatePlayerStepsOld(Container container, IAuthFixture authFixture, IPlayersTestFixture testFixture)
		{
			_container = container;
			_authFixture = authFixture;
			_testFixture = testFixture;
		}

		[BeforeScenario(Order = Constants.BEFORE_SCENARIO_STEPS_ORDER)]
		public void ScenarioSetup()
		{
			_authFixture.SetTestUserPermission(PlayersConstants.Name, nameof(CreateOld));
		}

		[Given(@"I have CreateRequest")]
		public void GivenIHaveCreatePlayerRequest(Table table)
		{
			_request = GetPlayer(table);

			var playerId = new PlayerId("player1");
			_testFixture.MockNextRandomId(playerId.ToString());

			_expectedPlayer = new Player(_testFixture.CurrentTenant, playerId, _request.FirstName, _request.LastName);
		}

		[When(@"I execute Create")]
		public async Task WhenIExecuteCreatePlayer()
		{
			var handler = _container.GetInstance<IRequestHandlerOld<CreateOld.Request, Player>>();
			_actualResult = await handler.Handle(_request);
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


		private static CreateOld.Request GetPlayer(Table table)
		{
			var player = table.CreateInstance<CreateOld.Request>();

			player.FirstName = SetNameField(player.FirstName);
			player.LastName = SetNameField(player.LastName);

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