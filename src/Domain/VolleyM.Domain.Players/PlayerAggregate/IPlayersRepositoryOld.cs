using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Players.PlayerAggregate
{
	public interface IPlayersRepositoryOld
	{
		Task<Either<Error, Player>> Get(TenantId tenant, PlayerId id);

		Task<Either<Error, Player>> Add(Player player);

		Task<Either<Error, Unit>> Delete(TenantId tenant, PlayerId id);
	}
}