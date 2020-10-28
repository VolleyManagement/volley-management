using System;

namespace VolleyM.Domain.UnitTests.Framework
{
	internal class NoOpSpecFlowTransform : ISpecFlowTransform
	{
		public Type TargetType { get; } = typeof(object);

		public object GetValue(object instance, string rawValue)
		{
			return null;
		}
	}
}