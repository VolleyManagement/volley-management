﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Domain.Framework.EventBroker
{
	[Obsolete]
	public class EventProducerDecoratorOld<TRequest, TResponse>
		: DecoratorBase<IRequestHandlerOld<TRequest, TResponse>>, IRequestHandlerOld<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private readonly IEventPublisher _eventPublisher;

		public EventProducerDecoratorOld(IRequestHandlerOld<TRequest, TResponse> decoratee, IEventPublisher eventPublisher)
			: base(decoratee)
		{
			_eventPublisher = eventPublisher;
		}

		public async Task<Either<Error, TResponse>> Handle(TRequest request)
		{
			var result = await Decoratee.Handle(request);

			if (RootInstance is ICanProduceEvent eventProducer)
			{
				if (eventProducer.DomainEvents == null)
				{
					return Error.DesignViolation("DomainEvents property should be initialized when you inherit ICanProduceEvents interface");
				}

				await PublishAllRegisteredEvents(eventProducer);
			}

			return result;
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