using VolleyM.Domain.Contracts;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
	public class IdentityAndAccessFixtureBase
	{
		public EntityId GetEntityId(object instance)
		{
			return instance switch
			{
				User u => GetIdForUser(u.Tenant, u.Id),
				_ => null
			};
		}

		private static EntityId GetIdForUser(TenantId tenantId, UserId userId)
		{
			return new EntityId($"{tenantId}|{userId}");
		}
	}
}