namespace VolleyManagement.Data.MsSql.Context
{
    using System.Data.Entity;
    using Entities;
    using VolleyManagement.Data.MsSql.Context.Configurators;
    using VolleyManagement.Data.MsSql.Context.Interfaces;
    

    /// <summary>
    /// Volley management database context
    /// </summary>
    public class VolleyManagementEntities : DbContext
    {
        internal IVolleyManagementEntitiesConfigurator VolleyManagementEntitiesConfigurator { get; set; }

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
            VolleyManagementEntitiesConfigurator = new VolleyManagementEntitiesConfigurator();
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
            VolleyManagementEntitiesConfigurator.RemoveManyToManyCascadeConvention(modelBuilder);

            VolleyManagementEntitiesConfigurator.ConfigureUserEnitites(modelBuilder);
            VolleyManagementEntitiesConfigurator.ConfigureGameDataEntities(modelBuilder);
            VolleyManagementEntitiesConfigurator.ConfigureGameParticipantEntities(modelBuilder);
            VolleyManagementEntitiesConfigurator.ConfigureEntityRelationships(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        #endregion
     }
}
