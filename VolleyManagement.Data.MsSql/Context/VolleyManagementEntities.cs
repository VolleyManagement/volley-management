namespace VolleyManagement.Data.MsSql.Context
{
    using System.Data.Entity;

    using VolleyManagement.Data.MsSql.Entities;

    /// <summary>
    /// Volley management database context
    /// </summary>
    internal class VolleyManagementEntities : DbContext
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
        public DbSet<ContributorTeamEntity> ContributorTeam { get; set; }

        /// <summary>
        /// Gets or sets the team table.
        /// </summary>
        public DbSet<TeamEntity> Teams { get; set; }

        #endregion

        #region Mapping Configuration

        /// <summary>
        /// configure models if needed
        /// </summary>
        /// <param name="modelBuilder">model builder</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureTournaments(modelBuilder);
            ConfigureUsers(modelBuilder);
            ConfigureUserLogins(modelBuilder);
            ConfigurePlayers(modelBuilder);
            ConfigureTeams(modelBuilder);
            ConfigureContributors(modelBuilder);
            ConfigureContributorTeams(modelBuilder);

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

        #endregion
    }
}
