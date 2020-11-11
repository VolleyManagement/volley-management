using System;
using Version = VolleyM.Domain.Contracts.Version;

namespace VolleyM.Domain.UnitTests.Framework.Transforms.Common
{
	public class VersionTransform : ISpecFlowTransform
	{
		private readonly ITestFixture _testFixture;
		private readonly NonMockableVersionMap _versionMap;

		public VersionTransform(ITestFixture testFixture, NonMockableVersionMap versionMap)
		{
			_testFixture = testFixture;
			_versionMap = versionMap;
		}

		public Type TargetType { get; } = typeof(Version);

		public object GetValue(object instance, string rawValue)
		{
			if (string.IsNullOrEmpty(rawValue))
			{
				return null;
			}

			if (TryGetValueFromContext(instance, out var result))
			{
				return result;
			}

			return new Version(rawValue);
		}

		private bool TryGetValueFromContext(object instance, out Version value)
		{
			value = null;
			var key = _testFixture.GetEntityId(instance);

			if (key == null) return false;

			value = _versionMap[key].Actual;

			return value != null;
		}
	}
}