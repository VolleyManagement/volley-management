using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.UnitTests.Framework
{
	public interface ITenantTestFixture : ITestFixture
	{
		TenantId CurrentTenant { get; }
	}
}