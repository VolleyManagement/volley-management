using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Contributors
{
    using IQueryObject = IQueryOld<Unit, List<ContributorDto>>;

	[Obsolete]
    public class GetAllOld
    {
        public class Request : IRequest<List<ContributorDto>>
        {
            // no params
        }

        public class Handler : IRequestHandlerOld<Request, List<ContributorDto>>
        {
            private readonly IQueryObject _query;

            public Handler(IQueryObject query) => _query = query;

            public Task<Either<Error, List<ContributorDto>>> Handle(Request request) => _query.Execute(Unit.Default);
        }
    }
}
