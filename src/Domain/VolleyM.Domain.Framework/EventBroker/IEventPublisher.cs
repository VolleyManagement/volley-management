using System.Threading.Tasks;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Domain.Framework.EventBroker
{
    public interface IEventPublisher
    {
        Task PublishEvent<TEvent>(TEvent @event) 
            where TEvent: IEvent;
    }
}