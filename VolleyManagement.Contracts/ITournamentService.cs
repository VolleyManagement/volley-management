namespace VolleyManagement.Contracts
{
    using System.Linq;

    using VolleyManagement.Domain.Tournaments;

    public interface ITournamentService
    {
        IQueryable<Tournament> GetAllTournaments();
    }
}
