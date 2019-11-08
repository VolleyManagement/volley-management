namespace VolleyM.Domain.Framework.EventBus
{
    public interface IEventBus
    {
        void PublishEvent<TEvent>(TEvent @event);
    }
}