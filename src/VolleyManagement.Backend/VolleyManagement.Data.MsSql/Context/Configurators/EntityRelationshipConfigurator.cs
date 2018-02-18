namespace VolleyManagement.Data.MsSql.Context.Configurators
{
    using System.Data.Entity;
    using VolleyManagement.Data.MsSql.Entities;

    class EntityRelationshipConfigurator : Interfaces.IEntityRelationshipConfigurator
    {
        public void ConfigureGroupTeamRelationship(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupEntity>()
                .HasMany<TeamEntity>(t => t.Teams)
                .WithMany(t => t.Groups)
                .Map(tt =>
                {
                    tt.MapLeftKey(VolleyDatabaseMetadata.TEAM_TO_GROUP_FK);
                    tt.MapRightKey(VolleyDatabaseMetadata.GROUP_TO_TEAM_FK);
                    tt.ToTable(VolleyDatabaseMetadata.GROUPS_TO_TEAMS_TABLE_NAME);
                });
        }

        public void ConfigureRequests(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestEntity>()
               .ToTable(VolleyDatabaseMetadata.REQUESTS_TABLE_NAME)
               .HasKey(p => p.Id);
        }

        public void ConfigureRoleToOperations(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleToOperationEntity>()
                .ToTable(VolleyDatabaseMetadata.ROLES_TO_OPERATIONS_TABLE_NAME)
                .HasKey(r => r.Id);

            modelBuilder.Entity<RoleToOperationEntity>()
                .Property(r => r.OperationId)
                .IsRequired();

            modelBuilder.Entity<RoleToOperationEntity>()
                .Property(r => r.RoleId)
                .IsRequired();
        }

        public void ConfigureTournamentRequests(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TournamentRequestEntity>()
              .ToTable(VolleyDatabaseMetadata.TOURNAMENT_REQUEST_TABLE_NAME)
              .HasKey(p => p.Id);
        }

        public void ConfigureUserLogins(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginInfoEntity>()
                .ToTable(VolleyDatabaseMetadata.LOGIN_PROVIDERS_TABLE_NAME)
                .HasKey(l => new { l.ProviderKey, l.LoginProvider });

            modelBuilder.Entity<LoginInfoEntity>()
                .Property(l => l.LoginProvider)
                .IsRequired()
                .HasMaxLength(ValidationConstants.User.MAX_LOGIN_PROVIDER_LENGTH)
                .IsUnicode()
                .IsVariableLength();

            modelBuilder.Entity<LoginInfoEntity>()
                .Property(l => l.ProviderKey)
                .IsRequired()
                .HasMaxLength(ValidationConstants.User.MAX_PROVIDER_KEY_LENGTH)
                .IsUnicode()
                .IsVariableLength();
        }

        public void ConfigureUserRoleRelationship(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(m =>
                {
                    m.MapLeftKey(VolleyDatabaseMetadata.ROLE_TO_USER_FK);
                    m.MapRightKey(VolleyDatabaseMetadata.USER_TO_ROLE_FK);
                    m.ToTable(VolleyDatabaseMetadata.USERS_TO_ROLES_TABLE_NAME);
                });
        }
    }
}
