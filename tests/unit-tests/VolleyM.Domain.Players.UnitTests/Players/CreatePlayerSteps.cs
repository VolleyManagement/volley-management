using System.Threading.Tasks;
using FluentAssertions;
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
	[Scope(Feature = "Create Player")]
	public class CreatePlayerSteps
	{
		private readonly IPlayersTestFixture _testFixture;
		private readonly IAuthFixture _authFixture;
		private readonly Container _container;

		private CreatePlayer.Request _request;
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
			_authFixture.SetTestUserPermission(PlayersConstants.Name, nameof(CreatePlayer));
		}

		[Given(@"I have CreatePlayerRequest")]
		public void GivenIHaveCreatePlayerRequest(Table table)
		{
			_request = GetPlayer(table);

			_expectedPlayer = new Player(_request.Tenant, _request.Id, _request.FirstName, _request.LastName);
		}

		[When(@"I execute CreatePlayer")]
		public async Task WhenIExecuteCreatePlayer()
		{
			var handler = _container.GetInstance<IRequestHandler<CreatePlayer.Request, Player>>();
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
			_actualResult.IsRight.Should().BeTrue("created player should be returned");
			_actualResult.IfRight(u => u.Should()
				.BeEquivalentTo(_expectedPlayer, "created player should be returned"));
		}

		private static CreatePlayer.Request GetPlayer(Table table)
		{
			var player = table.CreateInstance<CreatePlayer.Request>();

			player.Tenant = new TenantId(table.Rows[0][nameof(Player.Tenant)]);
			player.Id = new PlayerId(table.Rows[0][nameof(Player.Id)]);

			return player;
		}
	}
}
