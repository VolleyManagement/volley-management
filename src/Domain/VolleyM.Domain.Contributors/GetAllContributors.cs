using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Contributors
{
    public class GetAllContributors
    {
        public class Request : IRequest<List<ContributorDto>>
        {
            // no params
        }

        public class Handler : IRequestHandler<Request, List<ContributorDto>>
        {
            private readonly IQueryObject _query;

            public Handler(IQueryObject query)
            {
                _query = query;
            }

            public Task<List<ContributorDto>> Handle(Request request)
            {
                Log.Information("Handler {Handler} action called.", nameof(GetAllContributors));
                return _query.Execute(Unit.Value);
            }
        }

        public interface IQueryObject : IQuery<Unit, List<ContributorDto>>
        {

        }
    }
}
