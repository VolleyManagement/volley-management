namespace VolleyManagement.Data.MsSql.Context.Configurators
{
    using System;
    using System.Data.Entity;
    using Interfaces;
    using System.Data.Entity.ModelConfiguration.Conventions;

    class VolleyManagementEntitiesConfigurator : IVolleyManagementEntitiesConfigurator
    {
        internal IUserEntitiesConfigurator UserEntitiesConfigurator { get; set; }
        internal IEntityRelationshipConfigurator EntityRelationshipConfigurator { get; set; }
        internal IGameDataEntitiesConfigurator GameDataEntitiesConfigurator { get; set; }
        internal IGameParticipantEntitiesConfigurator GameParticipantEntitiesConfigurator { get; set; }

        public void ConfigureEntityRelationships(DbModelBuilder modelBuilder)
        {
            EntityRelationshipConfigurator.ConfigureGroupTeamRelationship(modelBuilder);
            EntityRelationshipConfigurator.ConfigureUserLogins(modelBuilder);
            EntityRelationshipConfigurator.ConfigureUserRoleRelationship(modelBuilder);
            EntityRelationshipConfigurator.ConfigureRoleToOperations(modelBuilder);
            EntityRelationshipConfigurator.ConfigureTournamentRequests(modelBuilder);
            EntityRelationshipConfigurator.ConfigureRequests(modelBuilder);
        }

        public void ConfigureGameDataEntities(DbModelBuilder modelBuilder)
        {
            GameDataEntitiesConfigurator.ConfigureTournaments(modelBuilder);
            GameDataEntitiesConfigurator.ConfigureDivisions(modelBuilder);
            GameDataEntitiesConfigurator.ConfigureGroups(modelBuilder);
            GameDataEntitiesConfigurator.ConfigureGameResults(modelBuilder);
            GameDataEntitiesConfigurator.ConfigureFeedbacks(modelBuilder);
        }

        public void ConfigureGameParticipantEntities(DbModelBuilder modelBuilder)
        {
            GameParticipantEntitiesConfigurator.ConfigurePlayers(modelBuilder);
            GameParticipantEntitiesConfigurator.ConfigureTeams(modelBuilder);
            GameParticipantEntitiesConfigurator.ConfigureContributors(modelBuilder);
            GameParticipantEntitiesConfigurator.ConfigureContributorTeams(modelBuilder);
        }

        public void ConfigureUserEnitites(DbModelBuilder modelBuilder)
        {
            UserEntitiesConfigurator.ConfigureUsers(modelBuilder);
            UserEntitiesConfigurator.ConfigureRoles(modelBuilder);
        }

        public void RemoveManyToManyCascadeConvention(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }
}
