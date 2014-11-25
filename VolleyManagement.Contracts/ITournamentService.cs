namespace SoftServe.VolleyManagement.Contracts
{
    using SoftServe.VolleyManagement.Domain.Tournaments;
    using System.Collections.Generic;
    using System.Linq;

    public interface ITournamentService
    {
        IQueryable<Tournament> GetAllTournaments();
    }
}
