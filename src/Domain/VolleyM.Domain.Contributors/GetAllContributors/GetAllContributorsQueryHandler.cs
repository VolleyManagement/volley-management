using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Contributors.GetAllContributors
{
    public class GetAllContributorsQueryHandler : IRequestHandler<GetAllContributorsRequest, List<ContributorDto>>
    {
        private readonly IQuery<Null, List<ContributorDto>> _query;

        public GetAllContributorsQueryHandler(IQuery<Null, List<ContributorDto>> query)
        {
            _query = query;
        }

        public Task<List<ContributorDto>> Handle(GetAllContributorsRequest request, CancellationToken cancellationToken)
        {
            return _query.Execute(Null.Value);
        }
    }
}
