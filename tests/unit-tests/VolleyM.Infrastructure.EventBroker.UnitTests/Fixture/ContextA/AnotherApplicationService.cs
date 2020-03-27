﻿using System.Threading.Tasks;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture.ContextA
{
    public class AnotherApplicationService : IEventHandler<EventC>
    {
        private readonly EventInvocationSpy _eventSpy;

        public AnotherApplicationService(EventInvocationSpy eventSpy)
        {
            _eventSpy = eventSpy;
        }

        public Task Handle(EventC @event)
        {
            return RegisterEvent(@event);
        }

        private Task RegisterEvent(object @event)
        {
            _eventSpy.RegisterInvocation(@event);
            return Task.CompletedTask;
        }
    }
}