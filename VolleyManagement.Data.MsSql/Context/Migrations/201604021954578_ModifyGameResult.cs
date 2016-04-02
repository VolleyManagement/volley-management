namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyGameResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameResults", "StartTime", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.GameResults", "RoundNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameResults", "RoundNumber");
            DropColumn("dbo.GameResults", "StartTime");
        }
    }
}
