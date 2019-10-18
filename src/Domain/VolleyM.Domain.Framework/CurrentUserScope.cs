using System;
using VolleyM.Domain.Framework.Authorization;

namespace VolleyM.Domain.Framework
{
    public class CurrentUserScope : IDisposable
    {
        private readonly CurrentUserProvider _currentUserProvider;

        private readonly CurrentUserContext _previousContext;

        internal CurrentUserScope(CurrentUserProvider currentUserProvider, CurrentUserContext scopedContext)
        {
            _currentUserProvider = currentUserProvider;
            
            _previousContext = currentUserProvider.Context;
            _currentUserProvider.Context = scopedContext;
        }

        public void Dispose()
        {
            _currentUserProvider.Context = _previousContext;
        }
    }
}