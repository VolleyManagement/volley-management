using System;
using System.Collections.Generic;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.UnitTests.Framework;
using TechTalk.SpecFlow.Assist;

namespace VolleyM.Domain.Players.UnitTests
{
	public class PlayerIdTransform : ISpecFlowTransform, IValueRetriever
	{
		public Type TargetType { get; } = typeof(PlayerId);
		public object GetValue(string rawValue)
		{
			return new PlayerId(rawValue);
		}

		public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
		{
			return propertyType == typeof(PlayerId);
		}

		public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
		{
			if (string.IsNullOrEmpty(keyValuePair.Value))
			{
				return null;
			}

			return GetValue(keyValuePair.Value);
		}
	}
}