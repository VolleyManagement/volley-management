namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Add game number to game entity
    /// </summary>
    public partial class GameNumberInGameResult : DbMigration
    {
        /// <summary>
        /// Migrate up
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.GameResults", "GameNumber", c => c.Byte(nullable: false));
        }

        /// <summary>
        /// Migrate down
        /// </summary>
        public override void Down()
        {
            DropColumn("dbo.GameResults", "GameNumber");
        }
    }
}
