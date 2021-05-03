using System;

namespace VolleyM.Domain.UnitTests.Framework
{
	public interface ISpecFlowTransform
	{
		Type TargetType { get; }

		object GetValue(object instance, string rawValue);
	}
}