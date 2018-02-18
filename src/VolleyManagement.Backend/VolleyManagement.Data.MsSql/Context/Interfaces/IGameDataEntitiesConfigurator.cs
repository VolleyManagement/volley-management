namespace VolleyManagement.Data.MsSql.Context.Interfaces
{
    using System.Data.Entity;

    /// <summary>
    /// Represents functionality, which is related to
    /// a game, mostly targeting info about it.
    /// </summary>
    interface IGameDataEntitiesConfigurator
    {
        void ConfigureDivisions(DbModelBuilder modelBuilder);
        void ConfigureFeedbacks(DbModelBuilder modelBuilder);
        void ConfigureGameResults(DbModelBuilder modelBuilder);
        void ConfigureGroups(DbModelBuilder modelBuilder);
        void ConfigureTournaments(DbModelBuilder modelBuilder);
    }
}
