namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Away Team state in Game Result model was changed to optional
    /// </summary>
    public partial class NullableAwayTeam : DbMigration
    {
        /// <summary>
        /// Migrates schema up
        /// </summary>
        public override void Up()
        {
            DropIndex("dbo.GameResults", new[] { "AwayTeamId" });
            AlterColumn("dbo.GameResults", "AwayTeamId", c => c.Int());
            CreateIndex("dbo.GameResults", "AwayTeamId");
        }

        /// <summary>
        /// Migrates schema down
        /// </summary>
        public override void Down()
        {
            DropIndex("dbo.GameResults", new[] { "AwayTeamId" });
            AlterColumn("dbo.GameResults", "AwayTeamId", c => c.Int(nullable: false));
            CreateIndex("dbo.GameResults", "AwayTeamId");
        }
    }
}
