using SimpleInjector;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests
{
	/// <summary>
	/// SpecFlow is not great at reusing steps across assemblies.
	/// By having this class we make it think that it belongs to this assembly
	/// </summary>
	public class EventAssertionStepsHook:EventAssertionsSteps
	{
		public EventAssertionStepsHook(Container container, ISpecFlowTransformFactory transformFactory) 
			: base(container, transformFactory)
		{
		}
	}
}