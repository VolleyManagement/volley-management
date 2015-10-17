namespace VolleyManagement.Data.MsSql.Mappers
{
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Domain;
    using VolleyManagement.Domain.TournamentsAggregate;

    /// <summary>
    /// Maps Domain models to Dal.
    /// </summary>
    public static class DomainToDal
    {
        /// <summary>
        /// Maps Tournament model.
        /// </summary>
        /// <param name="entity">Mapping target</param>
        /// <param name="domainTournament">Mapping source</param>
        public static void Map(TournamentEntity entity, Tournament domainTournament)
        {
            entity.Id = domainTournament.Id;
            entity.Name = domainTournament.Name;
            entity.Season = (byte)(domainTournament.Season - ValidationConstants.Tournament.SCHEMA_STORAGE_OFFSET);
            entity.Description = domainTournament.Description;
            entity.Scheme = (byte)domainTournament.Scheme;
            entity.RegulationsLink = domainTournament.RegulationsLink;
            entity.GamesStart = domainTournament.GamesStart;
            entity.GamesEnd = domainTournament.GamesEnd;
            entity.ApplyingPeriodStart = domainTournament.ApplyingPeriodStart;
            entity.ApplyingPeriodEnd = domainTournament.ApplyingPeriodEnd;
            entity.TransferStart = domainTournament.TransferStart;
            entity.TransferEnd = domainTournament.TransferEnd;
        }

        /// <summary>
        /// Maps User model.
        /// </summary>
        /// <param name="domainUser">User Domain model</param>
        /// <returns>User Dal model</returns>
        public static UserEntity Map(Domain.Users.User domainUser)
        {
            UserEntity user = new UserEntity();
            user.Id = domainUser.Id;
            user.FullName = domainUser.FullName;
            user.UserName = domainUser.UserName;
            user.Email = domainUser.Email;
            user.CellPhone = domainUser.CellPhone;
            user.Password = domainUser.Password;
            return user;
        }

        /// <summary>
        /// Maps Player model.
        /// </summary>
        /// <param name="to">Target of the mapping</param>
        /// <param name="from">Source of the mapping</param>
        public static void Map(PlayerEntity to, Domain.PlayersAggregate.Player from)
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
        public static void Map(TeamEntity to, Domain.TeamsAggregate.Team from)
        {
            to.Id = from.Id;
            to.Name = from.Name;
            to.CaptainId = from.CaptainId;
            to.Coach = from.Coach;
            to.Achievements = from.Achievements;
        }
    }
}
