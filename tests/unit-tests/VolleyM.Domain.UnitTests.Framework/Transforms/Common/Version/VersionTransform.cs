using System;
using System.Collections.Generic;
using LanguageExt;
using LanguageExt.SomeHelp;
using LanguageExt.UnsafeValueAccess;
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
			var testVersion = new Version(rawValue);

			var key = GetEntityKey(instance);

			var associatedTestVersion = key.Bind(entityId =>
			  {
				  return _versionMap.GetFromTestVersion(testVersion)
					  .Bind(tv =>
					  {
						  (Version createdVersion, EntityId changedEntityId) = tv;
						  if (changedEntityId == entityId)
						  {
							  return Option<Version>.Some(createdVersion);
						  }

						  return Option<Version>.None;
					  });
			  });

			var result = associatedTestVersion
				.BiBind(v => v.ToSome(),
				() =>
				{
					return key.Map(eId => _versionMap.GetVersionLog(eId))
						.Bind(versionLog => GetVersionByConvention(rawValue, versionLog));
				});


			if (result.IsSome)
			{
				value = result.ValueUnsafe();
			}
			return result.IsSome;
		}

		private Option<EntityId> GetEntityKey(object instance)
		{
			var key = _testFixture.GetEntityId(instance);

			return key == null ? Option<EntityId>.None : key;
		}

		private static Option<Version> GetVersionByConvention(string rawValue, IReadOnlyList<Version> changeLog)
		{
			const string VERSION = "version";

			// for readability we put version as "version1", "version2". Index starts at 1.
			if (rawValue.StartsWith(VERSION))
			{
				var indexStr = rawValue[VERSION.Length..];
				if (byte.TryParse(indexStr, out var index))
				{
					if (changeLog.Count >= index)
					{
						// there is always 'initial' version
						return changeLog[index];
					}
				}
			}
			return Option<Version>.None;
		}
	}
}