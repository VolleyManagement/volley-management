using System;
using System.Linq;
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

			if (TryGetValueFromContext(instance, rawValue, out var result))
			{
				return result;
			}

			return new Version(rawValue);
		}

		private bool TryGetValueFromContext(object instance, string rawValue, out Version value)
		{
			value = null;
			var key = _testFixture.GetEntityId(instance);

			if (key == null) return false;

			var log = _versionMap.GetVersionLog(key);

			value = log.LastOrDefault();
			if (value == null)
			{
				return false;
			}

			var tv=_versionMap.GetTestVersion(value);
			if (key != tv.entityId)
			{
				value = null;
			}

			if (tv.testVersion.ToString() != rawValue)
			{
				value = null;
			}

			return value != null;
		}
	}
}