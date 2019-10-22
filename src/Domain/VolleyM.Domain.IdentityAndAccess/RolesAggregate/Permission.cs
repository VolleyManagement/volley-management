using System;

namespace VolleyM.Domain.IdentityAndAccess.RolesAggregate
{
    public class Permission : IEquatable<Permission>
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

        public bool Equals(Permission other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(ToString(), other.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Permission)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Context != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Context) : 0) * 397) ^ (Action != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Action) : 0);
            }
        }

        public static bool operator ==(Permission left, Permission right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Permission left, Permission right)
        {
            return !Equals(left, right);
        }
    }
}