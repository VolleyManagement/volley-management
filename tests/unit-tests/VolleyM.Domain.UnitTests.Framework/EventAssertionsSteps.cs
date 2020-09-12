using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SimpleInjector;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Domain.Framework.EventBroker;

namespace VolleyM.Domain.UnitTests.Framework
{
	[Binding]
	public abstract class EventAssertionsSteps
	{
		private readonly Container _container;

		protected EventAssertionsSteps(Container container)
		{
			_container = container;
		}

		[Then(@"(.*) event is produced")]
		public void ThenPlayerCreatedEventIsProduced(string eventName, Table table)
		{
			var evt = GetReceivedEvent(eventName);

			evt.Should().NotBeNull();

			object expectedEvent = GetExpectedEvent(table, evt);

			evt.Should().BeEquivalentTo(expectedEvent);
		}

		[Then(@"(.*) event is (Public|Internal)")]
		public void ThenPlayerCreatedEventHasProperScope(string eventName, string eventScope)
		{
			var evt = GetReceivedEvent(eventName);

			evt.Should().NotBeNull();

			if (PublicEventExpected(eventScope))
			{
				evt.Should().BeAssignableTo<IPublicEvent>();
			}
			else
			{
				evt.Should().NotBeAssignableTo<IPublicEvent>();
			}
		}

		[Then(@"(.*) event is not produced")]
		public void ThenPlayerCreatedEventIsNotProduced(string eventName)
		{
			var evt = GetReceivedEvent(eventName);

			evt.Should().BeNull();
		}

		private IEvent GetReceivedEvent(string eventName)
		{
			List<IEvent> publishedEvents = null;

			var eventPublisher = _container.GetInstance<IEventPublisher>();
			if (eventPublisher is TestSpyEventPublisher spyEventPublisher)
			{
				publishedEvents = spyEventPublisher.ReceivedEvents;
			}

			var target = publishedEvents?.FirstOrDefault(
				e => string.CompareOrdinal(e.GetType().Name, eventName) == 0);

			return target;
		}

		private static bool PublicEventExpected(string eventScope)
		{
			return string.Compare("public", eventScope, StringComparison.OrdinalIgnoreCase) == 0;
		}

		private object GetExpectedEvent(Table table, IEvent evt)
		{
			var eventType = evt.GetType();

			var expectedEvent = Activator.CreateInstance(eventType);

			table.FillInstance(expectedEvent);

			return expectedEvent;
		}
	}
}