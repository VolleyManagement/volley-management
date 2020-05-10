namespace VolleyM.Domain.Contracts.Crosscutting
{

	/// <summary>
	/// Provides information about the overall application
	/// </summary>
	public interface IApplicationInfo
	{
		bool IsRunningInProduction { get; }
	}
}