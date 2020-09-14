﻿namespace VolleyM.Domain.Players.PlayerAggregate
{
	public sealed class PlayerFactory
	{
		public Player Create(PlayerFactoryDto dto)
		{
			return new Player(dto.Tenant,dto.Version, dto.Id, dto.FirstName, dto.LastName);
		}
	}
}