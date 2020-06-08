using System;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.UnitTests.Framework
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
			return rawValue == "<default>"
				? _currentTenantProvider()
				: new TenantId(rawValue);
		}
	}
}