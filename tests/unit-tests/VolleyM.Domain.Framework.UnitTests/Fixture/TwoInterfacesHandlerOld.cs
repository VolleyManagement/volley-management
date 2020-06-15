using LanguageExt;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.UnitTests.Fixture
{
    public class TwoInterfacesHandlerOld
    {
        public class Request : IRequest<Unit>
        {
            public int A { get; set; }

            public int B { get; set; }
        }

        public class Request2 : IRequest<Unit>
        {
            public int A { get; set; }

            public int B { get; set; }
        }

        public class Handler : IRequestHandlerOld<Request, Unit>, IRequestHandlerOld<Request2, Unit>
        {
            public Task<Either<Error, Unit>> Handle(Request request)
            {
                return Task.FromResult<Either<Error, Unit>>(Unit.Default);
            }

            public Task<Either<Error, Unit>> Handle(Request2 request)
            {
                return Task.FromResult<Either<Error, Unit>>(Unit.Default);
            }
        }
    }
}