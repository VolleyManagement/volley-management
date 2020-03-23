using System.Threading.Tasks;

namespace VolleyM.Domain.Framework.EventBroker
{
    public interface IEventPublisher
    {
        Task PublishEvent<TEvent>(TEvent @event) 
            where TEvent: class;
    }
}