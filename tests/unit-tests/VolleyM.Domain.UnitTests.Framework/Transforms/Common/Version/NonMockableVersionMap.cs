using System.Collections.Generic;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.UnitTests.Framework.Transforms.Common
{
	/// <summary>
	/// In integration tests where we cannot control Version creation we use this class to record all versions created for later use
	/// </summary>
	public class NonMockableVersionMap
	{
		private readonly Dictionary<EntityId, Version> _map;

		public NonMockableVersionMap(Dictionary<EntityId, Version> map)
		{
			_map = map;
		}

		public Version this[EntityId key]
		{
			get
			{
				if (_map.TryGetValue(key, out var result))
				{
					return result;
				}

				return null;
			}
			set
			{
				_map[key] = value;
			}
		}
	}
}