namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class GamePenalty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameResults", "PenaltyTeam", c => c.Byte(nullable: false));
            AddColumn("dbo.GameResults", "PenaltyAmount", c => c.Byte(nullable: false));
            AddColumn("dbo.GameResults", "PenaltyDescription", c => c.String(maxLength: 255));
        }

        public override void Down()
        {
            DropColumn("dbo.GameResults", "PenaltyDescription");
            DropColumn("dbo.GameResults", "PenaltyAmount");
            DropColumn("dbo.GameResults", "PenaltyTeam");
        }
    }
}
