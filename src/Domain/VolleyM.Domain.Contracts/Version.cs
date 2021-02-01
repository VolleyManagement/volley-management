namespace VolleyM.Domain.Contracts
{
	public class Version : ImmutableBase<string>
	{
		public Version(string value) : base(value)
		{
		}

		public static Version Initial = new Version("initial");
		// Object was deleted
		public static Version Deleted = new Version("deleted");
	}
}