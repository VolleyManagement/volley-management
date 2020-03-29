using System.Threading.Tasks;

namespace VolleyM.Domain.Contracts.EventBroker
{
    public interface IEventHandler<in T> where T: IEvent
    {
        Task Handle(T @event);
    }
}