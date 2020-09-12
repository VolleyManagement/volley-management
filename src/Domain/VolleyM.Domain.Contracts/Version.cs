namespace VolleyM.Domain.Contracts
{
	public class Version : ImmutableBase<string>
	{
		public Version(string value) : base(value)
		{
		}
	}
}