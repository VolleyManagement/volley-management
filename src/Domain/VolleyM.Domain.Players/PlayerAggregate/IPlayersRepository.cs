using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Players.PlayerAggregate
{
	public interface IPlayersRepository
	{
		EitherAsync<Error, Player> Get(TenantId tenant, PlayerId id);

		EitherAsync<Error, Player> Add(Player player);

		/// <summary>
		///Persists changes to the Player
		/// </summary>
		/// <param name="player">Instance to persist</param>
		/// <param name="lastKnownEntityVersion">Last known entity version to do concurrency check</param>
		/// <returns></returns>
		EitherAsync<Error, Player> Update(Player player, Version lastKnownEntityVersion);

		EitherAsync<Error, Unit> Delete(TenantId tenant, PlayerId id);
	}
}