namespace VolleyManagement.Domain.GamesAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// Defines a contract for GameRepository.
    /// </summary>
    public interface IGameRepository : IGenericRepository<Game>
    {
    }
}
