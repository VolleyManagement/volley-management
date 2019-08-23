using System.Collections.Generic;
using MediatR;

namespace VolleyM.Domain.Contributors.GetAllContributors
{
    public class GetAllContributorsRequest : IRequest<List<ContributorDto>>
    {
        // no params
    }
}