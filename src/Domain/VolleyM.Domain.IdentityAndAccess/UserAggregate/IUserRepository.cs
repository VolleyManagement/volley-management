using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess
{
	public interface IUserRepository
	{
		EitherAsync<Error, User> Add(User user);

		EitherAsync<Error, User> Get(TenantId tenant, UserId id);

		EitherAsync<Error, Unit> Delete(TenantId tenant, UserId id);
	}
}