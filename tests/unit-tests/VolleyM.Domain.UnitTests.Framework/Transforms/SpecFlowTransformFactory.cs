using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyM.Domain.UnitTests.Framework
{
	internal class SpecFlowTransformFactory : ISpecFlowTransformFactory
	{
		private readonly Dictionary<Type, ISpecFlowTransform> _registry;

		private readonly ISpecFlowTransform _empty = new NoOpSpecFlowTransform();

		public SpecFlowTransformFactory(IEnumerable<ISpecFlowTransform> transforms)
		{
			_registry = transforms.ToDictionary(t => t.TargetType, t => t);
		}

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
			// do nothing
		}
	}
}