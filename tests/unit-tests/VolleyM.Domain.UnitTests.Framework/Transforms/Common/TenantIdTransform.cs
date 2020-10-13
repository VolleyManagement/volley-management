using System;
using Serilog;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.UnitTests.Framework.Common
{
	public class TenantIdTransform : ISpecFlowTransform
	{
		private readonly Func<TenantId> _currentTenantProvider;

		public TenantIdTransform(Func<TenantId> currentTenantProvider)
		{
			_currentTenantProvider = currentTenantProvider;
		}

		public Type TargetType { get; } = typeof(TenantId);

		public object GetValue(string rawValue)
		{
			if (string.IsNullOrEmpty(rawValue))
			{
				return null;
			}

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
				Log.Error(e, "Failed to get Current tenant");
				throw;
			}
		}
	}
}