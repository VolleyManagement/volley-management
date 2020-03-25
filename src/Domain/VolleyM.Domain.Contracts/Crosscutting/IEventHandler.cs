using System.Threading.Tasks;

namespace VolleyM.Domain.Contracts.Crosscutting
{
    public interface IEventHandler<T>
    {
        Task Handle(T @event);
    }
}