using System;
using System.Collections.Generic;
using System.Threading;
using Serilog;
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
			Log.Warning("TenantIdRetriever.Retrieve called. Thread: {ThreadId}", Thread.CurrentThread.ManagedThreadId);
			if (string.IsNullOrEmpty(keyValuePair.Value))
			{
				return null;
			}

			var rawValue = keyValuePair.Value;

			return rawValue == "<default>"
				? GetCurrentTenant()
				: new TenantId(rawValue);
		}

		private TenantId GetCurrentTenant()
		{
			try
			{
				return _currentTenantProvider();
			}
			catch (Exception e)
			{
				Log.Error(e, "Failed to get Current tenant; {ThreadId}", Thread.CurrentThread.ManagedThreadId);
				throw;
			}
		}
	}
}