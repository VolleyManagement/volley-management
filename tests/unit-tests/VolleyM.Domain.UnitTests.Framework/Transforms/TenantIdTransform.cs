using System;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.UnitTests.Framework
{
	public class TenantIdTransform : ISpecFlowTransform
	{
		public Type TargetType { get; } = typeof(TenantId);

		public object GetValue(string rawValue)
		{
			return rawValue == "<default>"
				? TenantId.Default
				: new TenantId(rawValue);
		}
	}
}