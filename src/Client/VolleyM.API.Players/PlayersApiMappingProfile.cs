using AutoMapper;

namespace VolleyM.API.Players
{
	public class PlayersApiMappingProfile : Profile
	{
		public PlayersApiMappingProfile()
		{
			CreateMap<Domain.Players.PlayerAggregate.Player, Player>();
		}
	}
}