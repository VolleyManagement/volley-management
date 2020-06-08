using System.Threading.Tasks;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.UnitTests.Framework
{
	public abstract class TenantTestFixtureBase : ITenantTestFixture
	{
		protected Container _container;

		public virtual void RegisterScenarioDependencies(Container container)
		{
			_container = container;
		}

		public TenantId CurrentTenant
		{
			get
			{
				return _container.GetInstance<ICurrentUserProvider>().Tenant;
			}
		}

		public abstract Task ScenarioSetup();

		public abstract Task ScenarioTearDown();
	}
}