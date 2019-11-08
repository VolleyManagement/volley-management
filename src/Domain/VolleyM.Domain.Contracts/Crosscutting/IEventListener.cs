using System;
using System.Threading.Tasks;

namespace VolleyM.Domain.Contracts.Crosscutting
{
    /// <summary>
    /// Subscribes and listens to events
    /// </summary>
    /// <typeparam name="TEvent">Type of event to listen</typeparam>
    public interface IEventListener<in TEvent> : IEventListener
        where TEvent : class
    {
        Task Handle(TEvent @event);

        Type IEventListener.EventType => typeof(TEvent);

        Task IEventListener.HandleCommon(object @event) => Handle((TEvent)@event);
    }

    /// <summary>
    /// Non-generic interface used to hook up listeners
    /// </summary>
    public interface IEventListener
    {
        Type EventType { get; }

        Task HandleCommon(object @event);
    }
}