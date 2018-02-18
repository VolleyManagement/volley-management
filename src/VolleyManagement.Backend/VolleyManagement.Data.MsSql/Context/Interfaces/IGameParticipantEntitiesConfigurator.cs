namespace VolleyManagement.Data.MsSql.Context.Interfaces
{
    using System.Data.Entity;

    /// <summary>
    /// Represents functionality, which is related to 
    /// a game itself, mostly targeting participants.
    /// </summary>
    interface IGameParticipantEntitiesConfigurator
    {
        void ConfigureContributors(DbModelBuilder modelBuilder);
        void ConfigureContributorTeams(DbModelBuilder modelBuilder);
        void ConfigurePlayers(DbModelBuilder modelBuilder);
        void ConfigureTeams(DbModelBuilder modelBuilder);
    }
}
