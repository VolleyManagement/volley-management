namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameStartTimeNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GameResults", "StartTime", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GameResults", "StartTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
