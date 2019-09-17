using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contributors;

namespace VolleyM.Infrastructure.Hardcoded
{
    public class GetAllContributorsQuery : IQuery<Unit, List<ContributorDto>>
    {
        public Task<Result<List<ContributorDto>>> Execute(Unit param) =>
            Task.FromResult<Result<List<ContributorDto>>>(new List<ContributorDto> {
                new ContributorDto {FullName = "Dmytro Shapoval", CourseDirection = "All", Team = "Special"},
                new ContributorDto {FullName = "Mykola Bocharskiy", CourseDirection = "All", Team = "Special"},
            });
    }
}
