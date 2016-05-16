namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastTimeUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "LastTimeUpdated", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tournaments", "LastTimeUpdated");
        }
    }
}
