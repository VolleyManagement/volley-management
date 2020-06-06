using System.Collections.Generic;

namespace VolleyM.Domain.Contracts.EventBroker
{
    public interface ICanProduceEvent
    {
        List<IEvent> DomainEvents { get; }
    }
}