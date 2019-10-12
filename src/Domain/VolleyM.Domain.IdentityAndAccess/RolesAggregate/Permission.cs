namespace VolleyM.Domain.IdentityAndAccess.RolesAggregate
{
    public class Permission
    {
        /// <summary>
        /// Bounded context permission applies to
        /// </summary>
        public string Context { get; }

        /// <summary>
        /// Action allowed by permission
        /// </summary>
        public string Action { get; }

        /// <summary>
        /// Creates new permission object
        /// </summary>
        /// <param name="context">Bounded context permission applies to</param>
        /// <param name="action">Action allowed by permission</param>
        public Permission(string context, string action)
        {
            Action = action;
            Context = context;
        }

        public override string ToString() => $"{Context}:{Action}";
    }
}