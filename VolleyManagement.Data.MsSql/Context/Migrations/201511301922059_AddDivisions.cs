namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Adds divisions for tournaments.
    /// </summary>
    public partial class AddDivisions : DbMigration
    {
        /// <summary>
        /// Migrates up.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.Divisions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 60),
                        TournamentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId);
        }

        /// <summary>
        /// Migrates down.
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo.Divisions", "TournamentId", "dbo.Tournaments");
            DropTable("dbo.Divisions");
        }
    }
}
