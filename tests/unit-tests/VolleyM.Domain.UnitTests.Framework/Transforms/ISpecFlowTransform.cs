
using System;

namespace VolleyM.Domain.UnitTests.Framework
{
	[Obsolete]
	public interface ISpecFlowTransform
	{
		Type TargetType { get; }

		object GetValue(string rawValue);
	}
}