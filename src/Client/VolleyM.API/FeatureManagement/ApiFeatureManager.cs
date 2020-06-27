using System.Threading;
using System.Threading.Tasks;
using Esquio.Abstractions;
using SimpleInjector;
using VolleyM.Domain.Contracts.FeatureManagement;

namespace VolleyM.API.FeatureManagement
{
	public class ApiFeatureManager : IFeatureManager
	{
		private readonly Container _container;

		public ApiFeatureManager(Container container)
		{
			_container = container;
		}

		public Task<bool> IsEnabledAsync(string featureName, string contextName,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			// This is a workaround
			// When SimpleInjector runs Verify method it creates all instances of the services to resolve the tree.
			// Esquio depends on IHttpContextAccessor which is not available outside of active request scope
			// Verify() methods runs at a startup so there is a clash :(
			var featureService = _container.GetInstance<IFeatureService>();

			return featureService.IsEnabledAsync($"{contextName}::{featureName}", cancellationToken);
		}
	}
}