using AutoMapper;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework
{
	public class DomainFrameworkMappingProfile : Profile
	{
		public DomainFrameworkMappingProfile()
		{
			CreateMap<TenantId, string>()
				.ConvertUsing(t => t.ToString());
		}
	}
}