namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "City", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tournaments", "City");
        }
    }
}
