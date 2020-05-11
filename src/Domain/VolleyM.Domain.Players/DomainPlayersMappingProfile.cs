using AutoMapper;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players
{
	public class DomainPlayersMappingProfile : Profile
	{
		public DomainPlayersMappingProfile()
		{
			CreateMap<PlayerId, string>()
				.ConvertUsing(t => t.ToString());
		}
	}
}