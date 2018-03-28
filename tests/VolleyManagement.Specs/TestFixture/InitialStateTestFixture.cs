using System.Linq;
using VolleyManagement.Data.MsSql.Entities;
using VolleyManagement.Specs.Infrastructure;

namespace VolleyManagement.Specs.TestFixture
{
    public static class InitialStateTestFixture
    {
        public static void ConfigureTestRun()
        {
            using (var ctx = TestDbAdapter.Context)
            {
                var adminRole = ctx.Roles.Single(r => r.Name == "Administrator");

                var admin = new UserEntity {
                    UserName = "Admin",
                    Email = "admin@volley-mgmt.org.ua",
                    FullName = "Admin Name"
                };
                adminRole.Users.Add(admin);

                ctx.SaveChanges();
            }
        }
    }
}