namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture
{
    /// <summary>
    /// Simplifies test setup for event generating handlers. See usage for more details
    /// </summary>
    public interface IEventProducingRequest
    {
        public int EventData { get; set; }
    }
}