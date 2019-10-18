using System;

namespace VolleyM.Domain.Contracts.Crosscutting
{
    public class PermissionAttribute : Attribute
    {
        public string Context { get; }
        public string Action { get; }

        public PermissionAttribute(string context, string action)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Context = context;
            Action = action;
        }
    }
}