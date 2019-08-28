namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class TechnicalDefeatForSet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameResults", "IsSet1TechnicalDefeat", c => c.Boolean(nullable: false));
            AddColumn("dbo.GameResults", "IsSet2TechnicalDefeat", c => c.Boolean(nullable: false));
            AddColumn("dbo.GameResults", "IsSet3TechnicalDefeat", c => c.Boolean(nullable: false));
            AddColumn("dbo.GameResults", "IsSet4TechnicalDefeat", c => c.Boolean(nullable: false));
            AddColumn("dbo.GameResults", "IsSet5TechnicalDefeat", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.GameResults", "IsSet5TechnicalDefeat");
            DropColumn("dbo.GameResults", "IsSet4TechnicalDefeat");
            DropColumn("dbo.GameResults", "IsSet3TechnicalDefeat");
            DropColumn("dbo.GameResults", "IsSet2TechnicalDefeat");
            DropColumn("dbo.GameResults", "IsSet1TechnicalDefeat");
        }
    }
}
