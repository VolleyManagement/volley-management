using AutoMapper;
using VolleyM.Domain.Contributors;

namespace VolleyM.API.Contributors
{
    public class ContributorsApiMappingProfile : Profile
    {
        public ContributorsApiMappingProfile()
        {
            CreateMap<ContributorDto, Contributor>();
        }
    }
}