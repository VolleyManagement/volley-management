using System.Collections.Generic;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Domain.Framework.EventBroker
{
    public interface ICanProduceEvent
    {
        List<IEvent> DomainEvents { get; }
    }
}