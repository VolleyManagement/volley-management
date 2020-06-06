using System;

namespace VolleyM.Domain.UnitTests.Framework
{
	/// <summary>
	/// SpecFlow does not do a good job in transforms for regular tables so this interface is a way to fix that.
	/// </summary>
	public interface ISpecFlowTransformFactory
	{
		ISpecFlowTransform GetTransform(Type targetType);

		void RegisterTransform(ISpecFlowTransform transform);
	}
}