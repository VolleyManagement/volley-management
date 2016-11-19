namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Added UserEnvironment field to Feedback.
    /// </summary>
    public partial class AddedUserEnvironmentToFeedback : DbMigration
    {
        /// <summary>
        /// The up migration.
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.Feedbacks", "UserEnvironment", c => c.String());
        }

        /// <summary>
        /// The down migration.
        /// </summary>
        public override void Down()
        {
            DropColumn("dbo.Feedbacks", "UserEnvironment");
        }
    }
}
