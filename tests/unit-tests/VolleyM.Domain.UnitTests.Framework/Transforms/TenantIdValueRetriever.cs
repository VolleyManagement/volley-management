using System;
using System.Collections.Generic;
using TechTalk.SpecFlow.Assist;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.UnitTests.Framework
{
	public class TenantIdValueRetriever : IValueRetriever
	{
		private readonly Func<TenantId> _currentTenantProvider;

		public TenantIdValueRetriever(Func<TenantId> currentTenantProvider)
		{
			_currentTenantProvider = currentTenantProvider;
		}

		public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
		{
			return propertyType == typeof(TenantId);
		}

		public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
		{
			if (string.IsNullOrEmpty(keyValuePair.Value))
			{
				return null;
			}

			var rawValue = keyValuePair.Value;

			return rawValue == "<default>"
				? _currentTenantProvider()
				: new TenantId(rawValue);
		}
	}
}