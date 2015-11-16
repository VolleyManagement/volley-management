namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Initial DB migration
    /// </summary>
    public partial class AddDivisions : DbMigration
    {
        /// <summary>
        /// Migrates schema up
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
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .Index(t => t.TournamentId);
        }

        /// <summary>
        /// Migrates schema down
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo.Divisions", "TournamentId", "dbo.Tournaments");
            DropIndex("dbo.Divisions", new[] { "TournamentId" });
            DropTable("dbo.Divisions");
        }
    }
}
