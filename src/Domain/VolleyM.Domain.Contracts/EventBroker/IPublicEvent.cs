namespace VolleyM.Domain.Contracts.EventBroker
{
    /// <summary>
    /// arks event which should be routed outside of the bounded context
    /// </summary>
    public interface IPublicEvent : IEvent
    {

    }
}