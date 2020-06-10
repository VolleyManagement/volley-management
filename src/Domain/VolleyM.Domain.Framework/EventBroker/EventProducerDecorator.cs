using LanguageExt;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.EventBroker;

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

		public EitherAsync<Error, TResponse> Handle(TRequest request)
		{
			var result = Decoratee.Handle(request);

			return result.Match<Either<Error, TResponse>>(
					Right: r =>
					{
						if (RootInstance is ICanProduceEvent eventProducer)
						{
							if (eventProducer.DomainEvents == null)
							{
								return Error.DesignViolation(
									"DomainEvents property should be initialized when you inherit ICanProduceEvents interface");
							}

							PublishAllRegisteredEvents(eventProducer)
								.Wait();
						}

						return r;
					},
					e => e)
				.ToAsync();
		}

		private async Task PublishAllRegisteredEvents(ICanProduceEvent eventProducer)
		{
			var pubTasks = new List<Task>();
			foreach (IEvent domainEvent in eventProducer.DomainEvents)
			{
				pubTasks.Add(_eventPublisher.PublishEvent(domainEvent));
			}

			await Task.WhenAll(pubTasks);
		}
	}
}