using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SimpleInjector;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Domain.Framework.EventBroker;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests
{
	[Binding]
	public class EventAssertionsSteps
	{
		private readonly Container _container;

		public EventAssertionsSteps(Container container)
		{
			_container = container;
		}

		[Then(@"(.*) event is produced")]
		public void ThenPlayerCreatedEventIsProduced(string eventName, Table table)
		{
			var evt = GetReceivedEvent(eventName);

			evt.Should().NotBeNull();
		}

		[Then(@"(.*) event is (.*)")]
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

		private bool PublicEventExpected(string eventScope)
		{
			return string.Compare("public", eventScope, StringComparison.OrdinalIgnoreCase) == 0;
		}
	}
}