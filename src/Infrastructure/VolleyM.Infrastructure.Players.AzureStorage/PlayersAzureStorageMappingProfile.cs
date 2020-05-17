using AutoMapper;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Infrastructure.Players.AzureStorage
{
	public class PlayersAzureStorageMappingProfile : Profile
	{
		public PlayersAzureStorageMappingProfile()
		{
			EntityToFactoryMap();
			EntityToDtoMap();
		}

		private void EntityToFactoryMap()
		{
			CreateMap<PlayerEntity, PlayerFactoryDto>()
				.ForMember(m => m.Id,
					m => m.MapFrom(src => new PlayerId(src.RowKey)))
				.ForMember(m => m.Tenant,
					m => m.MapFrom(src => new TenantId(src.PartitionKey)))
				.ForMember(m => m.FirstName,
					m => m.MapFrom(pe => pe.FirstName))
				.ForMember(m => m.LastName,
					m => m.MapFrom(pe => pe.LastName));
		}
		private void EntityToDtoMap()
		{
			CreateMap<PlayerEntity, PlayerDto>()
				.ForMember(m => m.FirstName,
					m => m.MapFrom(pe => pe.FirstName))
				.ForMember(m => m.LastName,
					m => m.MapFrom(pe => pe.LastName));
		}
	}
}