﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.EventBroker;

namespace VolleyM.Domain.IDomainFrameworkTestFixture
{
	public class SampleHandlerOld
    {
        public class Request : IRequest<Unit>
        {
            public int A { get; set; }

            public int B { get; set; }

        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(r => r.A).Equal(0);
            }
        }

        public class Handler : IRequestHandlerOld<Request, Unit>, ICanProduceEvent
        {
            public Task<Either<Error, Unit>> Handle(Request request)
            {
                DomainEvents.Add(new SampleEvent { Data = "SampleHandlerOld invoked" });
                return Task.FromResult<Either<Error, Unit>>(Unit.Default);
            }

            public List<IEvent> DomainEvents { get; } = new List<IEvent>();
        }
    }
}