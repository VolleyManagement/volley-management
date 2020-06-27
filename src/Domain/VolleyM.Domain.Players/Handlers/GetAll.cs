using System.Collections.Generic;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.Players.Handlers
{
	using IQueryObject = IQuery<TenantId, List<PlayerDto>>;

	public class GetAll
	{
		public class Request : IRequest<List<PlayerDto>>
		{
			// no params
		}

		public class Handler : IRequestHandler<Request, List<PlayerDto>>
		{
			private readonly IQueryObject _query;
			private readonly ICurrentUserProvider _currentUser;

			public Handler(IQueryObject query, ICurrentUserProvider currentUser)
			{
				_query = query;
				_currentUser = currentUser;
			}

			public EitherAsync<Error, List<PlayerDto>> Handle(Request request)
			{
				return _query.Execute(_currentUser.Tenant);
			}
		}
	}
}