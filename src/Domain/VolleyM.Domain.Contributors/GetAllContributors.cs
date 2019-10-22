using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Contributors
{
    using IQueryObject = IQuery<Unit, List<ContributorDto>>;

    [Permission(Permissions.GetAll)]
    public class GetAllContributors
    {
        public class Request : IRequest<List<ContributorDto>>
        {
            // no params
        }

        public class Handler : IRequestHandler<Request, List<ContributorDto>>
        {
            private readonly IQueryObject _query;

            public Handler(IQueryObject query) => _query = query;

            public Task<Result<List<ContributorDto>>> Handle(Request request) => _query.Execute(Unit.Value);
        }
    }
}
