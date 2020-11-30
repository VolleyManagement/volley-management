namespace VolleyManagement.Data.MsSql.Context.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class AddScoreScheme : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Tournaments", "ScoreSchemeCode", c => c.Byte(nullable: false, defaultValue: 1, defaultValueSql: "1"));
		}

		public override void Down()
		{
			DropColumn("dbo.Tournaments", "ScoreSchemeCode");
		}
	}
}
