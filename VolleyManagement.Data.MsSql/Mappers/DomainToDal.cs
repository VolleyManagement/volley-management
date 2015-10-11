namespace VolleyManagement.Data.MsSql.Mappers
{
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Domain.TournamentsAggregate;

    using Constants = VolleyManagement.Domain.Constants.Tournament;

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
            entity.Season = (byte)(domainTournament.Season - Constants.SCHEMA_STORAGE_OFFSET);
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
        /// <param name="domainPlayer">Player Domain model</param>
        /// <returns>Player Dal model</returns>
        public static PlayerEntity Map(Domain.PlayersAggregate.Player domainPlayer)
        {
            PlayerEntity player = new PlayerEntity();
            player.Id = domainPlayer.Id;
            player.FirstName = domainPlayer.FirstName;
            player.LastName = domainPlayer.LastName;
            player.BirthYear = domainPlayer.BirthYear;
            player.Height = domainPlayer.Height;
            player.Weight = domainPlayer.Weight;
            // player.TeamId = domainPlayer.TeamId;

            return player;
        }

        /// <summary>
        /// Maps Team model.
        /// </summary>
        /// <param name="domainTeam">Team Domain model</param>
        /// <returns>Team Dal model</returns>
        public static TeamEntity Map(Domain.TeamsAggregate.Team domainTeam)
        {
            TeamEntity dalTeam = new TeamEntity();
            dalTeam.Id = domainTeam.Id;
            dalTeam.Name = domainTeam.Name;
            // dalTeam.CaptainId = domainTeam.CaptainId;
            dalTeam.Coach = domainTeam.Coach;
            dalTeam.Achievements = domainTeam.Achievements;

            return dalTeam;
        }
    }
}
