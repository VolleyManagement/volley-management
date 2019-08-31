using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentAssertions;
using NetArchTest.Rules;
using NetArchTest.Rules.Policies;
using Xunit;

namespace VolleyM.Architecture.UnitTests
{
    public static class TestExtensions
    {
        public static PredicateList ThatNotCompilerGenerated(this Types types)
        {
            return types.That().DoNotHaveCustomAttribute(typeof(CompilerGeneratedAttribute));
        }

        public static void AssertHasNoViolations(this PolicyResults result)
        {
            var lines = new List<string> {
                $"{result.Description}",
                $"{result.Name} policy has been violated by following types:"
            };


            static string GetFirstViolators(IEnumerable<Type> failingTypes)
            {
                return string.Join(',', failingTypes.Take(3).Select(t => t.FullName).Union(new[] { "..." }));
            }

            lines.AddRange(
                result.Results
                                .Where(r => !r.IsSuccessful)
                                .Select(policyResult => policyResult.Name + ": " + GetFirstViolators(policyResult.FailingTypes)));

            result.HasViolations.Should().BeFalse(string.Join(Environment.NewLine, lines));
        }
    }
}