namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Add teams to tournaments
    /// </summary>
    public partial class TeamTournamentRelation : DbMigration
    {
        /// <summary>
        /// Migrates up
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.TournamentTeam",
                c => new
                    {
                        TournamentId = c.Int(nullable: false),
                        TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TournamentId, t.TeamId })
                .ForeignKey("dbo.Tournaments", t => t.TournamentId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.TournamentId);
        }

        /// <summary>
        /// Migrates down
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo.TournamentTeam", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.TournamentTeam", "TournamentId", "dbo.Tournaments");
            DropIndex("dbo.TournamentTeam", new[] { "TeamId" });
            DropIndex("dbo.TournamentTeam", new[] { "TournamentId" });
            DropTable("dbo.TournamentTeam");
        }
    }
}
