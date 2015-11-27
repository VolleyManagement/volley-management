namespace VolleyManagement.Data.MsSql.Mappers
{
    using System.Linq;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Domain.GameResultsAggregate;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.Domain.UsersAggregate;

    /// <summary>
    /// Maps Domain models to Dal.
    /// </summary>
    public static class DomainToDal
    {
        /// <summary>
        /// Maps Tournament model.
        /// </summary>
        /// <param name="to">Mapping target</param>
        /// <param name="from">Mapping source</param>
        public static void Map(TournamentEntity to, Tournament from)
        {
            to.Id = from.Id;
            to.Name = from.Name;
            to.Season = (byte)(from.Season - ValidationConstants.Tournament.SCHEMA_STORAGE_OFFSET);
            to.Description = from.Description;
            to.Scheme = (byte)from.Scheme;
            to.RegulationsLink = from.RegulationsLink;
            to.GamesStart = from.GamesStart;
            to.GamesEnd = from.GamesEnd;
            to.ApplyingPeriodStart = from.ApplyingPeriodStart;
            to.ApplyingPeriodEnd = from.ApplyingPeriodEnd;
            to.TransferStart = from.TransferStart;
            to.TransferEnd = from.TransferEnd;
        }

        /// <summary>
        /// Maps User model.
        /// </summary>
        /// <param name="to">User Entity model</param>
        /// <param name="from">User Domain model</param>
        public static void Map(UserEntity to, User from)
        {
            to.Id = from.Id;
            to.FullName = from.PersonName;
            to.UserName = from.UserName;
            to.Email = from.Email;
            to.CellPhone = from.PhoneNumber;
            to.LoginProviders = from.LoginProviders
                .Select(l => new LoginInfoEntity
                                     {
                                         LoginProvider = l.LoginProvider,
                                         ProviderKey = l.ProviderKey
                                     })
                                     .ToList();
        }

        /// <summary>
        /// Maps Player model.
        /// </summary>
        /// <param name="to">Target of the mapping</param>
        /// <param name="from">Source of the mapping</param>
        public static void Map(PlayerEntity to, Player from)
        {
            to.Id = from.Id;
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.BirthYear = from.BirthYear;
            to.Height = from.Height;
            to.Weight = from.Weight;
            to.TeamId = from.TeamId;
        }

        /// <summary>
        /// Maps Team model.
        /// </summary>
        /// <param name="to">Team Entity model</param>
        /// <param name="from">Team Domain model</param>
        public static void Map(TeamEntity to, Team from)
        {
            to.Id = from.Id;
            to.Name = from.Name;
            to.CaptainId = from.CaptainId;
            to.Coach = from.Coach;
            to.Achievements = from.Achievements;
        }

        /// <summary>
        /// Maps DAL model of game result to domain model of game result.
        /// </summary>
        /// <param name="to">DAL model of game result.</param>
        /// <param name="from">Domain model of game result.</param>
        public static void Map(GameResultEntity to, GameResult from)
        {
            to.Id = from.Id;
            to.TournamentId = from.TournamentId;
            to.HomeTeamId = from.HomeTeamId;
            to.AwayTeamId = from.AwayTeamId;
            to.HomeSetsScore = from.HomeSetsScore;
            to.AwaySetsScore = from.AwaySetsScore;
            to.IsTechnicalDefeat = from.IsTechnicalDefeat;
            to.HomeSet1Score = from.HomeSet1Score;
            to.AwaySet1Score = from.AwaySet1Score;
            to.HomeSet2Score = from.HomeSet2Score;
            to.AwaySet2Score = from.AwaySet2Score;
            to.HomeSet3Score = from.HomeSet3Score;
            to.AwaySet3Score = from.AwaySet3Score;
            to.HomeSet4Score = from.HomeSet4Score;
            to.AwaySet4Score = from.AwaySet4Score;
            to.HomeSet5Score = from.HomeSet5Score;
            to.AwaySet5Score = from.AwaySet5Score;
        }
    }
}
