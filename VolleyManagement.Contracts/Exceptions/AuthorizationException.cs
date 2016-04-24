namespace VolleyManagement.Contracts.Exceptions
{
    using System;

    public class AuthorizationException: Exception
    {
        public AuthorizationException(): base("Requested area is forbidden for current user")
        {
        }
    }
}
