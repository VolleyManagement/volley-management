using System;
using System.Collections.Concurrent;

namespace VolleyM.Domain.Framework.UnitTests.EventBus.Fixture
{
    public class EventListenerTestSpy
    {
        private readonly ConcurrentDictionary<Type, int> _invocations = new ConcurrentDictionary<Type, int>();

        public void RegisterInvocation<T>()
        {
            _invocations.AddOrUpdate(typeof(T), 1, (t, currentVal) => currentVal + 1);
        }

        public int GetInvocations<T>()
        {
            if (_invocations.TryGetValue(typeof(T), out int result))
            {
                return result;
            }

            return 0;
        }
    }
}