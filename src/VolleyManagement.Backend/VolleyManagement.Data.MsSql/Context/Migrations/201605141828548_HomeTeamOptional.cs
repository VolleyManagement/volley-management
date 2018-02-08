namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Change home team to optional
    /// </summary>
    public partial class HomeTeamOptional : DbMigration
    {
        /// <summary>
        /// Migrates schema up
        /// </summary>
        public override void Up()
        {
            DropIndex("dbo.GameResults", new[] { "HomeTeamId" });
            AlterColumn("dbo.GameResults", "HomeTeamId", c => c.Int());
            CreateIndex("dbo.GameResults", "HomeTeamId");
        }

        /// <summary>
        /// Migrates schema down
        /// </summary>
        public override void Down()
        {
            DropIndex("dbo.GameResults", new[] { "HomeTeamId" });
            AlterColumn("dbo.GameResults", "HomeTeamId", c => c.Int(nullable: false));
            CreateIndex("dbo.GameResults", "HomeTeamId");
        }
    }
}
