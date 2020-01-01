using System.Collections.Generic;

namespace VolleyM.Domain.Framework.EventBroker
{
    public interface ICanProduceEvent
    {
        List<object> DomainEvents { get; }
    }
}