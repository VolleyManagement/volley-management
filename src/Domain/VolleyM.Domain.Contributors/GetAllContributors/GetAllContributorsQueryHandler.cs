using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace VolleyM.Domain.Contributors.GetAllContributors
{
    public class GetAllContributorsQueryHandler : IRequestHandler<GetAllContributorsRequest, List<ContributorDto>>
    {
        public Task<List<ContributorDto>> Handle(GetAllContributorsRequest request, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
