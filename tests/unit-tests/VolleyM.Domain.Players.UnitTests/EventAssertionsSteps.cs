using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SimpleInjector;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Domain.Framework.EventBroker;
using VolleyM.Domain.Players.PlayerAggregate;
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

			object expectedEvent = GetExpectedEvent(table, evt);

			evt.Should().BeEquivalentTo(expectedEvent, cfg=>
			{
				cfg.Using<PlayerId>(ctx => ctx.Subject.Should().NotBeNull());
				return cfg;
			});
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

		private bool PublicEventExpected(string eventScope)
		{
			return string.Compare("public", eventScope, StringComparison.OrdinalIgnoreCase) == 0;
		}
		private static object GetExpectedEvent(Table table, IEvent evt)
		{
			var eventType = evt.GetType();

			var expectedEvent = Activator.CreateInstance(eventType);

			foreach (var propertyInfo in eventType.GetProperties())
			{
				object value = null;
				var rawValue = table.Rows[0][propertyInfo.Name];
				var propType = propertyInfo.PropertyType;

				if (propType == typeof(TenantId))
				{
					value = rawValue == "<default>" 
						? TenantId.Default 
						: new TenantId(rawValue);
				}
				else if (propType == typeof(PlayerId))
				{
					value = new PlayerId(rawValue);
				}

				if (value != null)
				{
					propertyInfo.SetValue(expectedEvent, value);
				}
			}

			table.FillInstance(expectedEvent);

			return expectedEvent;
		}
	}
}