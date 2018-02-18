namespace VolleyManagement.Data.MsSql.Context.Configurators
{
    using System.Data.Entity;
    using VolleyManagement.Data.MsSql.Entities;

    class GameParticipantEntitiesConfigurator : Interfaces.IGameParticipantEntitiesConfigurator
    {
        public void ConfigureContributors(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContributorEntity>()
                .ToTable(VolleyDatabaseMetadata.CONTRIBUTORS_TABLE_NAME)
                .HasKey(ct => ct.Id);

            // Name
            modelBuilder.Entity<ContributorEntity>()
                .Property(ct => ct.Name)
                .IsRequired()
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.Contributor.MAX_NAME_LENGTH);

            // FK
            modelBuilder.Entity<ContributorEntity>()
                .HasRequired(c => c.Team)
                .WithMany(ct => ct.Contributors)
                .Map(m => m.MapKey(VolleyDatabaseMetadata.CONTRIBUTOR_TO_TEAM_FK))
                .WillCascadeOnDelete(false);
        }

        public void ConfigureContributorTeams(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContributorTeamEntity>()
                .ToTable(VolleyDatabaseMetadata.CONTRIBUTOR_TEAMS_TABLE_NAME)
                .HasKey(ct => ct.Id);

            // Name
            modelBuilder.Entity<ContributorTeamEntity>()
                .Property(ct => ct.Name)
                .IsRequired()
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.Contributor.MAX_TEAM_NAME_LENGTH);

            // Course Direction
            modelBuilder.Entity<ContributorTeamEntity>()
                .Property(ct => ct.CourseDirection)
                .IsRequired()
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.Contributor.MAX_COURSE_NAME_LENGTH);
        }

        public void ConfigurePlayers(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerEntity>()
                .ToTable(VolleyDatabaseMetadata.PLAYERS_TABLE_NAME)
                .HasKey(p => p.Id);

            // First name
            modelBuilder.Entity<PlayerEntity>()
                .Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(ValidationConstants.Player.MAX_FIRST_NAME_LENGTH)
                .IsUnicode()
                .IsVariableLength();

            // Last name
            modelBuilder.Entity<PlayerEntity>()
                .Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(ValidationConstants.Player.MAX_LAST_NAME_LENGTH)
                .IsUnicode()
                .IsVariableLength();

            // Birth Year
            modelBuilder.Entity<PlayerEntity>()
                .Property(p => p.BirthYear)
                .HasColumnType(VolleyDatabaseMetadata.SMALL_INT_COLUMN_TYPE)
                .IsOptional();

            // Height
            modelBuilder.Entity<PlayerEntity>()
                .Property(p => p.Height)
                .HasColumnType(VolleyDatabaseMetadata.SMALL_INT_COLUMN_TYPE)
                .IsOptional();

            // Weight
            modelBuilder.Entity<PlayerEntity>()
                .Property(p => p.Weight)
                .HasColumnType(VolleyDatabaseMetadata.SMALL_INT_COLUMN_TYPE)
                .IsOptional();
        }

        public void ConfigureTeams(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamEntity>()
                .ToTable(VolleyDatabaseMetadata.TEAMS_TABLE_NAME)
                .HasKey(p => p.Id);

            // Name
            modelBuilder.Entity<TeamEntity>()
                .Property(t => t.Name)
                .IsRequired()
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.Team.MAX_NAME_LENGTH);

            // Coach
            modelBuilder.Entity<TeamEntity>()
                .Property(t => t.Coach)
                .IsOptional()
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.Team.MAX_COACH_NAME_LENGTH);

            // Achievements
            modelBuilder.Entity<TeamEntity>()
                .Property(t => t.Achievements)
                .IsOptional()
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.Team.MAX_ACHIEVEMENTS_LENGTH);

            // FK Team -> Players
            modelBuilder.Entity<TeamEntity>()
                .HasMany(t => t.Players)
                .WithOptional(p => p.Team)
                .HasForeignKey(p => p.TeamId)
                .WillCascadeOnDelete(false);

            // FK Team - Captain
            modelBuilder.Entity<TeamEntity>()
                .HasRequired(t => t.Captain)
                .WithMany(p => p.LedTeam)
                .HasForeignKey(t => t.CaptainId)
                .WillCascadeOnDelete(false);
        }
    }
}
