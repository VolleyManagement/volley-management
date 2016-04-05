namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    /// Adds round number and start time to game result
    /// </summary>
    public partial class ModifyGameResult : DbMigration
    {
        /// <summary>
        /// Migrates up
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.GameResults", "StartTime", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.GameResults", "RoundNumber", c => c.Int(nullable: false));
        }
        
        /// <summary>
        /// Migrates down 
        /// </summary>
        public override void Down()
        {
            DropColumn("dbo.GameResults", "RoundNumber");
            DropColumn("dbo.GameResults", "StartTime");
        }
    }
}
