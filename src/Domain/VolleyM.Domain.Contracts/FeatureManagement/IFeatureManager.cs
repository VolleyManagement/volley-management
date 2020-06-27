using System.Threading;
using System.Threading.Tasks;

namespace VolleyM.Domain.Contracts.FeatureManagement
{
	public interface IFeatureManager
	{
		Task<bool> IsEnabledAsync(
			string featureName,
			string contextName,
			CancellationToken cancellationToken = default(CancellationToken));
	}
}