using System;
using Serilog;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.UnitTests.Framework.Common
{
	public class TenantIdTransform : ISpecFlowTransform
	{
		private readonly ICurrentUserProvider _currentUserProvider;

		public TenantIdTransform(ICurrentUserProvider currentUserProvider)
		{
			_currentUserProvider = currentUserProvider;
		}

		public Type TargetType { get; } = typeof(TenantId);

		public object GetValue(object instance, string rawValue)
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
				return _currentUserProvider.Tenant;
			}
			catch (Exception e)
			{
				Log.Error(e, "Failed to get Current tenant");
				throw;
			}
		}
	}
}