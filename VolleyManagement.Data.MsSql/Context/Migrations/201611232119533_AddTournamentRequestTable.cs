namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Added new tournament's request table
    /// </summary>
    public partial class AddTournamentRequestTable : DbMigration
    {
        /// <summary>
        /// The up migration.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.TournamentRequests",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.Int(nullable: false),
                    TournamentId = c.Int(nullable: false),
                    TeamId = c.Int(nullable: false),
                })
                    .PrimaryKey(t => t.Id);
        }

        /// <summary>
        /// The down migration.
        /// </summary>
        public override void Down()
        {
            DropTable("dbo.TournamentRequests");
        }
    }
}
