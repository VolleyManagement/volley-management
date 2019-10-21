// ReSharper disable InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
namespace VolleyM.Domain.IdentityAndAccess
{
    public static class Permissions
    {
        public static string Context { get; } = "IdentityAndAccess";

        public static class User
        {
            public const string GetUser = "GetUser";
            public const string CreateUser = "CreateUser";
        }
    }

    internal class PermissionAttribute : Contracts.Crosscutting.PermissionAttribute
    {
        public PermissionAttribute(string action) : base(Permissions.Context, action)
        {
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles