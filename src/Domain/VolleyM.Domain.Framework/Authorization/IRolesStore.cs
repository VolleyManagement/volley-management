using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.Framework.Authorization
{
	public interface IRolesStore
	{
		EitherAsync<Error, Role> Get(RoleId roleId);
	}
}