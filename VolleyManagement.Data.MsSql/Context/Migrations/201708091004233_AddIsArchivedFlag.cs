namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddIsArchivedFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "IsArchived", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Tournaments", "IsArchived");
        }
    }
}
