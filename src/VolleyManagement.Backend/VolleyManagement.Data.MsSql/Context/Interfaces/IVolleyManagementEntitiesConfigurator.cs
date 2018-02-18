namespace VolleyManagement.Data.MsSql.Context.Interfaces
{
    using System.Data.Entity;

    interface IVolleyManagementEntitiesConfigurator
    {
        void ConfigureUserEnitites(DbModelBuilder modelBuilder);
        void ConfigureGameDataEntities(DbModelBuilder modelBuilder);
        void ConfigureGameParticipantEntities(DbModelBuilder modelBuilder);
        void ConfigureEntityRelationships(DbModelBuilder modelBuilder);
    }
}
