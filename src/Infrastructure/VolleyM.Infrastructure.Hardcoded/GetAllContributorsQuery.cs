using System.Collections.Generic;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contributors;

namespace VolleyM.Infrastructure.Hardcoded
{
	public class GetAllContributorsQuery : IQuery<Unit, List<ContributorDto>>
	{
		public EitherAsync<Error, List<ContributorDto>> Execute(Unit param) => new List<ContributorDto> {
				new ContributorDto {FullName = "Dmytro Shapoval", CourseDirection = "All", Team = "Special"},
				new ContributorDto {FullName = "Mykola Bocharskiy", CourseDirection = "All", Team = "Special"},
			};
	}
}