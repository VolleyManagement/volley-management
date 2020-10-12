using System;

namespace VolleyM.Domain.UnitTests.Framework.Transforms.Common
{
	public class VersionTransform:ISpecFlowTransform
	{
		public Type TargetType { get; } = typeof(Version);

		public object GetValue(string rawValue)
		{
			if (string.IsNullOrEmpty(rawValue))
			{
				return null;
			}

			return new Version(rawValue);
		}
	}
}