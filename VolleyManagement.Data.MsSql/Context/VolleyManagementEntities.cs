namespace VolleyManagement.Data.MsSql.Context
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Entities;

    /// <summary>
    /// Volley management database context
    /// </summary>
    public class VolleyManagementEntities : DbContext
    {
        #region Constructor

        /// <summary>
        /// Initializes static members of the <see cref="VolleyManagementEntities"/> class.
        /// </summary>
        static VolleyManagementEntities()
        {
            Database.SetInitializer(new VolleyManagementDatabaseInitializer());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VolleyManagementEntities" /> class.
        /// </summary>
        public VolleyManagementEntities()
            : base("VolleyManagementEntities")
        {
        }

        #endregion

        #region Entity Sets

        /// <summary>
        /// Gets or sets the tournament table.
        /// </summary>
        public DbSet<TournamentEntity> Tournaments { get; set; }

        /// <summary>
        /// Gets or sets the user table.
        /// </summary>
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// Gets or sets the role table.
        /// </summary>
        public DbSet<RoleEntity> Roles { get; set; }

        /// <summary>
        /// Gets or sets the roles to features table.
        /// </summary>
        public DbSet<RoleToOperationEntity> RolesToOperations { get; set; }

        /// <summary>
        /// Gets or sets the user table.
        /// </summary>
        public DbSet<LoginInfoEntity> LoginProviders { get; set; }

        /// <summary>
        /// Gets or sets the player table.
        /// </summary>
        public DbSet<PlayerEntity> Players { get; set; }

        /// <summary>
        /// Gets or sets the contributor table.
        /// </summary>
        public DbSet<ContributorEntity> Contributors { get; set; }

        /// <summary>
        /// Gets or sets the contributor team table.
        /// </summary>
        public DbSet<ContributorTeamEntity> ContributorTeams { get; set; }

        /// <summary>
        /// Gets or sets the team table.
        /// </summary>
        public DbSet<TeamEntity> Teams { get; set; }

        /// <summary>
        /// Gets or sets the divisions table.
        /// </summary>
        public DbSet<DivisionEntity> Divisions { get; set; }

        /// <summary>
        /// Gets or sets the group table.
        /// </summary>
        public DbSet<GroupEntity> Groups { get; set; }

        /// <summary>
        /// Gets or sets the game results table.
        /// </summary>
        public DbSet<GameResultEntity> GameResults { get; set; }

        /// <summary>
        /// Gets or sets the feedback table.
        /// </summary>
        public DbSet<FeedbackEntity> Feedbacks { get; set; }

        /// <summary>
        /// Gets or sets the tournament's requests table.
        /// </summary>
        public DbSet<TournamentRequestEntity> TournamentRequests { get; set; }

        /// <summary>
        /// Gets or sets the request table.
        /// </summary>
        public DbSet<RequestEntity> Requests { get; set; }

        #endregion

        #region Mapping Configuration

        /// <summary>
        /// configure models if needed
        /// </summary>
        /// <param name="modelBuilder">model builder</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            ConfigureTournaments(modelBuilder);
            ConfigureUsers(modelBuilder);
            ConfigureUserLogins(modelBuilder);
            ConfigureRoles(modelBuilder);
            ConfigureUserRoleRelationship(modelBuilder);
            ConfigureRoleToOperations(modelBuilder);
            ConfigurePlayers(modelBuilder);
            ConfigureTeams(modelBuilder);
            ConfigureGroupTeamRelationship(modelBuilder);
            ConfigureContributors(modelBuilder);
            ConfigureContributorTeams(modelBuilder);
            ConfigureDivisions(modelBuilder);
            ConfigureGroups(modelBuilder);
            ConfigureGameResults(modelBuilder);
            ConfigureFeedbacks(modelBuilder);
            ConfigureTournamentRequests(modelBuilder);
            ConfigureRequests(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void ConfigureTournaments(DbModelBuilder modelBuilder)
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

        private static void ConfigureUsers(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .ToTable(VolleyDatabaseMetadata.USERS_TABLE_NAME)
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(ValidationConstants.User.MAX_USER_NAME_LENGTH)
                .IsUnicode()
                .IsVariableLength();

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.Email)
                .IsOptional()
                .HasMaxLength(ValidationConstants.User.MAX_EMAIL_LENGTH)
                .IsUnicode()
                .IsVariableLength();

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.FullName)
                .IsOptional()
                .HasMaxLength(ValidationConstants.User.MAX_FULL_NAME_LENGTH)
                .IsUnicode()
                .IsVariableLength();

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.CellPhone)
                .IsOptional()
                .HasMaxLength(ValidationConstants.User.MAX_PHONE_LENGTH)
                .IsUnicode()
                .IsVariableLength();

            modelBuilder.Entity<UserEntity>()
                .HasMany(u => u.LoginProviders)
                .WithRequired(l => l.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.IsBlocked);
        }

        private static void ConfigureUserLogins(DbModelBuilder modelBuilder)
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

        private static void ConfigureRoles(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleEntity>()
                .ToTable(VolleyDatabaseMetadata.ROLES_TABLE_NAME)
                .HasKey(r => r.Id);

            modelBuilder.Entity<RoleEntity>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(ValidationConstants.Role.MAX_NAME_LENGTH)
                .IsUnicode()
                .IsVariableLength();
        }

        private static void ConfigureRoleToOperations(DbModelBuilder modelBuilder)
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

        private static void ConfigurePlayers(DbModelBuilder modelBuilder)
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

        private static void ConfigureTeams(DbModelBuilder modelBuilder)
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

        private static void ConfigureContributors(DbModelBuilder modelBuilder)
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

        private static void ConfigureContributorTeams(DbModelBuilder modelBuilder)
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

        private static void ConfigureUserRoleRelationship(DbModelBuilder modelBuilder)
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

        private static void ConfigureGroupTeamRelationship(DbModelBuilder modelBuilder)
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

        private static void ConfigureDivisions(DbModelBuilder modelBuilder)
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

        private static void ConfigureGroups(DbModelBuilder modelBuilder)
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

        private static void ConfigureGameResults(DbModelBuilder modelBuilder)
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

        private static void ConfigureFeedbacks(DbModelBuilder modelBuilder)
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

        private static void ConfigureTournamentRequests(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TournamentRequestEntity>()
              .ToTable(VolleyDatabaseMetadata.TOURNAMENT_REQUEST_TABLE_NAME)
              .HasKey(p => p.Id);
        }

        private static void ConfigureRequests(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestEntity>()
               .ToTable(VolleyDatabaseMetadata.REQUESTS_TABLE_NAME)
               .HasKey(p => p.Id);
        }

        #endregion
     }
}
