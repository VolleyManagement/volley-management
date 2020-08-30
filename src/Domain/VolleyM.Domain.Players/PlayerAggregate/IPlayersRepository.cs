using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Players.PlayerAggregate
{
	public interface IPlayersRepository
	{
		EitherAsync<Error, Player> Get(TenantId tenant, PlayerId id);

		EitherAsync<Error, Player> Add(Player player);

		EitherAsync<Error, Unit> Delete(TenantId tenant, PlayerId id);
	}
}