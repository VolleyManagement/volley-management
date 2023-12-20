using System.Collections.Generic;
using LanguageExt;
using Version = VolleyM.Domain.Contracts.Version;

namespace VolleyM.Domain.UnitTests.Framework.Transforms.Common
{
	/// <summary>
	/// In integration tests where we cannot control Version creation we use this class to record all versions created for later use
	/// </summary>
	public class NonMockableVersionMap
	{
		private readonly Dictionary<EntityId, List<Version>> _log = new();
		
		private readonly Dictionary<Version, (Version CreatedVersion, EntityId EntityId)> _createdVersionMap =
			new();

		public IReadOnlyList<Version> GetVersionLog(EntityId key)
		{
			if (_log.TryGetValue(key, out var result))
			{
				return result.AsReadOnly();
			}

			return new List<Version>();
		}

		public void RecordVersionChange(EntityId key, Version newVersion)
		{
			var log = GetCurrentVersionLog(key);
			log.Add(newVersion);
		}

		public void RecordEndVersion(EntityId key)
		{
			var log = GetCurrentVersionLog(key);
			log.Add(Version.Deleted);
		}

		private List<Version> GetCurrentVersionLog(EntityId key)
		{
			if (_log.TryGetValue(key, out var result))
			{
				return result;
			}

			result = new List<Version> {Version.Initial};
			_log[key] = result;
			return result;
		}

		public void AssociateTestVersions(EntityId key, Version createdVersion, Version testVersion)
		{
			_createdVersionMap[testVersion] = (createdVersion, key);
		}

		public Option<(Version CreatedVersion, EntityId EntityId)> GetFromTestVersion(Version testVersion)
		{
			if (_createdVersionMap.TryGetValue(testVersion, out var result))
			{
				return result;
			}

			return Option<(Version CreatedVersion, EntityId EntityId)>.None;
		}
	}
}