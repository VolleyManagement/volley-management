using System;
using System.Collections.Generic;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contributors.Contracts;

namespace VolleyM.Infrastructure.Hardcoded
{
    public class GetAllContributorsQuery : IQuery<Null, List<ContributorDto>>
    {
        public List<ContributorDto> Execute(Null param) => new List<ContributorDto> {
            new ContributorDto {FullName = "Dmytro Shapoval", CourseDirection = "All", Team = "Special"},
            new ContributorDto {FullName = "Mykola Bocharskiy", CourseDirection = "All", Team = "Special"},
        };
    }
}
