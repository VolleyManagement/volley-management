using System.Threading.Tasks;

namespace VolleyM.Domain.Framework.EventBus
{
    public interface IEventPublisher
    {
        Task PublishEvent<TEvent>(TEvent @event) 
            where TEvent: class;
    }
}