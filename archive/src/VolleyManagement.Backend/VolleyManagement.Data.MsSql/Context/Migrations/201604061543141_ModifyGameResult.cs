namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Added round number and start time
    /// </summary>
    public partial class ModifyGameResult : DbMigration
    {
        /// <summary>
        /// Migrate up
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.GameResults", "StartTime", c => c.DateTime(nullable: false, precision: 0, storeType: "datetime2"));
            AddColumn("dbo.GameResults", "RoundNumber", c => c.Byte(nullable: false));
        }

        /// <summary>
        /// Migrate down
        /// </summary>
        public override void Down()
        {
            DropColumn("dbo.GameResults", "RoundNumber");
            DropColumn("dbo.GameResults", "StartTime");
        }
    }
}
