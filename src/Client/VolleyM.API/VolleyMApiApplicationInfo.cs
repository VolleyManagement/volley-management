using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.API
{
	public class VolleyMApiApplicationInfo : IApplicationInfo
	{
		public VolleyMApiApplicationInfo(bool isRunningInProduction)
		{
			IsRunningInProduction = isRunningInProduction;
		}

		public bool IsRunningInProduction { get; }
	}
}