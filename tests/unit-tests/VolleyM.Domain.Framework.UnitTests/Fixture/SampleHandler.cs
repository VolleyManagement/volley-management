﻿using System.Threading.Tasks;
using FluentValidation;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.Framework.UnitTests.Fixture
{
    [Permission("DomainFrameworkTests", "SampleHandler")]
    public class SampleHandler
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
                RuleFor(r => r.A).IsDefault();
            }
        }

        public class Handler : IRequestHandler<Request, Unit>
        {
            public Task<Either<Error, Unit>> Handle(Request request)
            {
                return Task.FromResult<Either<Error, Unit>>(Unit.Default);
            }
        }
    }
}