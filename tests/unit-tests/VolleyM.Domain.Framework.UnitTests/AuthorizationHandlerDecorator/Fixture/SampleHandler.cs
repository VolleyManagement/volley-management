using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.Framework.UnitTests.AuthorizationHandlerDecorator.Fixture
{
    [Permission("DomainFrameworkTests", "SampleHandler")]
    public class SampleHandler
    {
        public class Request : IRequest<Unit>
        {
            public int A { get; set; }

            public int B { get; set; }

        }

        public class Handler : IRequestHandler<Request, Unit>
        {
            public Task<Result<Unit>> Handle(Request request)
            {
                return Task.FromResult<Result<Unit>>(Unit.Value);
            }
        }
    }
}