using System.Linq;
using NSubstitute;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Players.Handlers;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests.Fixture
{
	public abstract class PlayersTestFixtureBase : TenantTestFixtureBase
	{
		private readonly IRandomIdGenerator _idGenerator;

		protected PlayersTestFixtureBase()
		{
			_idGenerator = Substitute.For<IRandomIdGenerator>();
		}

		public override void RegisterScenarioDependencies(Container container)
		{
			base.RegisterScenarioDependencies(container);

			container.RegisterInstance(_idGenerator);
		}

		public override EntityId GetEntityId(object instance)
		{
			return instance switch
			{
				Player p => GetIdForPlayer(p.Tenant, p.Id),
				PlayerDto dto => GetIdForPlayer(dto.Tenant, dto.Id),
				TestPlayerDto test => GetIdForPlayer(base.CurrentTenant, test.Id),
				_ => null
			};
		}

		public void MockNextRandomId(string id)
		{
			_idGenerator.GetRandomId().Returns(id);
		}

		public void SetupPlayerName(IPlayerNameRequest request)
		{
			request.FirstName = SetNameField(request.FirstName);
			request.LastName = SetNameField(request.LastName);
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

		private EntityId GetIdForPlayer(TenantId tenantId, PlayerId playerId)
		{
			return new EntityId($"{tenantId}|{playerId}");
		}
	}
}