using System;
using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;

namespace VolleyM.Architecture.UnitTests
{
    public static class PackageStructureConditionExtensions
    {
        public static ConditionList ResideInAllowedNamespace(this Conditions conditions, string parentNs, string[] allowedNames)
        {
            var result = conditions.ResideInNamespace($"{parentNs}.{allowedNames[0]}");

            for (var i = 1; i < allowedNames.Length; i++)
            {
                result = result.Or().ResideInNamespace($"{parentNs}.{allowedNames[i]}");
            }

            return result;
        }

        public static void AssertContextNameIsAllowed(this Assembly assembly, string[] allowedContexts)
        {
            var contextNames = assembly.GetName().Name.Split('.', StringSplitOptions.RemoveEmptyEntries);

            if (contextNames.Length < 3)
            {
                //this is covered by another case
            }
            else
            {
                contextNames[2].Should().BeOneOf(allowedContexts, "Domain packages should use context names");
            }
        }
    }
}