namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrlToGameVideo_Property : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameResults", "UrlToGameVideo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameResults", "UrlToGameVideo");
        }
    }
}
