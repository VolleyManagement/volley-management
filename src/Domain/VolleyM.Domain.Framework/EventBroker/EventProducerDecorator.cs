using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.EventBus;

namespace VolleyM.Domain.Framework.EventBroker
{
    public class EventProducerDecorator<TRequest, TResponse>
        : DecoratorBase<IRequestHandler<TRequest, TResponse>>, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEventPublisher _eventPublisher;

        public EventProducerDecorator(IRequestHandler<TRequest, TResponse> decoratee, IEventPublisher eventPublisher)
            : base(decoratee)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task<Either<Error, TResponse>> Handle(TRequest request)
        {
            var result = await Decoratee.Handle(request);

            if (RootInstance is ICanProduceEvent eventProducer)
            {
                var pubTasks = new List<Task>();
                foreach (object domainEvent in eventProducer.DomainEvents)
                {
                    pubTasks.Add(_eventPublisher.PublishEvent(domainEvent));
                }

                await Task.WhenAll(pubTasks);
            }

            return result;
        }
    }
}