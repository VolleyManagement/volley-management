using System;
using System.Collections.Concurrent;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.Framework.Authorization
{
    /// <summary>
    /// Provide a cache and store for all resolved PermissionAttributes
    /// </summary>
    [Obsolete]
    public class PermissionAttributeMappingStore : ConcurrentDictionary<Type, PermissionAttribute>
    {
    }
}