using System;
using System.Collections.Generic;

namespace VolleyM.Domain.UnitTests.Framework
{
	internal class SpecFlowTransformFactory : ISpecFlowTransformFactory
	{
		private readonly Dictionary<Type, ISpecFlowTransform> _registry = new Dictionary<Type, ISpecFlowTransform>();

		private readonly ISpecFlowTransform _empty = new NoOpSpecFlowTransform();

		public ISpecFlowTransform GetTransform(Type targetType)
		{
			if (!_registry.TryGetValue(targetType, out var result))
			{
				result = _empty;
			}

			return result;
		}

		public void RegisterTransform(ISpecFlowTransform transform)
		{
			_registry[transform.TargetType] = transform;
		}
	}
}