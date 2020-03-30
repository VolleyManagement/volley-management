﻿using System.Collections.Generic;
using LanguageExt;

namespace VolleyM.Infrastructure.EventBroker.UnitTests.Fixture
{
    /// <summary>
    /// Will provide unique string for each unique instance it was created
    /// </summary>
    public class ScopeLitmus
    {
        private static IEnumerable<string> GetScopes()
        {
            yield return "rootScope";

            for (int i = 0; i < 100; i++)
            {
                yield return $"eventScope{i + 1}";
            }
        }

        private static readonly IEnumerator<string> _scopes;

        static ScopeLitmus()
        {
            _scopes = GetScopes().GetEnumerator();
            _scopes.MoveNext();
        }

        public ScopeLitmus()
        {
            Scope = _scopes.Current;
            _scopes.MoveNext();
        }

        public string Scope { get; }
    }
}