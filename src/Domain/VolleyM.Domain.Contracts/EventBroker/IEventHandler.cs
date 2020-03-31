using System.Threading.Tasks;

namespace VolleyM.Domain.Contracts.EventBroker
{
    public interface IEventHandler<in T> : IEventHandler
        where T : IEvent
    {
        Task Handle(T @event);
    }

    /// <summary>
    /// Represents non generic event handler to simplify event dispatching
    /// </summary>
    public interface IEventHandler
    {
    }
}