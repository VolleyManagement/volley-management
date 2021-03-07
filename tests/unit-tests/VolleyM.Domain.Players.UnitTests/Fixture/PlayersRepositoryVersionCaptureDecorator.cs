using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Domain.UnitTests.Framework.Transforms.Common;

namespace VolleyM.Domain.Players.UnitTests.Fixture
{
	public class PlayersRepositoryVersionCaptureDecorator : IPlayersRepository

	{
		private readonly IPlayersRepository _decoratee;
		private readonly NonMockableVersionMap _versionMap;

		public PlayersRepositoryVersionCaptureDecorator(IPlayersRepository decoratee, NonMockableVersionMap versionMap)
		{
			_decoratee = decoratee;
			_versionMap = versionMap;
		}

		public EitherAsync<Error, Player> Get(TenantId tenant, PlayerId id)
		{
			return _decoratee.Get(tenant, id);
		}

		public EitherAsync<Error, Player> Add(Player player)
		{
			return _decoratee.Add(player)
				.Do(p =>
				{
					player.ToString();
					_versionMap.RecordVersionChange(GetEntityId(p), p.Version);
				});
		}

		public EitherAsync<Error, Player> Update(Player player, Version lastKnownEntityVersion)
		{
			return _decoratee.Update(player, lastKnownEntityVersion)
				.Do(p => { _versionMap.RecordVersionChange(GetEntityId(p), p.Version); });
		}

		public EitherAsync<Error, Unit> Delete(TenantId tenant, PlayerId id)
		{
			return _decoratee.Delete(tenant, id)
				.Do(_ => { _versionMap.RecordEndVersion(GetEntityId(tenant, id));});
		}

		private static EntityId GetEntityId(Player p)
		{
			return GetEntityId(p.Tenant, p.Id);
		}

		private static EntityId GetEntityId(TenantId tenant, PlayerId playerId)
		{
			return PlayersTestFixtureBase.GetIdForPlayer(tenant, playerId);
		}
	}
}