using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.Players.Handlers
{
	using IQueryObject = IQueryOld<TenantId, List<PlayerDto>>;

	public class GetAll
    {
        public class Request : IRequest<List<PlayerDto>>
        {
            // no params
        }
        public class Handler : IRequestHandlerOld<Request, List<PlayerDto>>
        {
            private readonly IQueryObject _query;
            private readonly ICurrentUserProvider _currentUser;

            public Handler(IQueryObject query, ICurrentUserProvider currentUser)
            {
                _query = query;
                _currentUser = currentUser;
            }

            public Task<Either<Error, List<PlayerDto>>> Handle(Request request)
            {
                return _query.Execute(_currentUser.Tenant);
            }
        }
    }
}
