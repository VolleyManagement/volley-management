using LanguageExt;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IDomainFrameworkTestFixture
{
    public class NoValidationHandlerOld
    {
        public class Request : IRequest<Unit>
        {
            public int A { get; set; }

            public int B { get; set; }

        }

        public class Handler : IRequestHandlerOld<Request, Unit>
        {
            public Task<Either<Error, Unit>> Handle(Request request)
            {
                return Task.FromResult<Either<Error, Unit>>(Unit.Default);
            }
        }
    }
}