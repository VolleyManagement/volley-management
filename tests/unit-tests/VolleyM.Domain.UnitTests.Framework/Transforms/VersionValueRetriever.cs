using System;
using System.Collections.Generic;
using TechTalk.SpecFlow.Assist;
using Version = VolleyM.Domain.Contracts.Version;

namespace VolleyM.Domain.UnitTests.Framework
{
	public class VersionValueRetriever:IValueRetriever
	{
		public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
		{
			return propertyType == typeof(Version);
		}

		public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
		{
			if (string.IsNullOrEmpty(keyValuePair.Value))
			{
				return null;
			}

			return GetValue(keyValuePair.Value);
		}

		public Version GetValue(string rawValue)
		{
			return new Version(rawValue);
		}
	}
}