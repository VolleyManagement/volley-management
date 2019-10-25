using System;
using System.Collections.Generic;
using System.Text;

namespace VolleyM.Domain.Players
{
    public static class Permissions
    {
        public static string Context { get; } = "Players";

        public const string GetAll = "GetAll";
    }

    internal class PermissionAttribute : Contracts.Crosscutting.PermissionAttribute
    {
        public PermissionAttribute(string action) : base(Permissions.Context, action)
        {
        }
    }
}
