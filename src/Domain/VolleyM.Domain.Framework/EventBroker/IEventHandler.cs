using System.Threading.Tasks;

namespace VolleyM.Domain.Framework.EventBus
{
    public interface IEventHandler<T>
    {
        Task Handle(T @event);
    }
}