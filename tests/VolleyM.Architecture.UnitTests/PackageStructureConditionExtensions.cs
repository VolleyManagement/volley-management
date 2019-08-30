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
    }
}