namespace VolleyManagement.Domain.GamesAggregate
{
    using Data.Contracts;

    /// <summary>
    /// Defines a contract for GameRepository.
    /// </summary>
    public interface IGameRepository : IGenericRepository<Game>, IRepository
    {
    }
}