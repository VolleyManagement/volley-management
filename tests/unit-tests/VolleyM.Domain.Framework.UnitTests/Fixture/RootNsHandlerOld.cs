using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace RootNs
{
    public class RootNsHandlerOld
    {
        public class Request : IRequest<Unit>
        {
            public int A { get; set; }

            public int B { get; set; }
        }

        public class Handler : IRequestHandlerOld<RootNsHandlerOld.Request, Unit>
        {
            public Task<Either<Error, Unit>> Handle(Request request)
            {
                return Task.FromResult<Either<Error, Unit>>(Unit.Default);
            }
        }
    }
}