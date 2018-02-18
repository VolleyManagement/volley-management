namespace VolleyManagement.Data.MsSql.Context.Configurators
{
    using System.Data.Entity;
    using VolleyManagement.Data.MsSql.Entities;

    class GameDataEntitiesConfigurator : Interfaces.IGameDataEntitiesConfigurator
    {
        public void ConfigureDivisions(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DivisionEntity>()
                .ToTable(VolleyDatabaseMetadata.DIVISION_TABLE_NAME)
                .HasKey(d => d.Id);

            modelBuilder.Entity<DivisionEntity>()
                .Property(d => d.Name)
                .IsRequired()
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.Division.MAX_DIVISION_NAME_LENGTH);
        }

        public void ConfigureFeedbacks(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedbackEntity>()
               .ToTable(VolleyDatabaseMetadata.FEEDBACKS_TABLE_NAME)
               .HasKey(p => p.Id);

            // UsersEmail
            modelBuilder.Entity<FeedbackEntity>()
                .Property(t => t.UsersEmail)
                .IsRequired()
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.Feedback.MAX_EMAIL_LENGTH);

            // Content
            modelBuilder.Entity<FeedbackEntity>()
                .Property(t => t.Content)
                .IsRequired()
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.Feedback.MAX_CONTENT_LENGTH);

            // Date
            modelBuilder.Entity<FeedbackEntity>()
                .Property(t => t.Date)
                .IsRequired()
                .HasColumnType(VolleyDatabaseMetadata.DATETIME2_COLUMN_TYPE);

            // Status
            modelBuilder.Entity<FeedbackEntity>()
                .Property(t => t.Status)
                .IsRequired()
                .HasColumnName(VolleyDatabaseMetadata.FEEDBACK_STATUS_COLUMN_NAME);

            // UserEnvironment
            modelBuilder.Entity<FeedbackEntity>()
                .Property(t => t.UserEnvironment)
                .IsOptional();

            // Update Date
            modelBuilder.Entity<FeedbackEntity>()
                .Property(t => t.UpdateDate)
                .HasColumnType(VolleyDatabaseMetadata.DATETIME2_COLUMN_TYPE);

            // Admin Name
            modelBuilder.Entity<FeedbackEntity>()
                .Property(t => t.AdminName)
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.User.MAX_FULL_NAME_LENGTH);
        }

        public void ConfigureGameResults(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameResultEntity>()
                .ToTable(VolleyDatabaseMetadata.GAME_RESULTS_TABLE_NAME)
                .HasKey(gr => gr.Id);

            modelBuilder.Entity<GameResultEntity>()
                .Property(gr => gr.StartTime)
                .IsOptional()
                .HasColumnType(VolleyDatabaseMetadata.DATETIME2_COLUMN_TYPE);

            modelBuilder.Entity<GameResultEntity>()
                .Property(gr => gr.RoundNumber)
                .IsRequired()
                .HasColumnType(VolleyDatabaseMetadata.TINYINT_COLUMN_TYPE);

            modelBuilder.Entity<GameResultEntity>()
                .Property(gr => gr.GameNumber)
                .HasColumnType(VolleyDatabaseMetadata.TINYINT_COLUMN_TYPE);

            modelBuilder.Entity<GameResultEntity>()
                .Property(gr => gr.PenaltyTeam)
                .IsRequired()
                .HasColumnType(VolleyDatabaseMetadata.TINYINT_COLUMN_TYPE);

            modelBuilder.Entity<GameResultEntity>()
                .Property(gr => gr.PenaltyAmount)
                .IsRequired()
                .HasColumnType(VolleyDatabaseMetadata.TINYINT_COLUMN_TYPE);

            modelBuilder.Entity<GameResultEntity>()
                .Property(gr => gr.PenaltyDescription)
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.GameResult.MAX_PENALTY_DESCRIPTION_LENGTH);

            // FK GameResult -> Tournament
            modelBuilder.Entity<GameResultEntity>()
                .HasRequired(gr => gr.Tournament)
                .WithMany(t => t.GameResults)
                .HasForeignKey(gr => gr.TournamentId)
                .WillCascadeOnDelete(false);

            // FK GameResult -> HomeTeam
            modelBuilder.Entity<GameResultEntity>()
                .HasOptional(gr => gr.HomeTeam)
                .WithMany(t => t.HomeGameResults)
                .HasForeignKey(gr => gr.HomeTeamId)
                .WillCascadeOnDelete(false);

            // FK GameResult -> AwayTeam
            modelBuilder.Entity<GameResultEntity>()
                .HasOptional(gr => gr.AwayTeam)
                .WithMany(t => t.AwayGameResults)
                .HasForeignKey(gr => gr.AwayTeamId)
                .WillCascadeOnDelete(false);
        }

        public void ConfigureGroups(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupEntity>()
                .ToTable(VolleyDatabaseMetadata.GROUPS_TABLE_NAME)
                .HasKey(g => g.Id);

            // Name
            modelBuilder.Entity<GroupEntity>()
                .Property(g => g.Name)
                .IsRequired()
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.Group.MAX_GROUP_NAME_LENGTH);

            // FK Group - Division
            modelBuilder.Entity<GroupEntity>()
                .HasRequired(g => g.Division)
                .WithMany(d => d.Groups)
                .HasForeignKey(g => g.DivisionId)
                .WillCascadeOnDelete(false);
        }

        public void ConfigureTournaments(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TournamentEntity>()
                .ToTable(VolleyDatabaseMetadata.TOURNAMENTS_TABLE_NAME)
                .HasKey(t => t.Id);

            // Name
            modelBuilder.Entity<TournamentEntity>()
                .Property(t => t.Name)
                .IsRequired()
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.Tournament.MAX_NAME_LENGTH);

            // Scheme
            modelBuilder.Entity<TournamentEntity>()
                .Property(t => t.Scheme)
                .IsRequired()
                .HasColumnName(VolleyDatabaseMetadata.TOURNAMENT_SCHEME_COLUMN_NAME);

            // Season
            modelBuilder.Entity<TournamentEntity>()
                .Property(t => t.Season)
                .IsRequired()
                .HasColumnName(VolleyDatabaseMetadata.TOURNAMENT_SEASON_COLUMN_NAME);

            // Description
            modelBuilder.Entity<TournamentEntity>()
                .Property(t => t.Description)
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.Tournament.MAX_DESCRIPTION_LENGTH);

            // Regulations link
            modelBuilder.Entity<TournamentEntity>()
                .Property(t => t.RegulationsLink)
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(ValidationConstants.Tournament.MAX_URL_LENGTH);

            // Applying Period Start
            modelBuilder.Entity<TournamentEntity>()
                .Property(t => t.ApplyingPeriodStart)
                .IsRequired()
                .HasColumnType(VolleyDatabaseMetadata.DATE_COLUMN_TYPE);

            // Applying Period End
            modelBuilder.Entity<TournamentEntity>()
                .Property(t => t.ApplyingPeriodEnd)
                .IsRequired()
                .HasColumnType(VolleyDatabaseMetadata.DATE_COLUMN_TYPE);

            // Games Start
            modelBuilder.Entity<TournamentEntity>()
                .Property(t => t.GamesStart)
                .IsRequired()
                .HasColumnType(VolleyDatabaseMetadata.DATE_COLUMN_TYPE);

            // Games End
            modelBuilder.Entity<TournamentEntity>()
                .Property(t => t.GamesEnd)
                .IsRequired()
                .HasColumnType(VolleyDatabaseMetadata.DATE_COLUMN_TYPE);

            // Transfer Start
            modelBuilder.Entity<TournamentEntity>()
                .Property(t => t.TransferStart)
                .HasColumnType(VolleyDatabaseMetadata.DATE_COLUMN_TYPE);

            // Transfer End
            modelBuilder.Entity<TournamentEntity>()
                .Property(t => t.TransferEnd)
                .HasColumnType(VolleyDatabaseMetadata.DATE_COLUMN_TYPE);

            modelBuilder.Entity<TournamentEntity>()
                .HasMany(d => d.Divisions)
                .WithRequired(d => d.Tournament)
                .HasForeignKey(d => d.TournamentId)
                .WillCascadeOnDelete(false);
        }
    }
}
