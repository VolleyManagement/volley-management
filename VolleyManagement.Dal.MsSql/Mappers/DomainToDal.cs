namespace VolleyManagement.Dal.MsSql.Mappers
{
    /// <summary>
    /// Maps Domain models to Dal.
    /// </summary>
    public static class DomainToDal
    {
        /// <summary>
        /// Maps Tournament model.
        /// </summary>
        /// <param name="domainTournament">Tournament Domain model</param>
        /// <returns>Tournament Dal model</returns>
        public static Tournament Map(Domain.Tournaments.Tournament domainTournament)
        {
            Tournament tournament = new Tournament();
            tournament.Id = domainTournament.Id;
            tournament.Name = domainTournament.Name;
            tournament.Season = domainTournament.Season;
            tournament.Description = domainTournament.Description;
            tournament.Scheme = (byte)domainTournament.Scheme;
            tournament.RegulationsLink = domainTournament.RegulationsLink;
            return tournament;
        }

        /// <summary>
        /// Maps User model.
        /// </summary>
        /// <param name="domainUser">User Domain model</param>
        /// <returns>User Dal model</returns>
        public static User Map(Domain.Users.User domainUser)
        {
            User user = new User();
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
        public static Player Map(Domain.Players.Player domainPlayer)
        {
            Player player = new Player();
            player.Id = domainPlayer.Id;
            player.FirstName = domainPlayer.FirstName;
            player.LastName = domainPlayer.LastName;
            player.Height = domainPlayer.Height;
            player.Weight = domainPlayer.Weight;
            return player;
        }    
    }
}
