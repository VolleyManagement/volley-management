using System;
using System.Collections.Generic;
using TechTalk.SpecFlow.Assist;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players.UnitTests
{
	public class PlayerIdValueRetriever : IValueRetriever
	{
		public PlayerIdValueRetriever()
		{
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

			return new PlayerId(keyValuePair.Value);
		}
	}
}