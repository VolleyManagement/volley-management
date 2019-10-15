using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Players.Handlers
{
    using IQueryObject = IQuery<TenantId, List<PlayerDto>>;

    public class GetAllPlayers
    {
        public class Request : IRequest<List<PlayerDto>>
        {
            // no params
        }
        public class Handler : IRequestHandler<Request, List<PlayerDto>>
        {
            private readonly IQueryObject _query;

            public Handler(IQueryObject query)
            {
                _query = query;
            }

            public Task<Result<List<PlayerDto>>> Handle(Request request)
            {
                throw new NotImplementedException();
            }
        }
    }
}
