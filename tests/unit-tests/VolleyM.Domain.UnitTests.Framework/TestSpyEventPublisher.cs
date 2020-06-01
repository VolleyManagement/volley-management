using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Domain.Framework.EventBroker;

namespace VolleyM.Domain.UnitTests.Framework
{
	public class TestSpyEventPublisher : IEventPublisher
	{
		public List<IEvent> ReceivedEvents { get; } = new List<IEvent>();

		public Task PublishEvent<TEvent>(TEvent @event) where TEvent : IEvent
		{
			ReceivedEvents.Add(@event);
			return Task.CompletedTask;
		}
	}
}