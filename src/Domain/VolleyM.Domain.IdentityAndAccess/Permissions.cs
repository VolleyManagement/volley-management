using System.Reflection.Metadata;

namespace VolleyM.Domain.IdentityAndAccess
{
    //ToDo: Use everywhere
    public static class Permissions
    {
        public static string Context { get; } = "IdentityAndAccess";

        public static class User
        {
            public static string GetUser { get; } = "GetUser";
            public static string CreateUser { get; } = "CreateUser";
        }
    }
}