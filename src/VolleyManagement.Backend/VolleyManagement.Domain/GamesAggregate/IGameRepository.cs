namespace VolleyManagement.Domain.GamesAggregate
{
    using System.Collections.Generic;
    using Data.Contracts;

    /// <summary>
    /// Defines a contract for GameRepository.
    /// </summary>
    public interface IGameRepository : IGenericRepository<Game>
    {
    }
}