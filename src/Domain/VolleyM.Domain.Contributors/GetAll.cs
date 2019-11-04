using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Contributors
{
    using IQueryObject = IQuery<Unit, List<ContributorDto>>;

    public class GetAll
    {
        public class Request : IRequest<List<ContributorDto>>
        {
            // no params
        }

        public class Handler : IRequestHandler<Request, List<ContributorDto>>
        {
            private readonly IQueryObject _query;

            public Handler(IQueryObject query) => _query = query;

            public Task<Either<Error, List<ContributorDto>>> Handle(Request request) => _query.Execute(Unit.Default);
        }
    }
}
