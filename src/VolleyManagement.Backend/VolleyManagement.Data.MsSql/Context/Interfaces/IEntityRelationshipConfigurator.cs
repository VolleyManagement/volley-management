namespace VolleyManagement.Data.MsSql.Context.Interfaces
{
    using System.Data.Entity;

    interface IEntityRelationshipConfigurator
    {
        void ConfigureGroupTeamRelationship(DbModelBuilder modelBuilder);
        void ConfigureUserLogins(DbModelBuilder modelBuilder);
        void ConfigureUserRoleRelationship(DbModelBuilder modelBuilder);
        void ConfigureRoleToOperations(DbModelBuilder modelBuilder);
        void ConfigureTournamentRequests(DbModelBuilder modelBuilder);
        void ConfigureRequests(DbModelBuilder modelBuilder);
    }
}
