namespace VolleyM.Domain.Contributors
{
    public static class Permissions
    {
        public static string Context { get; } = "Contributors";

        public const string GetAll = "GetAll";
    }

    internal class PermissionAttribute : Contracts.Crosscutting.PermissionAttribute
    {
        public PermissionAttribute(string action) : base(Permissions.Context, action)
        {
        }
    }
}