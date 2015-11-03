namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    using VolleyManagement.Data.MsSql.Entities;

    /// <summary>
    /// The volley context configuration.
    /// </summary>
    internal sealed class VolleyContextConfiguration : DbMigrationsConfiguration<VolleyManagementEntities>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolleyContextConfiguration"/> class.
        /// </summary>
        public VolleyContextConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Context\Migrations";
            ContextKey = "VolleyManagement.Data.MsSql.Context.VolleyManagementEntities";
        }

        /// <summary>
        /// This method will be called after migrating to the latest version.
        /// </summary>
        /// <param name="context"> Volley Management context</param>
        protected override void Seed(VolleyManagementEntities context)
        {
            var defaultRoles = new List<RoleEntity>();

            defaultRoles.Add(CreateRole("Administrator"));
            defaultRoles.Add(CreateRole("TournamentAdministrator"));
            defaultRoles.Add(CreateRole("User"));

            context.Roles.AddOrUpdate(
                r => r.Name,
                defaultRoles.ToArray());
        }

        private static RoleEntity CreateRole(string name)
        {
            return new RoleEntity { Name = name };
        }
    }
}
