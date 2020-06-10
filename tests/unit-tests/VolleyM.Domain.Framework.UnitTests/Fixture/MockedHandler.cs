using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.UnitTests.Fixture
{
    public class MockedHandler : IRequestHandlerOld<MockedHandler.Request, Unit>
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