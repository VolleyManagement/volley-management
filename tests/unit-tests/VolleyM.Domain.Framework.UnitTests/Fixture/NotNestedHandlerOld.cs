using LanguageExt;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.UnitTests.Fixture
{
    public class NotNestedHandlerOld : IRequestHandlerOld<NotNestedHandlerOld.Request, Unit>
    {
        public class Request : IRequest<Unit>
        {
            public int A { get; set; }

            public int B { get; set; }
        }

        public Task<Either<Error, Unit>> Handle(Request request)
        {
            return Task.FromResult<Either<Error, Unit>>(Unit.Default);
        }
    }
}