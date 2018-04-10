namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTournamentLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "Location", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tournaments", "Location");
        }
    }
}
