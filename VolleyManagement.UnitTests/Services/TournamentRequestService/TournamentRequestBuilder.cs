namespace VolleyManagement.UnitTests.Services.TournamentRequestService
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.TournamentRequestAggregate;

    [ExcludeFromCodeCoverage]
    public class TournamentRequestBuilder
    {
        private TournamentRequest _tournamentRequest;

        public TournamentRequestBuilder()
        {
            this._tournamentRequest = new TournamentRequest
            {
                Id = 1,
                TeamId = 1,
                TournamentId = 1,
                UserId = 1
            };
        }

        public TournamentRequestBuilder WithId(int id)
        {
            _tournamentRequest.Id = id;
            return this;
        }

        public TournamentRequestBuilder WithUserId(int userId)
        {
            _tournamentRequest.UserId = userId;
            return this;
        }

        public TournamentRequestBuilder WithTournamentId(int tournamentId)
        {
            _tournamentRequest.TournamentId = tournamentId;
            return this;
        }

        public TournamentRequestBuilder WithTeamId(int teamId)
        {
            _tournamentRequest.TeamId = teamId;
            return this;
        }

        public TournamentRequest Build()
        {
            return _tournamentRequest;
        }
    }
}
