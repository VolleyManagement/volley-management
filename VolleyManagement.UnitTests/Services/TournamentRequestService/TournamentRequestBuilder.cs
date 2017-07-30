namespace VolleyManagement.UnitTests.Services.TournamentRequestService
{
    using System.Diagnostics.CodeAnalysis;
    using Domain.TournamentRequestAggregate;

    [ExcludeFromCodeCoverage]
    public class TournamentRequestBuilder
    {
        private TournamentRequest _tournamentRequest;

        public TournamentRequestBuilder()
        {
            _tournamentRequest = new TournamentRequest
            {
                Id = 1,
                TeamId = 1,
                GroupId = 1,
                UserId = 1,
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

        public TournamentRequestBuilder WithGroupId(int groupId)
        {
            _tournamentRequest.GroupId = groupId;
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
