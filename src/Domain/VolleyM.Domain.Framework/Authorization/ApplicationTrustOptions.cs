namespace VolleyM.Domain.Framework.Authorization
{
	/// <summary>
	/// Specifies options for configuring special trust permissions.
	/// </summary>
	public class ApplicationTrustOptions
	{
		public static string ConfigKey = "ApplicationTrust";

		public string Auth0ClientId { get; set; }
	}
}