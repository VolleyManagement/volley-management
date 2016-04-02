namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyGameResult : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GameResults", "RoundNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GameResults", "RoundNumber", c => c.Byte(nullable: false));
        }
    }
}
